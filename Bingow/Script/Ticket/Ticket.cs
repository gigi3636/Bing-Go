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

    public bool isCompleted { get; private set; }

    [Export] private Sprite2D ticketTableVisual;
    public Texture2D ticketFullVisual { get; private set; }

    public PackedScene cellScene { get; private set; }
    public Vector2 gridDisplayPosition { get; private set; }
    public Vector2 gridDisplaySize { get; private set; }
    public int sizeLevel { get; private set; }

    public int cellScale { get; private set; }

    public int Column => column; // Geteur public exterieur
    public int Value => value;
    public PackedScene CellScene => cellScene;

    public event Action<Ticket> OnTicketCompleted;

    protected int higherNumber;

    protected List<int> ticketNumbers;
    public List<int> winningLineNumbers { get; private set; } = new List<int>();

    protected TicketNumbersManager ticketNumbersManager;

    public event Action OnEndOfTicket;
    
    public virtual void Initialize(TicketData pData, PlayerStatus pPlayerStatusRef)
    {
        // Get all the ticket data from the resource
        column = pData.Column;
        row = pData.Row;
        value = pData.Value;
        higherNumber = pPlayerStatusRef.CurrentBingoBallsAmount - 1;   //normalement scale avec le joueur A PA OUBLIER ########################################################################
        //higherNumber = column * row;
        cellScene = pData.CellScene;
        sizeLevel = pData.SizeLevel;
        ticketTableVisual.Texture = pData.TicketTableVisual;
        ticketFullVisual = pData.TicketFullVisual;

        gridDisplayPosition = pData.GridDisplayPosition;
        gridDisplaySize = pData.GridDisplaySize;
        cellScale = pData.CellScale;

        // Create a new empty List of number for the ticket
        ticketNumbers = new List<int>();
        ticketNumbersManager = new TicketNumbersManager();


        //Scale = new Vector2(TICKET_BASIC_SCALE, TICKET_BASIC_SCALE);
        SetupTicket();
        
    }

    public List<int> GetTicketNumbers()
    {
        return ticketNumbers;
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

    // Update the ticket by the GameScreenManager
    public void UpdateStampedNumber(int pCellNumbers, bool pIsNumbersAllowed, IBallEffect pBallEffect, bool pIsNumberAutostamped)
    {
        // TicketNumberManager save the changes of this ticket
        ticketNumbersManager.UpdateStampedNumbersList(pCellNumbers, pIsNumbersAllowed, pBallEffect, pIsNumberAutostamped, this);

        //Look if there is a bingo now
        CheckForCompletedLines(IsNumberStamped);
    }

    // Check if a line is stamped , and check if the line stamped numbers is allowed or not ( func meth can do both )
    public bool CheckForCompletedLines(Func<int, bool> pValidationCondition)
    {
        // Check Horizontal 
        for (int y = 0; y < row; y++)
        {
            bool isLineComplete = true;
            List<int> currentLine = new List<int>(); // save the line analyzed

            for (int x = 0; x < column; x++)
            {
                int index = (y * column) + x;
                int numberToCheck = ticketNumbers[index];
                currentLine.Add(numberToCheck); // Add this number in order to the line 

                if (!pValidationCondition(numberToCheck))
                {
                    isLineComplete = false;
                    break;
                }
            }
            if (isLineComplete)
            {
                winningLineNumbers = currentLine; // Save the bingo line
                if (!isCompleted)
                {
                    isCompleted = true;
                    OnTicketCompleted?.Invoke(this);
                }
                return true;
            }
        }

        // Check Vertical 
        for (int x = 0; x < column; x++)
        {
            bool isColumnComplete = true;
            List<int> currentLine = new List<int>(); // Save the column analyzed

            for (int y = 0; y < row; y++)
            {
                int index = (y * column) + x;
                int numberToCheck = ticketNumbers[index];
                currentLine.Add(numberToCheck); // Add this number in order to the column

                if (!pValidationCondition(numberToCheck))
                {
                    isColumnComplete = false;
                    break;
                }
            }
            if (isColumnComplete)
            {
                winningLineNumbers = currentLine; // Save the bingo column
                if (!isCompleted)
                {
                    isCompleted = true;
                    OnTicketCompleted?.Invoke(this);
                }
                return true;
            }
        }

        isCompleted = false;
        return false;
    }

    // Verify if a number is alredy stamped
    public bool IsNumberStamped(int pCellNumbers)
    {
        return ticketNumbersManager.IsNumbersStamped(pCellNumbers);
    }

    // Verifiy if a number is alredy stamped and if its an allowed number
    public bool IsNumberValidAndStamped(int pCellNumbers)
    {
        return ticketNumbersManager.IsNumberValidAndStamped(pCellNumbers);
    }

    // Return all the valid effect
    public List<IBallEffect> GetTicketValidEffects()
    {
        return ticketNumbersManager.GetAllValidEffects();
    }

    // return the ball effect of this number
    public IBallEffect GetEffectOfNumber(int number)
    {
        return ticketNumbersManager.GetNumberEffect(number);
    }

    public void DeleteTicket()
    {
        OnEndOfTicket?.Invoke();
    }

    protected abstract void SetupTicket();

}
