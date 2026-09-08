using Godot;
using System;

public partial class TicketSpawner : Node
{
    [Export] private Node2D ticketContainerRef;
    [Export] private PlayerStatus playerStatusRef;
    public event Action<Ticket> OnTicketAdded;

    public override void _Ready()
    {
        TicketEventBus.OnTicketRequested += SpawnTicket;
    }


    // Spawn the ticket and initialize it with the good ticket Data
    public void SpawnTicket(TicketData pTicketDataToSpawn)
    {
        GD.Print("ici");

        GD.Print(pTicketDataToSpawn.ToString());

        Ticket lNewTicket = (Ticket)pTicketDataToSpawn.TicketScene.Instantiate();
        ticketContainerRef.AddChild(lNewTicket);

        lNewTicket.Initialize(pTicketDataToSpawn, playerStatusRef);
        AnimNewTicket(lNewTicket);

        OnTicketAdded?.Invoke(lNewTicket);
    }

    private void AnimNewTicket(Ticket pNewTicket)
    {
        // Tween data 
        Vector2 baseScale = pNewTicket.Scale;

        Vector2 finalPosition = ticketContainerRef.GlobalPosition;
        Vector2 startPosition = new Vector2(-200, finalPosition.Y);

        pNewTicket.GlobalPosition = startPosition;

        float startRotation = (float)GD.RandRange(-Mathf.Pi, Mathf.Pi);
        float finalRotation = (float)GD.RandRange(-0.30f, 0.30f);

        pNewTicket.Rotation = startRotation;
        
        pNewTicket.Scale = baseScale * 1.2f;

        float animDuration = 0.35f;

        // Tween
        Tween tween = GetTree().CreateTween();
        tween.SetParallel(true);

        // Move the ticket
        tween.TweenProperty(pNewTicket, "global_position", finalPosition, animDuration)
             .SetTrans(Tween.TransitionType.Cubic)
             .SetEase(Tween.EaseType.Out);

        // Rotate the ticket
        tween.TweenProperty(pNewTicket, "rotation", finalRotation, animDuration)
             .SetTrans(Tween.TransitionType.Cubic)
             .SetEase(Tween.EaseType.Out);

        // Set the ticket to its original scale
        tween.TweenProperty(pNewTicket, "scale", baseScale, animDuration)
             .SetTrans(Tween.TransitionType.Cubic)
             .SetEase(Tween.EaseType.Out);
    }


    public override void _ExitTree()
    {
        TicketEventBus.OnTicketRequested -= SpawnTicket;
    }
}
