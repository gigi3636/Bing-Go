using Godot;
using System;

public partial class ShakeCamera2D : Camera2D
{
    public static ShakeCamera2D Instance { get; private set; }

    [ExportGroup("Parametres de Secousse (Trauma)")]
    [Export(PropertyHint.Range, "0,1,0.01")]
    public float Decay = 2.5f; // Augmente : la secousse s arrete plus vite pour un effet sec
    [Export]
    public Vector2 MaxOffset = new Vector2(8, 8); // Reduit pour le pixel art
    [Export]
    public float MaxRoll = 0.05f; // Rotation tres legere
    [Export]
    public float TraumaPower = 2.0f;

    [ExportGroup("Parametres du Bruit (Noise)")]
    [Export]
    public float NoiseSpeed = 40f;
    [Export]
    public int NoiseSeed = 0;

    private float _trauma = 0.0f;
    private float _time = 0.0f;
    private FastNoiseLite _noise;

    private Vector2 _baseOffset;
    private float _baseRotation;

    // NOUVEAU : Stocke le decalage du coup (tampon/recul)
    private Vector2 _impactOffset = Vector2.Zero;

    public override void _EnterTree()
    {
        if (Instance == null) Instance = this;
        else QueueFree();
    }

    public override void _Ready()
    {
        _baseOffset = Offset;
        _baseRotation = Rotation;

        _noise = new FastNoiseLite();
        _noise.Seed = (NoiseSeed == 0) ? (int)GD.Randi() : NoiseSeed;
        _noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        _noise.FractalOctaves = 4;
    }

    public override void _Process(double delta)
    {
        // 1. Amortir l impact directionnel tres rapidement (effet ressort)
        _impactOffset = _impactOffset.Lerp(Vector2.Zero, (float)delta * 15f);

        Vector2 noiseOffset = Vector2.Zero;
        float noiseRotation = 0f;

        if (_trauma > 0)
        {
            _trauma = Mathf.Max(_trauma - Decay * (float)delta, 0.0f);
            float amount = Mathf.Pow(_trauma, TraumaPower);
            _time += (float)delta * NoiseSpeed;

            noiseRotation = MaxRoll * amount * _noise.GetNoise2D(1, _time);
            noiseOffset = new Vector2(
                MaxOffset.X * amount * _noise.GetNoise2D(2, _time),
                MaxOffset.Y * amount * _noise.GetNoise2D(3, _time)
            );
        }

        // 2. Appliquer la combinaison finale a l Offset de la camera
        Offset = _baseOffset + noiseOffset + _impactOffset;

        // 3. Gerer la rotation separement avec un retour a la normale fluide
        if (_trauma > 0)
        {
            Rotation = _baseRotation + noiseRotation;
        }
        else
        {
            Rotation = Mathf.Lerp(Rotation, _baseRotation, (float)delta * 10f);
        }
    }

    public void AddTrauma(float amount)
    {
        _trauma = Mathf.Clamp(_trauma + amount, 0.0f, 1.0f);
    }

    public void AddDirectionalShake(Vector2 direction, float strength)
    {
        // On ajoute la force a l impact, sans toucher directement a l Offset de la camera
        _impactOffset += direction.Normalized() * strength;

        // Ajoute un tres leger trauma pour faire vibrer la camera apres le coup
        AddTrauma(0.3f);
    }
}