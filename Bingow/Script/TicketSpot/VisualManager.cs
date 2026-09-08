using Godot;
using System;

public partial class VisualManager : Node
{
    [Export] private Button spotBuyButtonRef;
    [Export] private TicketSpot ticketSpotRef;

    public override void _Ready()
    {
        base._Ready();
        ticketSpotRef.OnVisualUpdate += UpdateVisual;

        spotBuyButtonRef.Text = $"{ticketSpotRef.unlockCost} $";
    }

    private void UpdateVisual(bool pIsSpotUnlocked)
    {
        spotBuyButtonRef.Visible = !pIsSpotUnlocked;
       
    }
}
