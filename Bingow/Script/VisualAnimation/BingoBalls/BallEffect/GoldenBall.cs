using Godot;
using System;
using System.Collections.Generic;

// COmplete one line or column 
public class GoldenBall : IBallEffect
{
    public GoldenBall() { }

    public void ApplyOnStamp(Ticket ticketRef, int cellNumber)
    {
        GD.Print("STAMPED LINE OR COLUMN");

        // take coordinate of the stamped number
        Vector2I coords = ticketRef.GetCoordinatesOfNumber(cellNumber);

        // if number isnt in the ticket
        if (coords.X == -1) return;

        // take bingo grid data
        List<int> numbers = ticketRef.GetTicketNumbers();
        int col = ticketRef.Column;
        int row = numbers.Count / col; // number of line

        // decide if its a column or a line 
        bool fillRow = (GD.Randi() % 2 == 0);

        if (fillRow)
        {
            // complete all the column
            for (int x = 0; x < col; x++)
            {
                int index = (coords.Y * col) + x;
                int targetNumber = numbers[index];

                // Stamp if its not stamped
                if (!ticketRef.IsNumberStamped(targetNumber))
                {
                    // basic ball to not have an infinite effect call back
                    ticketRef.UpdateStampedNumber(targetNumber, true, new BasicBall(), true);
                }
            }
        }
        else
        {
            // complete all the row
            for (int y = 0; y < row; y++)
            {
                int index = (y * col) + coords.X;
                int targetNumber = numbers[index];

                // Stamp if its not stamped
                if (!ticketRef.IsNumberStamped(targetNumber))
                {
                    // basic ball to not have an infinite effect call back
                    ticketRef.UpdateStampedNumber(targetNumber, true, new BasicBall(), true);
                }
            }
        }
    }

    public void ApplyOnValidation(BingoRewardData rewardData)
    {

    }
}