using Godot;
using System;
using System.Collections.Generic;

public partial class TicketQueu : Node
{
    [Export] private TicketSpawner ticketSpawnnerRef;

    private Stack<Ticket> ticketStack = new Stack<Ticket>();

    public override void _Ready()
    {
        ticketSpawnnerRef.OnTicketAdded += AddTicketInQueu;
    }

    public void ResetQueu()
    {
        ticketStack.Clear();
    }

    public void AddTicketInQueu(Ticket pTicket)
    {
        ticketStack.Push(pTicket);
    }

    // Show the last ticket
    public Ticket GetTicketFromQueu()
    {
        if (ticketStack.Count == 0) return null;

        return ticketStack.Peek();
    }

    // Remove the last ticket
    public void RemoveTicketFromQueu()
    {
        if (ticketStack.Count > 0)
        {
            ticketStack.Pop();
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        ticketSpawnnerRef.OnTicketAdded -= AddTicketInQueu;
    }
}