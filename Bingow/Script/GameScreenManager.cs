using Godot;
using System;
using System.Collections;
using static TicketSpotUpgradeButton;

// This script link the bingo cage with the numbers stamped by the player  , the ticket spot with the money controller

public partial class GameScreenManager : Node
{
    [Export] private BingoCage bingoCageRef;
    [Export] private TicketFullScreen ticketFullScreenRef;
    [Export] private TicketSpot[] ticketSpotArray;
    [Export] private MoneyController moneyControllerRef;
    [Export] private TicketQueu ticketQueuRef;
    [Export] private Shop shopRef;
    [Export] private TicketPrinter ticketPrinterRef;

    public override void _Ready()
    {

        ticketFullScreenRef.OnTicketStamped += HandleStampedNumbers;

        foreach (TicketSpot lTicketSpot in ticketSpotArray)
        {
            lTicketSpot.OnPurchaseRequested += moneyControllerRef.TicketSpotPurchase;
            bingoCageRef.OnNewBall += lTicketSpot.AutoStamperRef.VerifyGrid;
            lTicketSpot.AutoStamperRef.Initialize(bingoCageRef.IsNumbersAllowed, bingoCageRef.GetCurrentBalls, bingoCageRef.GetNumberEffect);
            lTicketSpot.AutoFillerRef.Initialize(ticketQueuRef);

            shopRef.AddSpotItem(lTicketSpot,
                ShopUpgrades.AutoClicker,
                ShopUpgrades.AutoFiller,
                ShopUpgrades.AutoBingo,
                ShopUpgrades.SizeUpgrade);

            lTicketSpot.OnUpgradeShopClicked += shopRef.OpenSpecificWindow;

        }
        shopRef.OnAnyItemUpgradeRequested += moneyControllerRef.UpgradeAugment;

        shopRef.AddUniqueItem("Ticket printer",
            ticketPrinterRef.globalUpgrade,
            ShopUpgrades.UnlockPrinter,
            ShopUpgrades.PintingSpeed,
            ShopUpgrades.PrinterSize);

        shopRef.AddUniqueItem("Bingo cage",
            bingoCageRef.bingoCageUpgrade,
            ShopUpgrades.ManualCd,
            ShopUpgrades.BallPerS,
            ShopUpgrades.CageCapacity);

    }

    // When a number is stamped by the player
    private void HandleStampedNumbers(int pCellNumbers, Ticket pTicketStamped)
    {
        // Check if this number is currently displayed in the bingoCage
        bool lIsNumbersAllowed = bingoCageRef.IsNumbersAllowed(pCellNumbers);
        IBallEffect lBallEffect = bingoCageRef.GetNumberEffect(pCellNumbers);

        // Update the ticket information about this stamped number
        pTicketStamped.UpdateStampedNumber(pCellNumbers, lIsNumbersAllowed, lBallEffect, false);


    }

    public override void _ExitTree()
    {
        ticketFullScreenRef.OnTicketStamped -= HandleStampedNumbers;

        foreach (TicketSpot lTicketSpot in ticketSpotArray)
        {
            lTicketSpot.OnPurchaseRequested -= moneyControllerRef.TicketSpotPurchase;
            lTicketSpot.OnUpgradeShopClicked -= shopRef.OpenSpecificWindow;

            bingoCageRef.OnNewBall -= lTicketSpot.AutoStamperRef.VerifyGrid;
        }
        shopRef.OnAnyItemUpgradeRequested -= moneyControllerRef.UpgradeAugment;

    }
}
