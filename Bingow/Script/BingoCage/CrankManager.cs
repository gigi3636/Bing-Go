using Godot;
using System;
using System.Collections.Generic;

public partial class CrankManager : Upgrade
{
    [Export] private AnimatedSprite2D cageLeverAnim;
    [Export] private AnimatedSprite2D ballCagedAnim;
    [Export] private TextureRect redBingoCageRef;

    public Action OnCrankActionned;

    private int currentLevelIndex = 0;

    public override void _Ready()
    {
        isActive = true;
        upgradesLevelsAmount = new List<float> { 4f, 3f, 2.5f, 2f, 1f, 0.5f, 0.1f };
        reactionTimeTimer = new Timer();
        reactionTimeTimer.OneShot = true;

        reactionTimeTimer.Timeout += OnReactionTimeFinished;


        cageLeverAnim.AnimationLooped += OnLoopFinished;

        AddChild(reactionTimeTimer);
    }

    public override void _ExitTree()
    {
        reactionTimeTimer.Timeout -= OnReactionTimeFinished;

        cageLeverAnim.AnimationLooped -= OnLoopFinished;
    }

    public void _on_crank_button_pressed()
    {
        if (reactionTimeTimer.TimeLeft == 0 && !cageLeverAnim.IsPlaying())
        {
            redBingoCageRef.Visible = false;
            GD.Print(reactionTimeTimer.WaitTime);
            cageLeverAnim.Play();
            reactionTimeTimer.Start(currentReactionTime);

            ballCagedAnim.Play();
        }
    }

    private void OnLoopFinished()
    {
        if (reactionTimeTimer.TimeLeft != 0) return;
        OnCrankActionned?.Invoke();

        cageLeverAnim.Stop();
        ballCagedAnim.Stop();
        redBingoCageRef.Visible = true;


    }

    private void OnReactionTimeFinished()
    {

    }

    public override void UpdateUpgradeStatus(int pCurrentLevel)
    {
        isActive = true;

        if (isActive && upgradesLevelsAmount.Count >= pCurrentLevel)
        {
            currentReactionTime = upgradesLevelsAmount[pCurrentLevel];

            if (reactionTimeTimer != null)
            {
                reactionTimeTimer.WaitTime = currentReactionTime;
            }
        }

    }
}