using Godot;
using System;

public partial class PrinterManager : Node
{
    [Export] private TicketData[] ticketDatas;
    [Export] private HBoxContainer ticketContainer;
    [Export] private PackedScene ticketVisualScene;


    public override void _Ready()
    {
        base._Ready();

        foreach (TicketData lTicket in ticketDatas)
        {
            PrinterTicketVisual lTicketVisual = (PrinterTicketVisual)ticketVisualScene.Instantiate();

            lTicketVisual.Initialize(lTicket);

            ticketContainer.AddChild(lTicketVisual);

        }
    }
}
