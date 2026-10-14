using Godot;
using System;

public partial class BallDisplay : Node
{
    [Export] private BallRes[] ballRes;

    [Export] private GridContainer gridContainer;
    [Export] private PackedScene blackMarrketBallVisuelScene;

    [Export] private Vector2I spacing = new Vector2I(50, 50);
    [Export] private BallInfoListener ballInfoRef;
    public override void _Ready()
    {
        gridContainer.AddThemeConstantOverride("h_separation", spacing.X);
        gridContainer.AddThemeConstantOverride("v_separation", spacing.Y);

        foreach ( var lChild in gridContainer.GetChildren() )
        {
            if (lChild is BlackMarrketBallVisuel lBall)
            {
                lBall.OnBallClicked -= ballInfoRef.OnVisualUpdateRequested;

            }

            lChild.QueueFree();
        }

        foreach (BallRes ball in ballRes)
        {
            BlackMarrketBallVisuel lBall = (BlackMarrketBallVisuel)blackMarrketBallVisuelScene.Instantiate();

            lBall.Initialize(ball);

            lBall.OnBallClicked += ballInfoRef.OnVisualUpdateRequested;

            gridContainer.AddChild(lBall);


        }
    }
}