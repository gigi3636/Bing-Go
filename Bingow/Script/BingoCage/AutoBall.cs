using Godot;
using System;
using System.Collections.Generic;

public partial class AutoBall : Upgrade
{
    public const float BASE_TIME_BETWEEN_BALL = 1f;
    private Timer nextBallTimer;

    private float timeBetweenBall;

    public event Action OnBallSpawnerTimeout;

    public override void _Ready()
    {
        upgradesLevelsAmount = new List<float> { 0f, 1f, 0.8f, 0.5f, 0.3f, 0.1f };

        nextBallTimer = new Timer();
        nextBallTimer.WaitTime = BASE_TIME_BETWEEN_BALL;
        nextBallTimer.OneShot = false;
        AddChild(nextBallTimer);
        // when augment is bought : nextBallTimer.Start();

        nextBallTimer.Timeout += EmitSpawnBallSignal;



    }

    public override void UpdateUpgradeStatus(int pCurrentLevel)
    {
        base.UpdateUpgradeStatus(pCurrentLevel);


        if ( isActive)
        {
            nextBallTimer.Start();
        }
    }

    private void EmitSpawnBallSignal()
    {

        OnBallSpawnerTimeout?.Invoke();
    }
}
