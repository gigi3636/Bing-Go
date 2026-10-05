    using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BallListManager : Upgrade
{

    private Queue<BingoBall> currentBingoBalls = new Queue<BingoBall>();

    private int containerCapacity = 4;

    public override void _Ready()
    {
        upgradesLevelsAmount = new List<float> { 4, 5, 6, 7, 8, 9 , 10 };
    }


    public void AddBall(BingoBall pBingoBall)
    {
        if (currentBingoBalls.Count < containerCapacity)
        {
            currentBingoBalls.Enqueue(pBingoBall);
        }
        else
        {
            BingoBall lBallDoQuit = currentBingoBalls.Dequeue();
            lBallDoQuit.QueueFree();

            currentBingoBalls.Enqueue(pBingoBall);

        }
    }

    public List<int> GetBallNumbers()
    {
        List<int> lBallNumbers = new List<int>();
        foreach( BingoBall lBall in currentBingoBalls)
        {
            lBallNumbers.Add(lBall.BallNumber);
        }

        return lBallNumbers;
    }

    public override void UpdateUpgradeStatus(int pCurrentLevel)
    {
        base.UpdateUpgradeStatus(pCurrentLevel);

        containerCapacity = (int)upgradesLevelsAmount[pCurrentLevel];
    }


}
