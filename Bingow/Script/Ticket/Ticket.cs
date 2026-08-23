using Godot;
using System;
using System.Collections.Generic;

// Live data of each ticket
public abstract partial class Ticket : Node2D
{
    public const float TICKET_BASIC_SCALE = 0.2f;

    protected int column ;
    protected int row ;

    protected int value;
    protected int cost;

    public PackedScene cellScene { get; private set; }

    public int Column => column; // Geteur public exterieur
    public PackedScene CellScene => cellScene; 


    protected int higherNumber;

    protected List<int> ticketNumbers;

    protected TicketNumbersManager ticketNumbersManager;

    public virtual void Initialize(TicketData data)
    {
        // Get all the ticket data from the resource
        column = data.Column;
        row = data.Row;
        higherNumber = data.HigherNumber;
        cellScene = data.CellScene;

        // Create a new empty List of number for the ticket
        ticketNumbers = new List<int>();
        ticketNumbersManager = new TicketNumbersManager();


        Scale = new Vector2(TICKET_BASIC_SCALE, TICKET_BASIC_SCALE);
        SetupTicket();
        
    }

    public List<int> GetTicketNumbers()
    {
        return ticketNumbers;
    }

    public bool IsNumberStamped(int pCellNumbers)
    {
        return (ticketNumbersManager.IsNumbersStamped(pCellNumbers));
    }

    public Vector2I GetCoordinatesOfNumber(int targetNumber)
    {
        int index = ticketNumbers.IndexOf(targetNumber);

        if (index == -1)
        {
            return new Vector2I(-1, -1);
        }

        int x = index % column;
        int y = index / column;

        return new Vector2I(x, y);
    }

    public void UpdateStampedNumber(int pCellNumbers, bool pIsNumbersAllowed)
    {
        ticketNumbersManager.UpdateStampedNumbersList(pCellNumbers, pIsNumbersAllowed);
    }

    public bool CheckForCompletedLines()
    {
        // Horizontal Line Check
        for (int y = 0; y < row; y++)
        {
            bool isLineComplete = true;

            for (int x = 0; x < column; x++)
            {
                int index = (y * column) + x;
                int numberToCheck = ticketNumbers[index];

                // If one number isnt stamped the line is not completed
                if (!IsNumberStamped(numberToCheck))
                {
                    isLineComplete = false;
                    break; // The line is not completed pass to the next one
                }
            }

            // One Line is completed
            if (isLineComplete) return true;
        }

        // Vertical Line Check
        for (int x = 0; x < column; x++)
        {
            bool isColumnComplete = true;

            for (int y = 0; y < row; y++)
            {
                int index = (y * column) + x;
                int numberToCheck = ticketNumbers[index];

                if (!IsNumberStamped(numberToCheck))
                {
                    isColumnComplete = false;
                    break;
                }
            }

            if (isColumnComplete) return true;
        }

        // No line or column are completed yet 
        return false;
    }

    protected abstract void SetupTicket();

}
