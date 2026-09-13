using Godot;
using System;

public partial class BingoCageUpgrade : IUpgradeGroup
{
    public UpgradeableStat ballCapacityUpgrade { get; private set; } = new UpgradeableStat("Ball capacity", new int[] { 50, 100, 200, 300, 5000 });
    public UpgradeableStat ballSpeedUpgrade { get; private set; } = new UpgradeableStat("Ball speed", new int[] { 1000, 200, 700, 999, 1000, 1100 });
    public UpgradeableStat autoBallUpgrade { get; private set; } = new UpgradeableStat("Auto ball", new int[] { 1500, 5000, 10000, 100000 });

    public UpgradeableStat[] AllUpgrades => new UpgradeableStat[]
    {
        ballCapacityUpgrade,
        ballSpeedUpgrade,
        autoBallUpgrade
    };

    public UpgradeableStat GetStat(TicketSpotUpgradeButton.ShopUpgrades pUpgradeType) => pUpgradeType switch
    {
        TicketSpotUpgradeButton.ShopUpgrades.AutoBingo => ballCapacityUpgrade,
        TicketSpotUpgradeButton.ShopUpgrades.AutoClicker => ballSpeedUpgrade,
        TicketSpotUpgradeButton.ShopUpgrades.SizeUpgrade => autoBallUpgrade,
        _ => null
    };
}