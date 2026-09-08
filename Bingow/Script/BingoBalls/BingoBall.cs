using Godot;
using System;

public partial class BingoBall : Control
{
    [Export] private Label ballNumbersLabel;
    [Export] private TextureRect ballTextureRef;

    private int ballNumber;

    private RandomNumberGenerator rand = new RandomNumberGenerator();

    public int BallNumber => ballNumber;

    public void Initialize(int pBallNumber)
    {
        ballNumber = pBallNumber;
        ballNumbersLabel.Text = pBallNumber.ToString();

        ballTextureRef.RotationDegrees = rand.RandfRange(-20, 20);

    }

}
