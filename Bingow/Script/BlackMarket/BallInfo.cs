using Godot;
using System;

public partial class BallInfo : Node
{
	[Export] private TextureRect ballTxRectRef;
	[Export] private Label ballName;
	[Export] private Label ballDesc;

	public void ShowBall(Texture2D pBallTexture, string pBallDesc, string pBallName)
	{
		ballTxRectRef.Texture = pBallTexture;
		ballName.Text = pBallName;


        ballDesc.Text = pBallDesc;

	}
}
