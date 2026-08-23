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


    public void UpdateStampedNumbersList(int pCellNumbers, bool pIsNumbersAllowed)
    {
        if (ticketNumbersStamped.ContainsKey(pCellNumbers)) ticketNumbersStamped.Remove(pCellNumbers);
        else ticketNumbersStamped.Add(pCellNumbers, pIsNumbersAllowed);
    }

    public bool IsNumbersStamped(int pCellNumbers)
    {
        return (ticketNumbersStamped.ContainsKey(pCellNumbers));
    }
}
