using Godot;
using System;

public partial class TicketMoover : Node2D
{
    [Export] private TicketQueu ticketQueuRef;

    private bool isTicketMooving;
    private Ticket ticketToMooveRef;

    private TicketSpot ticketSpotAvailableRef;

    public override void _Ready()
    {

        TicketEventBus.OnFreeTicketSpotEnter += SetAvailableTicketSpot;
        TicketEventBus.OnFreeTicketSpotExit += ExitTicketSpot;
    }

    private void _on_texture_button_button_down()
    {
        GD.Print("down");

        isTicketMooving = true;
        ticketToMooveRef = ticketQueuRef.GetTicketFromQueu();

    }

    private void _on_texture_button_button_up()
    {
        GD.Print("up");

        // CHECK IF THE TICKET IS IN A TICKET SPOT FREE

        if (ticketSpotAvailableRef != null)
        {

            ticketSpotAvailableRef.SetNewTicket(ticketToMooveRef);
            ticketToMooveRef.GlobalPosition = ticketSpotAvailableRef.GlobalPosition;
            ticketToMooveRef.Scale = new Vector2(0.10f, 0.10f);
            ticketQueuRef.RemoveTicketFromQueu();
        }
        else
        {
            ticketToMooveRef.GlobalPosition = GlobalPosition;
        }


        isTicketMooving = false;
        ticketSpotAvailableRef = null;
        
    }

    public void SetAvailableTicketSpot(TicketSpot pTicketSpot)
    {
        ticketSpotAvailableRef = pTicketSpot;
    }

    public void ExitTicketSpot()
    {
        ticketSpotAvailableRef = null;
    }

    public override void _Process(double delta)
    {
        if (isTicketMooving)
        {
            ticketToMooveRef.GlobalPosition = GetGlobalMousePosition();
        }
    }
}
