using Godot;
using System;
using static TicketSpotUpgradeButton;

public partial class UpgradeRequestManager : Node
{
	// Emit with the price of the upgrade and the upgrade requested
	[Export] private ShopUpgradeItem shopUpgradeItemRef;
    [Export] private UpgradeShopVisualManager visualManagerRef;

    public event Action<int, Action> OnUpgradeRequested;

	[Export] private TicketSpotUpgradeButton[] upgradeButtonArrayRef;


	public override void _Ready()
	{
		foreach (TicketSpotUpgradeButton lButton in upgradeButtonArrayRef)
		{

			lButton.OnUpragdeButtonPressed += UpgradeRequested;
		}
	}


	private void UpgradeRequested(SpotUpgrades pUpgradesRequested)
	{
        shopUpgradeItemRef.OnUpgradeRequested?.Invoke(pUpgradesRequested, shopUpgradeItemRef.currentSpotUpgradesRef, visualManagerRef.UpdateVisual);
        // Prnedre  button[id] avec id = spotUpgradeScreenRef.l'upgrade.level et lancer la demande au money controller et si c'est bon call back la methode upgrade 
    }

    public override void _ExitTree()
    {
        foreach (TicketSpotUpgradeButton lButton in upgradeButtonArrayRef)
        {

            lButton.OnUpragdeButtonPressed -= UpgradeRequested;
        }

        base._ExitTree();
    }
}
