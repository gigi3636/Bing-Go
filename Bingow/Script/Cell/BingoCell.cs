using Godot;
using System;

public partial class BingoCell : Control
{
    [Export] private Label cellAmountLabelRef;
    [Export] private CellStamped cellStampedRef;

    private Vector2I cellId;
    private int cellNumbers;
    public event Action<int> OnCellStamped;

    public void Initialize(int pCellAmount, Vector2I pCellId, bool pIsCellCheck, int pCellScale)
    {
        cellAmountLabelRef.Text = pCellAmount.ToString();
        cellNumbers = pCellAmount;
        cellId = pCellId;
        cellAmountLabelRef.LabelSettings.FontSize = pCellScale;
        cellStampedRef.Initialize(pCellScale);

        cellStampedRef.SetInitialState(pIsCellCheck);
    }

    public void UpdateCellState(bool pIsCellCheck)
    {
        cellStampedRef.UpdateState(pIsCellCheck);
    }

    private void _on_cell_button_pressed()
    {
        OnCellStamped?.Invoke(cellNumbers);
    }
}