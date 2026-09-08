using Godot;
using System;
using System.Reflection.Emit;

public partial class AutoBingo : SpotUpgrade
{
    public void HandleTicketCompleted(Ticket pTicket)
    {
        if (!isActive) return;

        TicketEventBus.PublishBingoCalled(pTicket);
    }
}
