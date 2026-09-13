using Godot;
using System;

public partial class TicketSpotUpgradeButton : TextureButton
{
    public ShopUpgrades buttonUpgrade { get; private set; }
    [Export] private Label buttonLabel; // price
    [Export] private Label nameLabel;   // name and level

    public event Action<ShopUpgrades> OnUpragdeButtonPressed;

    public enum ShopUpgrades
    {
        NONE = 0,
        AutoClicker,
        AutoFiller,
        AutoBingo,
        SizeUpgrade,

        UnlockPrinter,
        PintingSpeed,
        PrinterSize
    }

    public override void _Ready()
    {
        base._Ready();
        Pressed += OnButtonPressed;
    }

    public void Initialize(ShopUpgrades pUpgradeType)
    {
        buttonUpgrade = pUpgradeType;
    }

    public void UpdateVisuals(string pNameAndLevel, string pPrice)
    {
        nameLabel.Text = pNameAndLevel;
        buttonLabel.Text = pPrice;
    }

    public void OnButtonPressed()
    {
        OnUpragdeButtonPressed?.Invoke(buttonUpgrade);
        TicketEventBus.UpdateUpgradesStatus();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Pressed -= OnButtonPressed;
    }
}