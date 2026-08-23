using Godot;
using System;

public partial class TicketEventBus : Node
{
    // Signal to spawn a new ticket
    public static event Action<TicketData> OnTicketRequested;

    public static event Action<Ticket> OnTicketOpened;

    public static event Action<TicketSpot> OnFreeTicketSpotEnter;
    public static event Action OnFreeTicketSpotExit;


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
}
