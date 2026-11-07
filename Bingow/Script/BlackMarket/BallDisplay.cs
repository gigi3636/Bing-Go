using Godot;
using System;

public partial class BallDisplay : Node
{
    [Export] private BallRes[] ballRes;

    [Export] private VBoxContainer mainContainer;
    [Export] private PackedScene blackMarrketBallVisuelScene;
    [Export] private BallInfoListener ballInfoRef;

    [Export] private int rowSpacing = -8;

    [Export] private int separationRowOfTwo = 104;
    [Export] private int separationRowOfThree = 96;

    public override void _Ready()
    {
        mainContainer.AddThemeConstantOverride("separation", rowSpacing);

        foreach (Node rowNode in mainContainer.GetChildren())
        {
            foreach (Node child in rowNode.GetChildren())
            {
                if (child is BlackMarrketBallVisuel lBall)
                {
                    lBall.OnBallClicked -= ballInfoRef.OnVisualUpdateRequested;
                }
            }
            mainContainer.RemoveChild(rowNode); 
            rowNode.QueueFree();
        }

        bool isRowOfTwo = true;
        int currentItems = 0;
        HBoxContainer currentRow = null;

        for (int i = 0; i < ballRes.Length; i++)
        {
            int maxItemsInRow = isRowOfTwo ? 2 : 3;

            if (currentRow == null || currentItems >= maxItemsInRow)
            {
                if (currentRow != null)
                {
                    isRowOfTwo = !isRowOfTwo;
                    maxItemsInRow = isRowOfTwo ? 2 : 3;
                }

                currentRow = new HBoxContainer();
                currentRow.Alignment = BoxContainer.AlignmentMode.Center;
                currentRow.AddThemeConstantOverride(
                    "separation",
                    isRowOfTwo ? separationRowOfTwo : separationRowOfThree
                );
                mainContainer.AddChild(currentRow);

                currentItems = 0;
            }

            BallRes ball = ballRes[i];
            BlackMarrketBallVisuel lBall =
                (BlackMarrketBallVisuel)blackMarrketBallVisuelScene.Instantiate();

            lBall.Initialize(ball);
            lBall.Texture = ball.miniTexture;
            lBall.OnBallClicked += ballInfoRef.OnVisualUpdateRequested;

            currentRow.AddChild(lBall);
            currentItems++;
        }
    }
}