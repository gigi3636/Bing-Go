using Godot;
using System;

public partial class AugmentCountdwon : Node
{
    private const float COUNTDOWN_TIME = 10;
    [Export] private Label countdownLabelRef;
    [Export] private Label payoutLabelRef;

    private Timer countdownTimer;
    private Timer payoutTimer; 

    public Action OnAugment;
    public Action OnPayoutAction; 

    public override void _Ready()
    {
        countdownTimer = new Timer();
        countdownTimer.WaitTime = COUNTDOWN_TIME;
        countdownTimer.OneShot = true;
        countdownTimer.Timeout += OnTimerTimeOut;
        AddChild(countdownTimer);

        payoutTimer = new Timer();
        payoutTimer.WaitTime = COUNTDOWN_TIME / 3.0f; // 1/3 du temps principal
        payoutTimer.OneShot = true;
        payoutTimer.Timeout += OnPayoutTimeOut;
        AddChild(payoutTimer);

        StartCountdown();
        payoutTimer.Start();
    }

    public override void _Process(double delta)
    {
        if (countdownTimer.TimeLeft > 0)
        {
            int lTtotalSeconds = Mathf.CeilToInt((float)countdownTimer.TimeLeft);

            int lMinutes = lTtotalSeconds / 60;
            int lSeconds = lTtotalSeconds % 60;

            countdownLabelRef.Text = $"{lMinutes:00}:{lSeconds:00}";
        }
        else
        {
            countdownLabelRef.Text = "00:00";
        }

        if (payoutTimer.TimeLeft > 0)
        {
            int lTtotalSeconds = Mathf.CeilToInt((float)payoutTimer.TimeLeft);

            int lMinutes = lTtotalSeconds / 60;
            int lSeconds = lTtotalSeconds % 60;

            payoutLabelRef.Text = $"{lMinutes:00}:{lSeconds:00}";
        }
        else
        {
            payoutLabelRef.Text = "00:00";
        }
    }

    public void OnTimerTimeOut()
    {
        StartCountdown(); 
        OnAugment?.Invoke();
    }

    public void OnPayoutTimeOut()
    {
        OnPayoutAction?.Invoke();

        payoutTimer.Start(); 
    }

    public void StartCountdown()
    {
        countdownTimer?.Start();
    }
}