using Godot;
using System;
using System.Collections.Generic;

public class CrossBall : IBallEffect
{
    public CrossBall() { }

    public void ApplyOnStamp(Ticket ticketRef, int cellNumber)
    {
        GD.Print("STAMPED");

        // take coordinate of the stamped number
        Vector2I coords = ticketRef.GetCoordinatesOfNumber(cellNumber);

        // if number isnt in the ticket
        if (coords.X == -1) return;

        // take bingo grid data
        List<int> numbers = ticketRef.GetTicketNumbers();
        int col = ticketRef.Column;
        int row = numbers.Count / col; // number of line

        // define the number stamped based on the stamped number pose
        Vector2I[] directions = new Vector2I[]
        {
            new Vector2I(0, -1), // up
            new Vector2I(0, 1),  // down
            new Vector2I(-1, 0), // left
            new Vector2I(1, 0)   // right
        };

        // Check each affected bingo cell
        foreach (Vector2I dir in directions)
        {
            int neighborX = coords.X + dir.X;
            int neighborY = coords.Y + dir.Y;

            // check if the neighboor isnt outside the bingo grid
            if (neighborX >= 0 && neighborX < col && neighborY >= 0 && neighborY < row)
            {
                // cast coordinate in Array ID
                int index = (neighborY * col) + neighborX;
                int neighborNumber = numbers[index];

                // Stamp if its not stamped
                if (!ticketRef.IsNumberStamped(neighborNumber))
                {
                    // basic ball to note have an infinite effect call back
                    ticketRef.UpdateStampedNumber(neighborNumber, true, new BasicBall(), true);
                }
            }
        }

    }

    public void ApplyOnValidation(BingoRewardData rewardData)
    {

    }
}