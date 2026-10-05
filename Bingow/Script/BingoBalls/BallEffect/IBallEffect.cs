public interface IBallEffect
{
    // called when a number is stamped
    void ApplyOnStamp(Ticket ticketRef, int cellNumber);

    // called when the bingo is calculated
    void ApplyOnValidation(BingoRewardData rewardData);
}