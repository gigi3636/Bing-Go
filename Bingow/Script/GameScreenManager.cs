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
            lTicketSpot.AutoStamperRef.Initialize(bingoCageRef.IsNumbersAllowed, bingoCageRef.GetCurrentBalls);
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
            lTicketSpot.OnUpgradeShopClicked -= shopRef.OpenSpecificWindow;

            bingoCageRef.OnNewBall -= lTicketSpot.AutoStamperRef.VerifyGrid;
        }
        shopRef.OnAnyItemUpgradeRequested -= moneyControllerRef.UpgradeAugment;

    }
}
