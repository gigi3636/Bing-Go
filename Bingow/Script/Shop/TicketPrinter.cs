using Godot;
using System;
using System.Collections.Generic;

public partial class TicketPrinter : Control
{
	[Export] private AutoTicketPrinting autoTicketPrintingRef;
    public PrinterUpgrade globalUpgrade { get; private set; } = new PrinterUpgrade();

    public override void _Ready()
    {
        TicketEventBus.OnUpgradeBought += UpdateUpgradesStatus;
        autoTicketPrintingRef.Initialize();
    }

    public override void _ExitTree()
    {
        TicketEventBus.OnUpgradeBought -= UpdateUpgradesStatus;
    }



    private void UpdateUpgradesStatus()
    {
        autoTicketPrintingRef.UpdateUpgradeStatus(globalUpgrade.unlockPrinter.level > 0 , globalUpgrade.printingSpeed.level , globalUpgrade.printerSize.level);
    }

}
