using Godot;
using System;
using System.Reflection.Emit;

public partial class TicketSpot : Node2D
{
    #region Exports
    [Export] private bool isPrimaryUnlocked;
    [Export] public int unlockCost { get; private set; }
    [Export] private AutoStamper autoStamperRef;
    [Export] private AutoFiller autoFillerRef;
    [Export] private AutoFillerVisualMover autoFillerVisualMoverRef;
    [Export] private AutoBingo autoBingoRef;
    #endregion

    #region Public Properties
    public AutoStamper AutoStamperRef => autoStamperRef;
    public AutoFiller AutoFillerRef => autoFillerRef;
    public bool isUnlocked { get; private set; }
    public bool isUsed { get; private set; } = false;
    public Ticket ticketContainer { get; private set; }
    public TicketSpotUpgrades ticketSpotUpgrades { get; private set; }
    #endregion

    #region Events
    public Action<int, Action> OnPurchaseRequested;
    public event Action<bool> OnVisualUpdate;
    public event Action<TicketSpotUpgrades> OnUpgradeShopClicked;
    #endregion

    #region Godot Lifecycle Methods
    public override void _Ready()
    {
        ticketSpotUpgrades = new TicketSpotUpgrades();

        isUnlocked = isPrimaryUnlocked;
        OnVisualUpdate?.Invoke(isUnlocked);

        autoFillerRef.OnTicketDisponible += OnAutoffiledTicket;
        TicketEventBus.OnUpgradeBought += UpdateUpgradesStatus;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        autoFillerRef.OnTicketDisponible -= OnAutoffiledTicket;

        TicketEventBus.OnUpgradeBought -= UpdateUpgradesStatus;
    }
    #endregion

    #region Public Methods
    //When a new ticket is draggede in the empty spot
    public void SetNewTicket(Ticket pTicket)
    {
        isUsed = true;
        ticketContainer = pTicket;
        autoStamperRef.SetNewTicket(pTicket);
        ticketContainer.OnTicketCompleted += autoBingoRef.HandleTicketCompleted;

        pTicket.OnEndOfTicket += DeleteTicket;
    }

    // Return if the size of the spot can handle the size of the ticket
    public bool isTicketSizeAllowed(int pSizeLevel)
    {
        if (pSizeLevel <= ticketSpotUpgrades.sizeUpgrade.level) return true;
        return false;
    }


    private void OnAutoffiledTicket(Ticket pTicket, float pDuration)
    {
        SetNewTicket(pTicket);
        autoFillerVisualMoverRef.AutoFillerMove(pTicket, this, pDuration);
    }
    
    public void DeleteTicket()
    {
        isUsed = false;
        ticketContainer.OnTicketCompleted -= autoBingoRef.HandleTicketCompleted;

        ticketContainer.OnEndOfTicket -= DeleteTicket;
        ticketContainer.QueueFree();
        ticketContainer = null;

        autoFillerRef.CheckDisponibility();
    }

    public void UnlockTicket()
    {
        isUnlocked = true;
        OnVisualUpdate?.Invoke(isUnlocked);
    }
    #endregion

    #region Private Methods & Signal Handlers
    // When upgrades is bought
    private void UpdateUpgradesStatus()
    {
        autoStamperRef.UpdateUpgradeStatus(ticketSpotUpgrades.autoStamperUpgrade.level);
        autoFillerRef.UpdateUpgradeStatus(ticketSpotUpgrades.autoFillerUpgrade.level);
        autoBingoRef.UpdateUpgradeStatus(ticketSpotUpgrades.autoBingoUpgrade.level);
    }


    private void _on_upgrade_shop_button_pressed()
    {
        OnUpgradeShopClicked?.Invoke(ticketSpotUpgrades);
    }
    #endregion
}