using Godot;
using System;
using static TicketSpotUpgradeButton;

public partial class MoneyController : Node
{
	[Export] private PlayerStatus playerStatusRes;
	[Export] private BingoCage bingoCageRef;
	[Export] private AugmentCountdwon augmentCountdwonRef;
	[Export] private TicketSpotUpgradeScreen ticketSpotUpgradeScreenRef;


	public event Action<int> OnUpdateBallPrice;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bingoCageRef.OnNewBall += OnBallBuyed;
		augmentCountdwonRef.OnAugment += IncreaseBallsCost;
		ticketSpotUpgradeScreenRef.OnUpgradeRequested += UpgradeAugment;

    }


	private void OnBallBuyed(int pBallNumber)
	{
		playerStatusRes.BuyBall();
	}

	private void IncreaseBallsCost()
	{
        playerStatusRes.IncreaseBallsPrice();
		OnUpdateBallPrice?.Invoke(playerStatusRes.bingoBallsCurrentPrice);

    }

	// Check if enough money to upgrade a ticket spot 
	private void UpgradeAugment(SpotUpgrades pAugment, TicketSpotUpgrades pTicketSpotUpgrades, Action<TicketSpotUpgrades> pUpgradeVisual)
	{
        UpgradeableStat lUpgradeRequested = pTicketSpotUpgrades.AllUpgrades[(int)pAugment -1];

		if (lUpgradeRequested.augmentPrice[lUpgradeRequested.level] <= playerStatusRes.playerCurrentMoney)
		{
			playerStatusRes.DiscountMoney(lUpgradeRequested.augmentPrice[lUpgradeRequested.level]);
			lUpgradeRequested.Upgrade();

			pUpgradeVisual(pTicketSpotUpgrades);


        }
		else
		{
            GD.Print(lUpgradeRequested.level);

            GD.Print(lUpgradeRequested.augmentPrice[lUpgradeRequested.level]);
			GD.Print(" no money u broke asf u cant ");
		}

	}

	public void TicketSpotPurchase(int pSpotCost , Action pBuySpot)
	{
		if (pSpotCost <= playerStatusRes.playerCurrentMoney)
		{
			playerStatusRes.DiscountMoney(pSpotCost);
			pBuySpot();
		}
	}

    public override void _ExitTree()
    {
        bingoCageRef.OnNewBall -= OnBallBuyed;
        augmentCountdwonRef.OnAugment -= IncreaseBallsCost;

        base._ExitTree();
    }
}
