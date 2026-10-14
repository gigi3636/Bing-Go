using Godot;
using System;

public partial class BallUpgrader : Node
{
    [Export] private RichTextLabel textLabelRef;

    public Action<int> OnUpgradeBought;

    public void UpdateVisuel(int pBallLevel)
    {
        textLabelRef.Text = $"Niveau {pBallLevel} => [color=green]Niveau {pBallLevel + 1}[/color]";
    }

    private void _on_buy_button_pressed()
    {

    }
}
