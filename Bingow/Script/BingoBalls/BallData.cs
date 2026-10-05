using Godot;
using System;

public class BallData
{
    public int ballNumber { get; private set; }
    public IBallEffect ballEffect { get; private set; }

    public Texture2D texture { get; private set; }

    public BallData(int number, IBallEffect initialEffect, Texture2D pTextureRect )
    {
        ballNumber = number;
        ballEffect = initialEffect ;
        texture = pTextureRect ;

    }

}
