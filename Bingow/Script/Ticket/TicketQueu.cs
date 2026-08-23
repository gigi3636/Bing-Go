using Godot;
using System;
using System.Collections.Generic;

public partial class TicketQueu : Node
{
    [Export] private TicketSpawner ticketSpawnnerRef;

    private Queue<Ticket> ticketsInQueu = new Queue<Ticket>();
    public override void _Ready()
    {
        ticketSpawnnerRef.OnTicketAdded += AddTicketInQueu;
    }

    public void ResetQueu()
    {
        ticketsInQueu = new Queue<Ticket>();
    }

    public void AddTicketInQueu(Ticket pTicket)
    {
        ticketsInQueu.Enqueue(pTicket);
    }

    // Take off the queu the latest ticket
    public Ticket GetTicketFromQueu()
    {
        if (ticketsInQueu.Count == 0) return null;

        return ticketsInQueu.Peek();
    }

    public void RemoveTicketFromQueu()
    {
        ticketsInQueu.Dequeue();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        ticketSpawnnerRef.OnTicketAdded -= AddTicketInQueu;

    }
}
