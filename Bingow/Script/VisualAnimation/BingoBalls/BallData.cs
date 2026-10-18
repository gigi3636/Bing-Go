using Godot;
using System;

public class BallData
{
    public int ballNumber { get; private set; }
    public IBallEffect ballEffect { get; private set; }

    public Texture2D texture { get; private set; }

    public EBallId ballId { get; private set; }

    public BallData(int number, IBallEffect initialEffect, Texture2D pTextureRect , EBallId pBallId)
    {
        ballNumber = number;
        ballEffect = initialEffect;
        texture = pTextureRect;
        ballId = pBallId;

    }

}

public enum EBallId
{
    Basic = 0,
    CrossBall = 1,
    GoldenBall = 2,
    MultiplierBall = 3,
    RabdomBall = 4,
    SquarreBall = 5
}
