using Godot;
using System;

public partial class PrinterUpgrade : IUpgradeGroup
{

    public UpgradeableStat unlockPrinter { get; private set; } = new UpgradeableStat("Unlock", new int[] { 50});
    public UpgradeableStat printingSpeed { get; private set; } = new UpgradeableStat("Printer speed", new int[] { 10, 20, 30, 100 , 200 , 70, 80, 90, 100, 110 });
    public UpgradeableStat printerSize { get; private set; } = new UpgradeableStat("Printer size", new int[] { 10, 20, 30, 40, 50, 60,  });

    public UpgradeableStat[] AllUpgrades => new UpgradeableStat[]
    {
        printingSpeed
    };


    // return thet stats with the enum type in param
    public UpgradeableStat GetStat(TicketSpotUpgradeButton.ShopUpgrades pUpgradeType) => pUpgradeType switch
    {
        TicketSpotUpgradeButton.ShopUpgrades.UnlockPrinter => unlockPrinter,
        TicketSpotUpgradeButton.ShopUpgrades.PintingSpeed => printingSpeed,
        TicketSpotUpgradeButton.ShopUpgrades.PrinterSize => printerSize,
        _ => null
    };
}
