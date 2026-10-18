using Godot;
using System;

public partial class LockedItem : Control
{
    public Action OnUnlockButtonPressed;

    private void _on_unlock_button_pressed()
    {
        OnUnlockButtonPressed?.Invoke();

        //ticketSpotRef.OnPurchaseRequested?.Invoke(ticketSpotRef.unlockCost, ticketSpotRef.UnlockTicket);

        //ticketSpotRef.OnPurchaseRequested?.Invoke(ticketSpotRef.unlockCost, ticketSpotRef.UnlockTicket);
    }
}
