using Godot;
using System;

public partial class CellStamped : Node
{
    [Export] private TextureRect stampRef;

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
        Vector2 newSize = new Vector2(pCellScale * 2, pCellScale * 2);
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
            stampRef.Scale = new Vector2(2.5f, 2.5f);

            // Simule le poids du coup de tampon vers le bas
            //ShakeCamera2D.Instance?.AddDirectionalShake(Vector2.Down, 0.1f);

            Tween tween = GetTree().CreateTween();

            tween.TweenProperty(stampRef, "scale", Vector2.One, 0.12f)
                 .SetTrans(Tween.TransitionType.Cubic) 
                 .SetEase(Tween.EaseType.In);          
        }
        else if (!pIsStamped)
        {
            stampRef.Visible = false;
            stampRef.Scale = Vector2.One;
        }
    }
}