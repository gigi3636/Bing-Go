using Godot;
using System;

public partial class BallTimer : Node2D
{
    public const float BASE_TIME_BETWEEN_BALL = 1f;
    private Timer nextBallTimer;

    private float timeBetweenBall;

    public event Action OnBallSpawnerTimeout;

    public override void _Ready()
    {
        nextBallTimer = new Timer ();
        nextBallTimer.WaitTime = BASE_TIME_BETWEEN_BALL;
        nextBallTimer.OneShot = false;
        AddChild (nextBallTimer);
        // when augment is bought : nextBallTimer.Start();

        nextBallTimer.Timeout += EmitSpawnBallSignal;



    }

    private void EmitSpawnBallSignal()
    {

        OnBallSpawnerTimeout?.Invoke();
    }


}
