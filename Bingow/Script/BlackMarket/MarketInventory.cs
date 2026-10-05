using Godot;
using System;

public partial class MarketInventory : Node
{

    [Export] private GridContainer bagContainer;
    [Export] private PackedScene bagRef;

    private int bagPrice = 10;

    public void Initialize(int pBagsAmount, int pBallAmount, Func<IBallEffect> createBallFunc, int pPlayerNumberBalls)
    {
        foreach (Node child in bagContainer.GetChildren())
        {
            child.QueueFree();
        }

        for (int i = 0; i < pBagsAmount; i++)
        {
            BallBag lNewBag = (BallBag)bagRef.Instantiate();

            bagContainer.AddChild(lNewBag);

            lNewBag.Initialize(bagPrice, pBallAmount, createBallFunc, pPlayerNumberBalls);

        }
    }
}
