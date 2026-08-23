using Godot;
using System;
using System.Collections.Generic;

public partial class BallSpawner : Node2D
{
    [Export] private PackedScene ballScene;
    [Export] private Node2D ballContainerRef;

    public BingoBall SpawnBall(int  pBallNumbers)
    {
        BingoBall lBall = (BingoBall)ballScene.Instantiate();
        lBall.Initialize(pBallNumbers);

        return lBall;
    }

}
