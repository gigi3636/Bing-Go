using Godot;
using System;

public partial class TicketEventBus : Node
{
    // Signal to spawn a new ticket
    public static event Action<TicketData> OnTicketRequested;

    public static event Action<Ticket> OnTicketOpened;

    public static event Action<TicketSpot> OnFreeTicketSpotEnter;
    public static event Action OnFreeTicketSpotExit;

    public static event Action OnTicketAutoStamped;

    public static event Action OnUpgradeBought;
    public static event Action<Ticket> OnBingoCalled;



    public static void PublishTicketRequested(TicketData pTicketData)
    {
        OnTicketRequested?.Invoke(pTicketData);
    }

    public static void OpenTicket(Ticket pTicket)
    {
        OnTicketOpened?.Invoke(pTicket);
    }

    public static void EnterFreeTikcetSpot(TicketSpot pTicketSpot)
    {
        OnFreeTicketSpotEnter?.Invoke(pTicketSpot);
    }
    public static void ExitTikcetSpot()
    {
        OnFreeTicketSpotExit?.Invoke();
    }

    public static void UpdateAutoStampedTicket()
    {
        OnTicketAutoStamped?.Invoke();
    }

    public static void UpdateUpgradesStatus()
    {
        OnUpgradeBought?.Invoke();
    }

    public static void PublishBingoCalled(Ticket pTicket)
    {
        OnBingoCalled?.Invoke(pTicket);
    }
}
