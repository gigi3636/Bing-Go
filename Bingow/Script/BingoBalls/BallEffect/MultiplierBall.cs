public class MultiplierBall : IBallEffect
{
    private int amount;
    public MultiplierBall(int pAmount) => amount = pAmount;


    public void ApplyOnStamp(Ticket ticketRef, int cellNumber)
    { }

    // called when the bingo is calculated
    public void ApplyOnValidation(BingoRewardData rewardData)
    {
        rewardData.Multiplier += amount;
    }
}