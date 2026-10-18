using Godot;
using System;

public partial class BingoCageUpgrade : IUpgradeGroup
{
    public UpgradeableStat ballManualCd { get; private set; } = new UpgradeableStat("Manual cooldown", new int[] { 50, 100, 200, 300, 5000 });
    public UpgradeableStat cageCapacity { get; private set; } = new UpgradeableStat("Cage capacity", new int[] { 1000, 200, 700, 999, 1000, 1100 });
    public UpgradeableStat ballPerS { get; private set; } = new UpgradeableStat("Auto ball par second", new int[] { 1500, 5000, 10000, 100000 });

    public UpgradeableStat[] AllUpgrades => new UpgradeableStat[]
    {
        ballManualCd,
        cageCapacity,
        ballPerS
    };

    public UpgradeableStat GetStat(TicketSpotUpgradeButton.ShopUpgrades pUpgradeType) => pUpgradeType switch
    {
        TicketSpotUpgradeButton.ShopUpgrades.ManualCd => ballManualCd,
        TicketSpotUpgradeButton.ShopUpgrades.CageCapacity => cageCapacity,
        TicketSpotUpgradeButton.ShopUpgrades.BallPerS => ballPerS,
        _ => null
    };
}