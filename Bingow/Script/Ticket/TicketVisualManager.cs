using Godot;
using System;

public partial class TicketVisualManager : Node
{
    [Export] Ticket ticketRef;

    public void SetNormalSize()
    {
        ticketRef.Scale = new Vector2(Ticket.TICKET_BASIC_SCALE, Ticket.TICKET_BASIC_SCALE);
    }

    public void SetFullScreenSize()
    {
        ticketRef.Scale = Vector2.One;

    }
}
