using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BagContaining 
{
    public List<IBallEffect> bagBalls {  get; private set; }
    RandomNumberGenerator rand = new RandomNumberGenerator();

    public BagContaining (int pBallAmount, Func<IBallEffect> createBallFunc, int pPlayerNumberBalls)
    {
        GD.Print("crreate a new bag");
        bagBalls = new List<IBallEffect> ();

        while (bagBalls.Count <= pBallAmount)
        {
            GD.Print("add a ball " + bagBalls.Count);

            int lBallNumber = rand.RandiRange(0, pBallAmount - 1);
            IBallEffect lNewBall = createBallFunc();


            if ( !bagBalls.Contains(lNewBall)) bagBalls.Add(lNewBall);

        }
    }

}
