using Godot;
using System;
using System.Collections;

// This script link the bingo cage with the numbers stamped by the player  , the ticket spot with the money controller

public partial class GameScreenManager : Node
{
    [Export] private BingoCage bingoCageRef;
    [Export] private TicketFullScreen ticketFullScreenRef;
    [Export] private TicketSpot[] ticketSpotArray;
    [Export] private MoneyController moneyControllerRef;
    [Export] private TicketQueu ticketQueuRef;
    [Export] private ShopTicket shopRef;

    public override void _Ready()
    {

        ticketFullScreenRef.OnTicketStamped += HandleStampedNumbers;

        foreach (TicketSpot lTicketSpot in ticketSpotArray)
        {
            lTicketSpot.OnPurchaseRequested += moneyControllerRef.TicketSpotPurchase;
            bingoCageRef.OnNewBall += lTicketSpot.AutoStamperRef.VerifyGrid;
            lTicketSpot.AutoStamperRef.Initialize(bingoCageRef.IsNumbersAllowed, bingoCageRef.GetCurrentBalls);
            lTicketSpot.AutoFillerRef.Initialize(ticketQueuRef);
            shopRef.Initialize(lTicketSpot);


        }
        shopRef.OnAnyItemUpgradeRequested += moneyControllerRef.UpgradeAugment;

    }

    private void HandleStampedNumbers(int pCellNumbers, Ticket pTicketStamped)
    {
        bool lIsNumbersAllowed = bingoCageRef.IsNumbersAllowed(pCellNumbers);

        pTicketStamped.UpdateStampedNumber(pCellNumbers, lIsNumbersAllowed, false);


    }

    public override void _ExitTree()
    {
        ticketFullScreenRef.OnTicketStamped -= HandleStampedNumbers;

        foreach (TicketSpot lTicketSpot in ticketSpotArray)
        {
            lTicketSpot.OnPurchaseRequested -= moneyControllerRef.TicketSpotPurchase;
            shopRef.OnAnyItemUpgradeRequested -= moneyControllerRef.UpgradeAugment;

            bingoCageRef.OnNewBall -= lTicketSpot.AutoStamperRef.VerifyGrid;
        }
    }
}
