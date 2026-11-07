using System.Collections.Generic;

public partial class BallListManager : Upgrade
{
    private Queue<BingoBall> currentBingoBalls = new Queue<BingoBall>();
    public int ContainerCapacity { get; private set; } = 4;

    public Queue<BingoBall> CurrentBingoBalls => currentBingoBalls;

    public override void _Ready()
    {
        upgradesLevelsAmount = new List<float> { 4, 5, 6, 7, 8, 9, 10 };
    }

    public BingoBall AddBall(BingoBall pBingoBall)
    {
        BingoBall ballToEject = null;

        if (currentBingoBalls.Count >= ContainerCapacity)
        {
            ballToEject = currentBingoBalls.Dequeue();
        }

        currentBingoBalls.Enqueue(pBingoBall);
        return ballToEject;
    }

    public List<int> GetBallNumbers()
    {
        List<int> lBallNumbers = new List<int>();
        foreach (BingoBall lBall in currentBingoBalls)
        {
            lBallNumbers.Add(lBall.BallNumber);
        }
        return lBallNumbers;
    }

    public override void UpdateUpgradeStatus(int pCurrentLevel)
    {
        base.UpdateUpgradeStatus(pCurrentLevel);
        ContainerCapacity = (int)upgradesLevelsAmount[pCurrentLevel];
    }
}