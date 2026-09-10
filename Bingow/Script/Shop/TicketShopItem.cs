using Godot;
using System;

public partial class TicketShopItem : Node
{
    [Export] private TicketData ticketRef;
        
    [Export] private TextureRect ticketVisualRef;
    [Export] private Label ticketNameRef;
    [Export] private Label tikcetPriceRef;
    [Export] private TicketShoptButton buyButtonRef;

    public override void _Ready()
    {
        ticketVisualRef.Texture = ticketRef.TicketTableVisual;
        ticketNameRef.Text = ticketRef.TicketName;
        tikcetPriceRef.Text =  $"{ ticketRef.Cost.ToString() + " $" }";

        buyButtonRef.Initialize(ticketRef);
    }
}
