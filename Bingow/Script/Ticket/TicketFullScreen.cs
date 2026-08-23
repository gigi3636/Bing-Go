using Godot;
using System;
using System.Collections.Generic;

public partial class TicketFullScreen : Control
{
    [Export] private GridContainer gridContainerRef;
    private RandomNumberGenerator rand = new RandomNumberGenerator();
    private Ticket currentTicketToShowRef;

    public event Action<int,Ticket> OnTicketStamped;

    public event Action<Ticket> OnTicketUpdate;
    
    //public event Action<List<int>> 

    public override void _Ready()
    {
        TicketEventBus.OnTicketOpened += ShowTicket;
        Visible = false;
    }

    public void ShowTicket(Ticket pTicket)
    {
        Visible = true;
        currentTicketToShowRef = pTicket;
        ClearGrid();

        var lTicketNumbers = pTicket.GetTicketNumbers();
        GD.Print(lTicketNumbers.Count);

        for (int i = 0; i < lTicketNumbers.Count; i++)
        {
            int lCellNumber = lTicketNumbers[i];

            int lTicketPosX = i % pTicket.Column;
            int lTicketPosY = i / pTicket.Column;

            BingoCell lCell = (BingoCell)pTicket.CellScene.Instantiate();
            gridContainerRef.AddChild(lCell);

            lCell.Initialize(lCellNumber, new Vector2I(lTicketPosX, lTicketPosY), pTicket.IsNumberStamped(lCellNumber));

            lCell.OnCellStamped += NumbersStamped;
        }

        OnTicketUpdate?.Invoke(pTicket);

    }

    private void _on_button_button_down()
    {
        CloseTicket();
    }

    // Emited when a ticket case is clicked
    private void NumbersStamped(int pCellNumbers)
    {

        OnTicketStamped?.Invoke(pCellNumbers, currentTicketToShowRef);
        OnTicketUpdate?.Invoke(currentTicketToShowRef);

    }

    public void CloseTicket()
    {
        ClearGrid();

        Visible = false;
    }

    private void ClearGrid()
    {
        foreach (Node child in gridContainerRef.GetChildren())
        {
            if (child is BingoCell lCell)
            {
                lCell.OnCellStamped -= NumbersStamped;
            }

            child.QueueFree();
        }
    }

    public override void _ExitTree()
    {
        TicketEventBus.OnTicketOpened -= ShowTicket;
    }
}
