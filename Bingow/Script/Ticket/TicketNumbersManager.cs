using Godot;
using System;
using System.Collections.Generic;

public partial class TicketNumbersManager 
{
    private Dictionary<int, StampedCellData> ticketNumbersStamped;

    public TicketNumbersManager()
    {
        ticketNumbersStamped = new Dictionary<int, StampedCellData>();
    }


    public void UpdateStampedNumbersList(int pCellNumbers, bool pIsNumbersAllowed, IBallEffect pBallEffect, bool pIsNumberAutoStamped, Ticket pTicket)
    {
        // if the auto stamper stamp an alredy stamped number => nothing
        if (pIsNumberAutoStamped && ticketNumbersStamped.ContainsKey(pCellNumbers)) return;

        // stamp an alredy stamped number => unstamp the number
        else if (ticketNumbersStamped.ContainsKey(pCellNumbers)) ticketNumbersStamped.Remove(pCellNumbers);

        // stamp a new number => add this number to the stamped list
        else
        {
            ticketNumbersStamped.Add(pCellNumbers, new StampedCellData(pIsNumbersAllowed, pBallEffect));

            if (pIsNumbersAllowed) pBallEffect.ApplyOnStamp(pTicket, pCellNumbers);



        }


    }

    public bool IsNumbersStamped(int pCellNumbers)
    {
        return (ticketNumbersStamped.ContainsKey(pCellNumbers));
    }

    public bool IsNumberValidAndStamped(int pCellNumbers)
    {
        // Check if the key exist and if its true 
        if (ticketNumbersStamped.TryGetValue(pCellNumbers, out StampedCellData cellData))
        {
            return cellData.IsAllowed;
        }
        return false;
    }

    public IBallEffect GetNumberEffect(int pCellNumbers)
    {
        if (ticketNumbersStamped.TryGetValue(pCellNumbers, out StampedCellData cellData))
        {
            return cellData.Effect;
        }
        return null;
    }

    // Get all the ball effect 
    public List<IBallEffect> GetAllValidEffects()
    {
        List<IBallEffect> validEffects = new List<IBallEffect>();

        foreach (var kvp in ticketNumbersStamped)
        {
            // Si le numéro est autorisé ET qu'il possède un effet
            if (kvp.Value.IsAllowed && kvp.Value.Effect != null)
            {
                validEffects.Add(kvp.Value.Effect);
            }
        }
        return validEffects;
    }
}

public class StampedCellData
{
    public bool IsAllowed { get; private set; }
    public IBallEffect Effect { get; private set; }

    public StampedCellData(bool isAllowed, IBallEffect effect)
    {
        IsAllowed = isAllowed;
        Effect = effect;
    }
}
