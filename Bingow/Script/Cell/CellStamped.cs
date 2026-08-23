using Godot;
using System;

public partial class CellStamped : Node
{
    [Export] private Sprite2D stampRef;

    public override void _Ready()
    {
        stampRef.Visible = false;
    }

    private void _on_cell_button_pressed()
    {
        ShowStamp();
    }

    public void ShowStamp()
    {
        stampRef.Visible = !stampRef.Visible;
    }
}
