using Godot;
using System;

public partial class PrinterTicketVisual : TextureRect
{
    public TicketData ticketRef {  get; private set; }

    public void Initialize(TicketData pTicketRef)
    {
        ticketRef = pTicketRef;
        Texture = pTicketRef.TicketTableVisual;
    }
}
