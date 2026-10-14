using Godot;
using System;

public partial class BallInfoListener : Node
{
    [Export] private BallInfo ballInfoRef;
    [Export] private BallUpgrader ballUpgraderRef;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public void OnVisualUpdateRequested(BallRes pBallRes)
    {
        ballInfoRef.ShowBall(pBallRes.texture, pBallRes.ballDesc, pBallRes.ballEffect.ToString());
        ballUpgraderRef.UpdateVisuel(pBallRes.ballLevel);
    }
}
