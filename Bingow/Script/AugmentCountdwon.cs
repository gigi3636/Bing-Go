using Godot;
using System;

public partial class AugmentCountdwon : Node
{
	private const float COUNTDOWN_TIME = 100;
	[Export] private Label countdownLabelRef;

	private Timer countdownTimer;

	public Action OnAugment;

	public override void _Ready()
	{
		countdownTimer = new Timer();
		countdownTimer.WaitTime = COUNTDOWN_TIME;
		countdownTimer.OneShot = true;
		countdownTimer.Timeout += OnTimerTimeOut;
		AddChild(countdownTimer);
		StartCountdown();

    }

	public override void _Process(double delta)
	{
        if (countdownTimer.TimeLeft > 0)
        {
            // Round to up
            int lTtotalSeconds = Mathf.CeilToInt((float)countdownTimer.TimeLeft);

            int lMinutes = lTtotalSeconds / 60;
            int lSeconds = lTtotalSeconds % 60;

            countdownLabelRef.Text = $"{lMinutes:00}:{lSeconds:00}";
        }
        else
        {
            countdownLabelRef.Text = "00:00";
        }
    }

	public void OnTimerTimeOut()
	{
        StartCountdown();
		OnAugment?.Invoke();

    }

    public void StartCountdown()
	{
		countdownTimer?.Start();
	}
}
