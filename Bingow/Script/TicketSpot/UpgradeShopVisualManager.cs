using Godot;

public partial class UpgradeShopVisualManager : Node
{

	[Export] private Label[] upgradesLabelsRef;
    [Export] private TicketSpotUpgradeButton[] upgradeButtonsRef;
	[Export] private TicketSpotUpgradeScreen upgradeScreenRef;


	public override void _Ready()
	{
		upgradeScreenRef.OnVisualUpdateRequest += UpdateVisual;

    }

	public void UpdateVisual(TicketSpotUpgrades pUpgraseData)
	{
        //Update the level of the upgrade
		upgradesLabelsRef[0].Text = $"{pUpgraseData.autoStamperUpgrade.name} : {pUpgraseData.autoStamperUpgrade.level}";
        upgradesLabelsRef[1].Text = $"{pUpgraseData.autoFillerUpgrade.name} : {pUpgraseData.autoFillerUpgrade.level}";
        upgradesLabelsRef[2].Text = $"{pUpgraseData.autoBingoUpgrade.name} : {pUpgraseData.autoBingoUpgrade.level}";
        upgradesLabelsRef[3].Text = $"{pUpgraseData.sizeUpgrade.name} : {pUpgraseData.sizeUpgrade.level}";

        //Update the price of the upgrade
        upgradeButtonsRef[0].Text = $"{pUpgraseData.autoStamperUpgrade.augmentPrice[pUpgraseData.autoStamperUpgrade.level ]} $";
        upgradeButtonsRef[1].Text = $"{pUpgraseData.autoFillerUpgrade.augmentPrice[pUpgraseData.autoFillerUpgrade.level ]} $";
        upgradeButtonsRef[2].Text = $"{pUpgraseData.autoBingoUpgrade.augmentPrice[pUpgraseData.autoBingoUpgrade.level ]} $";
        upgradeButtonsRef[3].Text = $"{pUpgraseData.sizeUpgrade.augmentPrice[pUpgraseData.sizeUpgrade.level]} $";

    }

    public override void _ExitTree()
    {
        upgradeScreenRef.OnVisualUpdateRequest -= UpdateVisual;

        base._ExitTree();
    }
}
