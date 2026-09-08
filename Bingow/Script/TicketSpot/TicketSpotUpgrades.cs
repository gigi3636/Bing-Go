using Godot;
using System;

public partial class TicketSpotUpgrades
{
    // Contain all the upgrades stats of one ticket spot 
    public UpgradeableStat autoStamperUpgrade { get; private set; } = new UpgradeableStat("Auto clicker", new int[] {50, 1000, 2000 , 5000 });
    public UpgradeableStat autoFillerUpgrade { get; private set; } = new UpgradeableStat("Auto filler", new int[] { 1000, 200, 700, 999 , 1000 , 1100 });
    public UpgradeableStat autoBingoUpgrade { get; private set; } = new UpgradeableStat("Auto bingo", new int[] { 1500, 5000, 10000 });
    public UpgradeableStat sizeUpgrade { get; private set; } = new UpgradeableStat("Size level", new int[] { 500, 2000, 3000 });

    public UpgradeableStat[] AllUpgrades => new UpgradeableStat[] 
    {
        autoStamperUpgrade,
        autoFillerUpgrade,
        autoBingoUpgrade,
        sizeUpgrade
    };

}
