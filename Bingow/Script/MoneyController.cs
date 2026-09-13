using Godot;
using System;
using static TicketSpotUpgradeButton;

public partial class MoneyController : Node
{
	[Export] private PlayerStatus playerStatusRes;
	[Export] private BingoCage bingoCageRef;
	[Export] private AugmentCountdwon augmentCountdwonRef;


	public event Action<int> OnUpdateBallPrice;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bingoCageRef.OnNewBall += OnBallBuyed;
		augmentCountdwonRef.OnAugment += IncreaseBallsCost;

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
    public void UpgradeAugment(ShopUpgrades pAugment, IUpgradeGroup pUpgradeGroup, Action<IUpgradeGroup> pUpgradeVisual)
    {
       UpgradeableStat lUpgradeRequested = pUpgradeGroup.GetStat(pAugment);

        if (lUpgradeRequested.augmentPrice[lUpgradeRequested.level] <= playerStatusRes.playerCurrentMoney)
        {
            playerStatusRes.DiscountMoney(lUpgradeRequested.augmentPrice[lUpgradeRequested.level]);
            lUpgradeRequested.Upgrade();

            pUpgradeVisual(pUpgradeGroup);
        }
        else
        {
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
