using Godot;
using System;

public partial class TicketSpot : Area2D
{
    private bool isUsed = false;
    private Ticket ticketContainer;

    public override void _Ready()
    {
        

    }

    private void _on_area_entered(Area2D pIntruder)
    {
        GD.Print("detecter");
        if(pIntruder.GetParent() is Ticket lTicket && !isUsed)
        {
            TicketEventBus.EnterFreeTikcetSpot(this);
        }

    }

    private void _on_area_exited(Area2D pIntruder)
    {
        if (pIntruder.GetParent() is Ticket lTicket)
        {
            TicketEventBus.ExitTikcetSpot();
        }

    }

    public void SetNewTicket(Ticket pTicket)
    {

        isUsed = true;
        ticketContainer = pTicket;


    }

    private void _on_button_pressed()
    {
        if (isUsed)
        {
            TicketEventBus.OpenTicket(ticketContainer);
        }
    }

}
