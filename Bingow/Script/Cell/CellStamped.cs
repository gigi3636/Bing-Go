using Godot;
using System;

public partial class CellStamped : Node2D
{
    [Export] private TextureRect stampRef;
    [Export] private GpuParticles2D inkParticlesRef;

    public override void _Ready()
    {
        stampRef.Visible = false;

        if (GetParent() is Control parentControl)
        {
            parentControl.Resized += CenterStamp;
        }
    }

    public void Initialize(int pCellScale)
    {
        Vector2 newSize = new Vector2(pCellScale * 2 * 1.1f, pCellScale * 2 * 1.1f);
        stampRef.Size = newSize;
        stampRef.PivotOffset = stampRef.Size / 2;

        CenterStamp();
    }

    private void CenterStamp()
    {
        if (GetParent() is Control parentControl)
        {
            stampRef.Position = (parentControl.Size - stampRef.Size) / 2;
            stampRef.PivotOffset = stampRef.Size / 2;

            if (inkParticlesRef != null)
            {
                inkParticlesRef.Position = parentControl.Size / 2;
            }
        }
    }

    public void SetInitialState(bool pIsStamped)
    {
        stampRef.Visible = pIsStamped;
        stampRef.Scale = Vector2.One;
    }

    public void UpdateState(bool pIsStamped)
    {
        if (pIsStamped && !stampRef.Visible)
        {
            stampRef.Visible = true;
            stampRef.Scale = new Vector2(2.4f, 2.4f);
            stampRef.Modulate = new Color(2.5f, 2.5f, 2.5f, 0.4f);

            ShakeCamera2D.Instance?.AddDirectionalShake(Vector2.Down, 1.5f);

            if (inkParticlesRef != null)
            {
                inkParticlesRef.Restart();
                inkParticlesRef.Emitting = true;
            }

            Tween tween = GetTree().CreateTween();
            tween.SetParallel(true);

            tween.TweenProperty(stampRef, "scale", Vector2.One, 0.14f)
                 .SetTrans(Tween.TransitionType.Back)
                 .SetEase(Tween.EaseType.In);

            tween.TweenProperty(stampRef, "modulate", Colors.White, 0.14f)
                 .SetTrans(Tween.TransitionType.Cubic)
                 .SetEase(Tween.EaseType.Out);

            if (GetParent() is Control parentCell)
            {
                parentCell.PivotOffset = parentCell.Size / 2;
                tween.SetParallel(false);
                tween.TweenProperty(parentCell, "scale", new Vector2(1.15f, 0.85f), 0.05f);
                tween.TweenProperty(parentCell, "scale", Vector2.One, 0.2f)
                     .SetTrans(Tween.TransitionType.Elastic)
                     .SetEase(Tween.EaseType.Out);
            }
        }
        else if (!pIsStamped)
        {
            stampRef.Visible = false;
            stampRef.Scale = Vector2.One;
        }
    }

    private void _on_cell_button_pressed()
    {
    }
}