using Godot;
using System;

public partial class BingoBall : Control
{
    [Export] private Label ballNumbersLabel;
    [Export] private TextureRect ballTextureRef;

    private int ballNumber;

    private RandomNumberGenerator rand = new RandomNumberGenerator();

    public int BallNumber => ballNumber;

    public void Initialize(int pBallNumber, BallData pBallData)
    {
        ballNumber = pBallNumber;
        ballNumbersLabel.Text = pBallNumber.ToString();

        ballTextureRef.Texture = pBallData.texture;
    }

    public override void _Ready()
    {
        float targetAngle = rand.RandfRange(-20, 20);
        ballTextureRef.PivotOffset = ballTextureRef.Size / 2f;
        ballTextureRef.Scale = Vector2.Zero;
        ballTextureRef.RotationDegrees = 180f;

        Tween t = CreateTween();
        t.SetParallel(true);
        t.TweenProperty(ballTextureRef, "scale", Vector2.One, 0.32f)
         .SetTrans(Tween.TransitionType.Bounce)
         .SetEase(Tween.EaseType.Out);
        t.TweenProperty(ballTextureRef, "rotation_degrees", targetAngle, 0.32f)
         .SetTrans(Tween.TransitionType.Back)
         .SetEase(Tween.EaseType.Out);
    }

    public void PlayRollShiftAnimation()
    {
        if (ballTextureRef == null) return;
        ballTextureRef.PivotOffset = ballTextureRef.Size / 2f;
        Tween t = CreateTween();
        t.SetParallel(true);
        t.TweenProperty(ballTextureRef, "rotation_degrees", ballTextureRef.RotationDegrees - 35f, 0.18f)
         .SetTrans(Tween.TransitionType.Back)
         .SetEase(Tween.EaseType.Out);
        t.TweenProperty(ballTextureRef, "scale", new Vector2(0.9f, 1.1f), 0.06f);
        t.SetParallel(false);
        t.TweenProperty(ballTextureRef, "scale", Vector2.One, 0.14f)
         .SetTrans(Tween.TransitionType.Bounce)
         .SetEase(Tween.EaseType.Out);
    }

}
