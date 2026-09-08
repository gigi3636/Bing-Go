using Godot;
using System;
using System.Collections.Generic;

public partial class TicketNumbersManager 
{
    private Dictionary<int,bool> ticketNumbersStamped;

    public TicketNumbersManager()
    {
        ticketNumbersStamped = new Dictionary<int, bool>();
    }


    public void UpdateStampedNumbersList(int pCellNumbers, bool pIsNumbersAllowed, bool pIsNumberAutoStamped)
    {
        if (pIsNumberAutoStamped && ticketNumbersStamped.ContainsKey(pCellNumbers)) return;
        else if (ticketNumbersStamped.ContainsKey(pCellNumbers)) ticketNumbersStamped.Remove(pCellNumbers);
        else ticketNumbersStamped.Add(pCellNumbers, pIsNumbersAllowed);


    }

    public bool IsNumbersStamped(int pCellNumbers)
    {
        return (ticketNumbersStamped.ContainsKey(pCellNumbers));
    }

    public bool IsNumberValidAndStamped(int pCellNumbers)
    {
        // Check if the key exist and if its true 
        if (ticketNumbersStamped.TryGetValue(pCellNumbers, out bool isAllowed))
        {
            return isAllowed; 
        }
        return false;
    }
}
