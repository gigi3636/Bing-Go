using Godot;
using System;
using System.Collections.Generic;

public partial class TicketFullScreen : Control
{
    [Export] private GridContainer gridContainerRef;
    [Export] private Sprite2D ticketFullVisual;
    private RandomNumberGenerator rand = new RandomNumberGenerator();
    private Ticket currentTicketToShowRef;

    public event Action<int,Ticket> OnTicketStamped;

    public event Action<Ticket> OnTicketUpdate;
    

    public override void _Ready()
    {
        TicketEventBus.OnTicketOpened += ShowTicket;
        TicketEventBus.OnTicketAutoStamped += UpdateVisual;
        Visible = false;
    }

    public void ShowTicket(Ticket pTicket)
    {
        Visible = true;
        currentTicketToShowRef = pTicket;
        UpdateVisual();

    }


    private void UpdateVisual()
    {
        if (currentTicketToShowRef is null) return;
        ticketFullVisual.Texture = currentTicketToShowRef.ticketFullVisual;
        UpdateGrid();
        OnTicketUpdate?.Invoke(currentTicketToShowRef);

    }

    private void UpdateGrid()
    {
        ClearGrid();

        var lTicketNumbers = currentTicketToShowRef.GetTicketNumbers();

        gridContainerRef.Columns = currentTicketToShowRef.Column;

        gridContainerRef.Position = currentTicketToShowRef.gridDisplayPosition;
        gridContainerRef.Size = currentTicketToShowRef.gridDisplaySize;

        for (int i = 0; i < lTicketNumbers.Count; i++)
        {
            int lCellNumber = lTicketNumbers[i];

            int lTicketPosX = i % currentTicketToShowRef.Column;
            int lTicketPosY = i / currentTicketToShowRef.Column;

            BingoCell lCell = (BingoCell)currentTicketToShowRef.CellScene.Instantiate();
            gridContainerRef.AddChild(lCell);

            lCell.Initialize(lCellNumber, new Vector2I(lTicketPosX, lTicketPosY), currentTicketToShowRef.IsNumberStamped(lCellNumber), currentTicketToShowRef.cellScale);

            lCell.OnCellStamped += NumbersStamped;
        }

    }

    private void _on_button_button_down()
    {
        CloseTicket();
    }

    // Emited when a ticket case is clicked
    private void NumbersStamped(int pCellNumbers)
    {

        OnTicketStamped?.Invoke(pCellNumbers, currentTicketToShowRef);
        UpdateVisual();

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
        TicketEventBus.OnTicketAutoStamped -= UpdateVisual;

    }
}
