using Godot;
using System;

public partial class ShopTicket : Control
{

    [Export] private Control ticketWindowRef;
    [Export] private Control upgradeWindowRef;
    [Export] private BoxContainer upgradeItemContainer;
    [Export] private PackedScene upgradeItemScene;

    public event Action<TicketSpotUpgradeButton.SpotUpgrades, TicketSpotUpgrades, Action<TicketSpotUpgrades>> OnAnyItemUpgradeRequested;

    public void Initialize(TicketSpot pTicketSpotRef)
    {
        ShopUpgradeItem lItem = (ShopUpgradeItem)upgradeItemScene.Instantiate();

        lItem.OnUpgradeRequested += (augment, upgrades, visualCb) =>
        {
            OnAnyItemUpgradeRequested?.Invoke(augment, upgrades, visualCb);
        };

        lItem.Initialize(pTicketSpotRef);

        upgradeItemContainer.AddChild(lItem);

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

    private void _on_shop_button_pressed()
    {
        
        Visible = !Visible;
    }

    private void _on_close_button_pressed()
    {
        Visible = !Visible;

    }

    private void _on_tickets_button_pressed()
    {
        ticketWindowRef.Visible = true;
        upgradeWindowRef.Visible = false;
        RefreshAllUpgrades();
    }

    // ici
    private void _on_upgrades_button_pressed()
    {
        ticketWindowRef.Visible = false;
        upgradeWindowRef.Visible = true;
        RefreshAllUpgrades();
    }


}
