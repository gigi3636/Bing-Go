using Godot;
using System;
using System.Collections.Generic;

public partial class AutoFiller : Upgrade
{
    private TicketQueu ticketQueuRef;


    public event Action<Ticket, float> OnTicketDisponible;

    public void Initialize(TicketQueu pTicketQueuRef)
    {
        upgradesLevelsAmount = new List<float> { 1f, 0.8f, 0.5f, 0.3f, 0.1f };

        ticketQueuRef = pTicketQueuRef;
    }

    public void CheckDisponibility()
    {
        if (!isActive) return;
        Ticket lNexTicket = ticketQueuRef.GetTicketFromQueu();


        if (lNexTicket != null)
        {
            OnTicketDisponible?.Invoke(lNexTicket, currentReactionTime);
            ticketQueuRef.RemoveTicketFromQueu();
        }

    }
}
