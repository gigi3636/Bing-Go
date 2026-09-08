using Godot;
using System;

public partial class TicketSpotUpgradeButton : Button
{
    [Export] public SpotUpgrades buttonUpgrade { get; private set; }

    public event Action<SpotUpgrades> OnUpragdeButtonPressed;

    public override void _Ready()
    {
        base._Ready();
        Pressed += OnButtonPressed;

    }

    public enum SpotUpgrades
    {
        NONE = 0,
        AutoClicker,
        AutoFiller,
        AutoBingo,
        SizeUpgrade

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
