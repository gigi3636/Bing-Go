using Godot;
using System;
using static TicketSpotUpgradeButton;

public partial class TicketSpotUpgradeScreen : Control
{
    public TicketSpotUpgrades currentSpotUpgradesRef;

    public Action<SpotUpgrades, TicketSpotUpgrades, Action<TicketSpotUpgrades> > OnUpgradeRequested;

    public event Action<TicketSpotUpgrades> OnVisualUpdateRequest;

    public override void _Ready()
    {
        Visible = false;
    }

    public void ShowSpotUpgrade(TicketSpotUpgrades pUpgradesToShow)
    {
        Visible= true;
        currentSpotUpgradesRef = pUpgradesToShow;
        OnVisualUpdateRequest?.Invoke(pUpgradesToShow);
    }

    private void _on_close_button_pressed()
    {
        CloseUpgrade();
    }

    public void CloseUpgrade()
    {

        Visible = false;
    }

}
