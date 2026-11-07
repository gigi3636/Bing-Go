using Godot;
using System;
using System.Collections.Generic;

public partial class AutoBall : Upgrade
{

    [Export] private AnimatedSprite2D cageLeverAnim;
    [Export] private AnimatedSprite2D ballCagedAnim;
    [Export] private TextureRect redBingoCageRef;


    public const float BASE_TIME_BETWEEN_BALL = 1f;

    private int loopCount = 0;
    private const int TARGET_LOOPS = 5;

    private float timeBetweenBall;

    public event Action OnBallSpawnerTimeout;

    private bool isAutoCd;

    public override void _Ready()
    {
        upgradesLevelsAmount = new List<float> { 2f, 1f, 0.8f, 0.1f, 0.1f, 0.1f };

        reactionTimeTimer = new Timer();
        reactionTimeTimer.WaitTime = BASE_TIME_BETWEEN_BALL;
        reactionTimeTimer.OneShot = false;
        AddChild(reactionTimeTimer);
        // when augment is bought : nextBallTimer.Start();

        reactionTimeTimer.Timeout += AnimateCage;


        cageLeverAnim.AnimationLooped += OnLoopFinished;



    }

    private void OnLoopFinished()
    {
        if (!isAutoCd) return;

        loopCount++;
        GD.Print("Loop = " + loopCount);

        if (loopCount >= TARGET_LOOPS)
        {
            GD.Print("spawn ball");

            OnBallSpawnerTimeout?.Invoke();
            isAutoCd = false;   
            //cageLeverAnim.Stop();
            //ballCagedAnim.Stop();
            //redBingoCageRef.Visible = true;

        }
    }

    public void AnimateCage()
    {
        isAutoCd = true;
        GD.Print("animate cage reset loop");
        redBingoCageRef.Visible = false;

        loopCount = 0;

        string animName = cageLeverAnim.Animation;
        float fps = (float)cageLeverAnim.SpriteFrames.GetAnimationSpeed(animName);
        int frameCount = cageLeverAnim.SpriteFrames.GetFrameCount(animName);
        float baseAnimDuration = frameCount / fps;
        float requiredSpeedScale = (baseAnimDuration * TARGET_LOOPS) / currentReactionTime;

        cageLeverAnim.SpeedScale = requiredSpeedScale;
        ballCagedAnim.SpeedScale = requiredSpeedScale;

        cageLeverAnim.Play();
        ballCagedAnim.Play();

        reactionTimeTimer.Start(currentReactionTime);


    }

    public override void UpdateUpgradeStatus(int pCurrentLevel)
    {
        base.UpdateUpgradeStatus(pCurrentLevel);


        if ( isActive)
        {
            reactionTimeTimer.Start();
            AnimateCage();
        }
    }

    private void EmitSpawnBallSignal()
    {

    }
}
