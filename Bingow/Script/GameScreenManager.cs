using Godot;
using System;

public partial class GameScreenManager : Node
{
    [Export] private BingoCage bingoCageRef;
    [Export] private TicketFullScreen ticketFullScreenRef;

    public override void _Ready()
    {

        ticketFullScreenRef.OnTicketStamped += HandleStampedNumbers;
    }

    private void HandleStampedNumbers(int pCellNumbers, Ticket pTicketStamped)
    {
        bool lIsNumbersAllowed = bingoCageRef.IsNumbersAllowed(pCellNumbers);

        pTicketStamped.UpdateStampedNumber(pCellNumbers, lIsNumbersAllowed);


    }

    public override void _ExitTree()
    {
        ticketFullScreenRef.OnTicketStamped -= HandleStampedNumbers;
    }
}
