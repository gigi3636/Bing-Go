using Godot;
using System;

public partial class TicketMoover : Node2D
{
    [Export] private TicketQueu ticketQueuRef;

    private bool isTicketMooving;
    private Ticket ticketToMooveRef;
    private float ticketInitialRotation;

    private TicketSpot ticketSpotAvailableRef;

    public override void _Ready()
    {

        TicketEventBus.OnFreeTicketSpotEnter += SetAvailableTicketSpot;
        TicketEventBus.OnFreeTicketSpotExit += ExitTicketSpot;
    }

    private void _on_texture_button_button_down()
    {
        isTicketMooving = true;
        ticketToMooveRef = ticketQueuRef.GetTicketFromQueu();
        ticketInitialRotation = ticketToMooveRef.GlobalRotation;

        Tween lTween = CreateTween();

        lTween.SetParallel(true);
        lTween.TweenProperty(ticketToMooveRef, "global_rotation", 0, 0.3);
    }

    private void _on_texture_button_button_up()
    {
        // CHECK IF THE TICKET IS IN A TICKET SPOT FREE

        if (ticketSpotAvailableRef != null && ticketSpotAvailableRef.isTicketSizeAllowed(ticketToMooveRef.sizeLevel))
        {

            ticketSpotAvailableRef.SetNewTicket(ticketToMooveRef);
            ticketToMooveRef.GlobalPosition = ticketSpotAvailableRef.GlobalPosition;
            //ticketToMooveRef.Scale = new Vector2(0.10f, 0.10f);
            ticketQueuRef.RemoveTicketFromQueu();
        }
        else
        {
            ResteTicketPosition();
        }


        isTicketMooving = false;
        ticketSpotAvailableRef = null;
        
    }

    private void ResteTicketPosition()
    {
        Tween lTween = CreateTween();

        lTween.SetParallel(true);
        lTween.TweenProperty(ticketToMooveRef, "global_position", GlobalPosition, 0.3);
        lTween.TweenProperty(ticketToMooveRef, "global_rotation", ticketInitialRotation, 0.3);


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
