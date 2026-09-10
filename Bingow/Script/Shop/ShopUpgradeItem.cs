using Godot;
using System;
using static TicketSpotUpgradeButton;

public partial class ShopUpgradeItem : Control
{
    [Export] private TextureRect upgradeIconRef;
    [Export] private Label upgradeName;
    public TicketSpotUpgrades currentSpotUpgradesRef;


    public Action<SpotUpgrades, TicketSpotUpgrades, Action<TicketSpotUpgrades>> OnUpgradeRequested;

    public event Action<TicketSpotUpgrades> OnVisualUpdateRequest;


    public void Initialize(TicketSpot pSpotRef)
    {
        upgradeName.Text = $"{"Ticket spot " +  pSpotRef.spotId}";
        currentSpotUpgradesRef = pSpotRef.ticketSpotUpgrades;   
    }

    public void UpdateVisual()
    {
        OnVisualUpdateRequest?.Invoke(currentSpotUpgradesRef);
    }

}
