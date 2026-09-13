using Godot;
using System;
using System.Collections.Generic;

public partial class Shop : Control
{
    [Export] private Control ticketWindowRef;
    [Export] private Control upgradeWindowRef;
    [Export] private BoxContainer upgradeItemContainer;
    [Export] private PackedScene upgradeItemScene;
    [Export] private ScrollContainer upgradesScrollContainerRef;

    private const int UPGRADES_SIZE_MARGIN = 90;

    public event Action<TicketSpotUpgradeButton.ShopUpgrades, IUpgradeGroup, Action<IUpgradeGroup>> OnAnyItemUpgradeRequested;

    // add a ticket spot with a array of upgrades
    public void AddSpotItem(TicketSpot pTicketSpotRef, params TicketSpotUpgradeButton.ShopUpgrades[] pUpgrades)
    {
        ShopUpgradeItem lItem = CreateAndBindShopItem();

        string lTitle = $"Ticket spot {pTicketSpotRef.spotId}";

        // Cast the params array in upgrades List

        lItem.Initialize(lTitle, pTicketSpotRef.ticketSpotUpgrades, new List<TicketSpotUpgradeButton.ShopUpgrades>(pUpgrades));
    }

    public void AddUniqueItem(string pName, IUpgradeGroup pUpgradeGroup, params TicketSpotUpgradeButton.ShopUpgrades[] pUpgrades)
    {
        ShopUpgradeItem lItem = CreateAndBindShopItem();

        lItem.Initialize(pName, pUpgradeGroup, new List<TicketSpotUpgradeButton.ShopUpgrades>(pUpgrades));
    }

    // Create new shop item
    private ShopUpgradeItem CreateAndBindShopItem()
    {
        ShopUpgradeItem lItem = (ShopUpgradeItem)upgradeItemScene.Instantiate();

        // Link buyout upgrade event
        lItem.OnUpgradeRequested += (augment, upgrades, visualCb) =>
        {
            OnAnyItemUpgradeRequested?.Invoke(augment, upgrades, visualCb);
        };

        upgradeItemContainer.AddChild(lItem);

        return lItem;
    }

    private void RefreshAllUpgrades()
    {
        foreach (Node child in upgradeItemContainer.GetChildren())
        {
            if (child is ShopUpgradeItem upgradeItem)
            {
                upgradeItem.UpdateVisual();
            }
        }
    }

    private void ShowTicketsTab()
    {
        ticketWindowRef.Visible = true;
        upgradeWindowRef.Visible = false;
    }

    private void ShowUpgradesTab()
    {
        ticketWindowRef.Visible = false;
        upgradeWindowRef.Visible = true;
        RefreshAllUpgrades();
    }

    private void _on_shop_button_pressed()
    {
        ShowTicketsTab();
        Visible = !Visible;
    }

    private void _on_close_button_pressed()
    {
        Visible = false;
    }

    private void _on_tickets_button_pressed()
    {
        ShowTicketsTab();
    }

    private void _on_upgrades_button_pressed()
    {
        ShowUpgradesTab();
    }

    public async void OpenSpecificWindow(int pSpotNumber)
    {
        Visible = true;
        ShowUpgradesTab();

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        int childIndex = pSpotNumber - 1;

        if (childIndex >= 0 && childIndex < upgradeItemContainer.GetChildCount())
        {
            if (upgradeItemContainer.GetChild(childIndex) is Control targetItem)
            {
                upgradesScrollContainerRef.ScrollVertical = (int)targetItem.Position.Y;
            }
        }
    }
}