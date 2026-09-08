using Godot;
using System;

public partial class SpotInteractionManager : Node
{

    [Export] private TicketSpot ticketSpotRef;


    private void _on_spot_area_area_entered(Area2D pIntruder)
    {
        if (!ticketSpotRef.isUnlocked) return;


        if (pIntruder.GetParent() is Ticket lTicket && !ticketSpotRef.isUsed)
        {
            TicketEventBus.EnterFreeTikcetSpot(ticketSpotRef);
        }

    }

    private void _on_spot_area_area_exited(Area2D pIntruder)
    {
        if (!ticketSpotRef.isUnlocked) return;

        if (pIntruder.GetParent() is Ticket lTicket)
        {
            TicketEventBus.ExitTikcetSpot();

        }

    }

    private void _on_spot_ticket_button_pressed()
    {
        if (ticketSpotRef.isUsed)
        {
            TicketEventBus.OpenTicket(ticketSpotRef.ticketContainer);
        }
    }

    private void _on_spot_buy_button_pressed()
    {
        ticketSpotRef.OnPurchaseRequested?.Invoke(ticketSpotRef.unlockCost, ticketSpotRef.UnlockTicket);
    }


}
