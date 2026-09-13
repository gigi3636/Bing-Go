using Godot;
using System;
using System.Collections.Generic;
using static TicketSpotUpgradeButton;

public partial class ShopUpgradeItem : Control
{
    [Export] private TextureRect upgradeIconRef;
    [Export] private Label upgradeName;

    [Export] private BoxContainer buttonsContainer;
    [Export] private PackedScene upgradeButtonScene;

    public IUpgradeGroup currentUpgradeGroupRef;

    public Action<ShopUpgrades, IUpgradeGroup, Action<IUpgradeGroup>> OnUpgradeRequested;

    public void Initialize(string pTitle, IUpgradeGroup pUpgradeGroup, IEnumerable<ShopUpgrades> upgradesToSpawn)
    {
        upgradeName.Text = pTitle;
        currentUpgradeGroupRef = pUpgradeGroup;

        SpawnButtons(upgradesToSpawn);
    }

    private void SpawnButtons(IEnumerable<ShopUpgrades> upgradesToSpawn)
    {
        foreach (ShopUpgrades upgradeType in upgradesToSpawn)
        {
            TicketSpotUpgradeButton newButton = upgradeButtonScene.Instantiate<TicketSpotUpgradeButton>();
            buttonsContainer.AddChild(newButton);

            newButton.Initialize(upgradeType);
            newButton.OnUpragdeButtonPressed += (upgrade) => HandleUpgradeRequest(upgrade);
        }

        UpdateVisual(); 
    }

    private void HandleUpgradeRequest(ShopUpgrades upgradeType)
    {
        OnUpgradeRequested?.Invoke(upgradeType, currentUpgradeGroupRef, UpdateVisualForCallback);
    }

    private void UpdateVisualForCallback(IUpgradeGroup pUpgradeGroup)
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (currentUpgradeGroupRef == null) return;

        foreach (Node child in buttonsContainer.GetChildren())
        {
            if (child is TicketSpotUpgradeButton button)
            {
                UpgradeableStat stat = currentUpgradeGroupRef.GetStat(button.buttonUpgrade);
                if (stat == null) continue;

                if (stat.level < stat.augmentPrice.Length)
                {
                    button.UpdateVisuals($"{stat.name} : {stat.level}", $"{stat.augmentPrice[stat.level]} $");
                    button.Disabled = false;
                }
                else
                {
                    button.Modulate = Colors.Gray;
                    button.UpdateVisuals($"{stat.name} : {stat.level}", "MAX");
                    button.Disabled = true;
                }
            }
        }
    }
}