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

    protected TicketNumbersManager ticketNumbersManager;

    public event Action OnEndOfTicket;
    
    public virtual void Initialize(TicketData pData, PlayerStatus pPlayerStatusRef)
    {
        // Get all the ticket data from the resource
        column = pData.Column;
        row = pData.Row;
        value = pData.Value;
        // higherNumber = pPlayerStatusRef.CurrentBingoBallsAmount;   normalement scale avec le joueur A PA OUBLIER ########################################################################
        higherNumber = column * row;
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

    public void UpdateStampedNumber(int pCellNumbers, bool pIsNumbersAllowed, bool pIsNumberAutostamped)
    {
        ticketNumbersManager.UpdateStampedNumbersList(pCellNumbers, pIsNumbersAllowed, pIsNumberAutostamped);
        CheckForCompletedLines(IsNumberStamped);
    }

    // Check if a line is stamped , and check if the line stamped numbers is allowed or not ( func meth can do both )
    public bool CheckForCompletedLines(Func<int, bool> pValidationCondition)
    {
        // Horizontal Line Check
        for (int y = 0; y < row; y++)
        {
            bool isLineComplete = true;

            for (int x = 0; x < column; x++)
            {
                int index = (y * column) + x;
                int numberToCheck = ticketNumbers[index];

                
                if (!pValidationCondition(numberToCheck))
                {
                    isLineComplete = false;
                    break;
                }
            }
            if (isLineComplete)
            {
                if (!isCompleted)
                {
                    isCompleted = true;
                    OnTicketCompleted?.Invoke(this);
                }
                return true;
            }
        }

        // Vertical Line Check
        for (int x = 0; x < column; x++)
        {
            bool isColumnComplete = true;

            for (int y = 0; y < row; y++)
            {
                int index = (y * column) + x;
                int numberToCheck = ticketNumbers[index];

                if (!pValidationCondition(numberToCheck))
                {
                    isColumnComplete = false;
                    break;
                }
            }
            if (isColumnComplete)
            {
                if (!isCompleted)
                {
                    isCompleted = true;
                    OnTicketCompleted?.Invoke(this);
                }
                return true;
            }
        }

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


    public void DeleteTicket()
    {
        OnEndOfTicket?.Invoke();
    }

    protected abstract void SetupTicket();

}
