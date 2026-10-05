using Godot;
using System;
using System.Collections.Generic;

// valid one random number
public class RandomBall : IBallEffect
{
    public RandomBall() { }
    private RandomNumberGenerator rand = new RandomNumberGenerator();


    public void ApplyOnStamp(Ticket ticketRef, int cellNumber)
    {
        GD.Print("STAMPED");

        // take coordinate of the stamped number
        Vector2I coords = ticketRef.GetCoordinatesOfNumber(cellNumber);

        // if number isnt in the ticket
        if (coords.X == -1) return;

        // take bingo grid data
        List<int> numbers = ticketRef.GetTicketNumbers();

        // Gather all currently unstamped numbers
        List<int> availableNumbers = new List<int>();
        foreach (int num in numbers)
        {
            if (!ticketRef.IsNumberStamped(num))
            {
                availableNumbers.Add(num);
            }
        }

        // If there are available spots left, pick one randomly
        if (availableNumbers.Count > 0)
        {
            int randomIndex = rand.RandiRange(0, availableNumbers.Count - 1);
            int lRandNumber = availableNumbers[randomIndex];

            // basic ball to not have an infinite effect call back
            ticketRef.UpdateStampedNumber(lRandNumber, true, new BasicBall(), true);
        }
    }

    public void ApplyOnValidation(BingoRewardData rewardData)
    {

    }
}