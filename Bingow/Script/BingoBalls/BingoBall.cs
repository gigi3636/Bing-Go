using Godot;
using System;

public partial class BingoBall : TextureRect
{
    [Export] private Label ballNumbersLabel;
    private int ballNumber;

    public int BallNumber => ballNumber;

    public void Initialize(int pBallNumber)
    {
        ballNumber = pBallNumber;
        ballNumbersLabel.Text = pBallNumber.ToString();
    }

}
