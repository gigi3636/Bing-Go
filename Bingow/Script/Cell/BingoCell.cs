using Godot;
using System;

public partial class BingoCell : Control
{
    [Export] private Label cellAmountLabelRef;
    [Export] private CellStamped cellStampedRef;
    [Export] private TextureRect stampRef;

    private bool isStamped;
    private Vector2I cellId;
    private int cellNumbers;
    public int CellNumber => cellNumbers;

    public event Action<int> OnCellStamped;

    public void Initialize(int pCellAmount, Vector2I pCellId, bool pIsCellCheck, int pCellScale)
    {
        cellAmountLabelRef.Text = pCellAmount.ToString();
        cellNumbers = pCellAmount;
        cellId = pCellId;
        cellAmountLabelRef.LabelSettings.FontSize = pCellScale;
        cellStampedRef.Initialize(pCellScale);
        cellStampedRef.SetInitialState(pIsCellCheck);

        PivotOffset = Size / 2;
    }

    public void UpdateCellState(bool pIsCellCheck)
    {
        cellStampedRef.UpdateState(pIsCellCheck);

    }

    public void PlayScoringPop(bool isSpecialEffect, int comboStep)
    {
        PivotOffset = Size / 2f;
        ZIndex = 20;

        float scaleBoost = isSpecialEffect ? 1.55f : 1.3f + Mathf.Min(comboStep * 0.05f, 0.35f);
        float angleKick = (comboStep % 2 == 0 ? 1f : -1f) * (isSpecialEffect ? 14f : 8f);
        Color flashColor = isSpecialEffect ? new Color(2.5f, 1.2f, 2.8f, 1f) : new Color(2.2f, 2.0f, 0.6f, 1f);

        Scale = new Vector2(scaleBoost, scaleBoost);
        RotationDegrees = angleKick;
        Modulate = flashColor;

        Tween t = CreateTween();
        t.SetParallel(true);
        t.TweenProperty(this, "scale", Vector2.One, 0.38f)
         .SetTrans(Tween.TransitionType.Elastic)
         .SetEase(Tween.EaseType.Out);
        t.TweenProperty(this, "rotation_degrees", 0f, 0.38f)
         .SetTrans(Tween.TransitionType.Elastic)
         .SetEase(Tween.EaseType.Out);
        t.TweenProperty(this, "modulate", Colors.White, 0.3f)
         .SetTrans(Tween.TransitionType.Cubic)
         .SetEase(Tween.EaseType.Out);

        t.SetParallel(false);
        t.TweenCallback(Callable.From(() => ZIndex = 0));
    }

    private void _on_cell_button_pressed()
    {
        if (!stampRef.Visible) stampRef.RotationDegrees = GD.RandRange(-12, 12);
        
        OnCellStamped?.Invoke(cellNumbers);
    }

    private void _on_cell_button_mouse_entered()
    {
        PivotOffset = Size / 2;
        Tween t = CreateTween();
        t.TweenProperty(this, "modulate", new Color(1.15f, 1.15f, 1.15f), 0.08f);
    }

    private void _on_cell_button_mouse_exited()
    {
        Tween t = CreateTween();
        t.TweenProperty(this, "modulate", Colors.White, 0.08f);
    }
}