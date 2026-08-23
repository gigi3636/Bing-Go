using Godot;
using System;
using System.Collections.Generic;

public partial class BasicTicket : Ticket
{

    private RandomNumberGenerator rand = new RandomNumberGenerator();

    protected override void SetupTicket()
    {
        ticketNumbers = new List<int>();

        for (int y = 0; y < row; y++)
        { 
            for (int x = 0; x  < column; x++)
            {
                int lCellNumber = rand.RandiRange(0, higherNumber);

                while (ticketNumbers.Contains(lCellNumber)) lCellNumber = rand.RandiRange(0, higherNumber);

                ticketNumbers.Add(lCellNumber);
            }
        }

    }
}
