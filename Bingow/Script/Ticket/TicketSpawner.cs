using Godot;
using System;

public partial class TicketSpawner : Node
{
    [Export] private Node2D ticketContainerRef;

    public event Action<Ticket> OnTicketAdded;

    public override void _Ready()
    {
        TicketEventBus.OnTicketRequested += SpawnTicket;
    }


    // Spawn the ticket and initialize it with the good ticket Data
    public void SpawnTicket(TicketData pTicketDataToSpawn)
    {
        GD.Print(pTicketDataToSpawn.ToString());

        Ticket lNewTicket = (Ticket)pTicketDataToSpawn.TicketScene.Instantiate();
        ticketContainerRef.AddChild(lNewTicket);

        lNewTicket.Initialize(pTicketDataToSpawn);
        lNewTicket.GlobalPosition = ticketContainerRef.GlobalPosition;

        OnTicketAdded?.Invoke(lNewTicket);
    }


    public override void _ExitTree()
    {
        TicketEventBus.OnTicketRequested -= SpawnTicket;
    }
}
