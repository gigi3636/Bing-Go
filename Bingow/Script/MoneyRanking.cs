using Godot;
using System;

public partial class MoneyRanking : Node
{
    [Export] private PlayerStatus playerStatusRes;

    [Export] private Control upperContainer;
    [Export] private Control playerContainer;
    [Export] private Control lowerContainer;

    [Export] private Label nextMoneyLabelRef;
    [Export] private Label lastMoneyLabelRef;
    [Export] private Label playerRankLabel;
    [Export] private Label nexRankLabel;
    [Export] private Label lastRankLabel;

    private int nextMoneyCap;
    private int lastMoneyCap;
    private int playerRanking;
    private const int PLAYER_NUMBER = 100;

    private RandomNumberGenerator rand = new RandomNumberGenerator();
    private bool isAnimating = false;

    private float upperOriginalY;
    private float playerOriginalY;
    private float lowerOriginalY;


    private double botSimulationTimer = 0;

    public override void _Process(double delta)
    {
        if (isAnimating) return;

        botSimulationTimer += delta;
        if (botSimulationTimer >= 2.0) 
        {
            botSimulationTimer = 0;
            SimulateBots();
        }

        CheckRankProgress();
    }

    private void SimulateBots()
    {
        float nextBotChange = rand.RandfRange(-0.015f, 0.035f);
        float lastBotChange = rand.RandfRange(-0.015f, 0.035f);

        nextMoneyCap += Mathf.RoundToInt(nextMoneyCap * nextBotChange);
        lastMoneyCap += Mathf.RoundToInt(lastMoneyCap * lastBotChange);

        if (nextMoneyCap <= playerStatusRes.playerCurrentMoney)
        {
            nextMoneyCap = playerStatusRes.playerCurrentMoney + 1;
        }
        if (lastMoneyCap >= playerStatusRes.playerCurrentMoney)
        {
            lastMoneyCap = playerStatusRes.playerCurrentMoney - 1;
        }

        UpdateUI();
    }


    public override void _Ready()
    {
        nextMoneyCap = Mathf.RoundToInt(playerStatusRes.playerCurrentMoney * (rand.RandfRange(1.05f, 1.3f)));
        lastMoneyCap = Mathf.RoundToInt(playerStatusRes.playerCurrentMoney * (rand.RandfRange(0.7f, 0.95f)));
        playerRanking = PLAYER_NUMBER - 7;

        upperOriginalY = upperContainer.Position.Y;
        playerOriginalY = playerContainer.Position.Y;
        lowerOriginalY = lowerContainer.Position.Y;

        UpdateUI();
    }

    private void CheckRankProgress()
    {
        if (playerStatusRes.playerCurrentMoney >= nextMoneyCap)
        {
            RankUpSequence();
        }
        else if (playerStatusRes.playerCurrentMoney <= lastMoneyCap)
        {
            RankDownSequence();
        }
    }

    private void RankUpSequence()
    {
        isAnimating = true;

        playerRanking--;
        lastMoneyCap = nextMoneyCap;
        nextMoneyCap = Mathf.RoundToInt(playerStatusRes.playerCurrentMoney * (rand.RandfRange(1.05f, 1.3f)));

        playerContainer.SelfModulate = nextMoneyLabelRef.LabelSettings.FontColor;

        Tween tween = CreateTween();
        tween.SetParallel(true);

        playerContainer.PivotOffset = playerContainer.Size / 2;
        tween.TweenProperty(playerContainer, "scale", new Vector2(1.2f, 1.2f), 0.15f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
        tween.TweenProperty(playerContainer, "scale", new Vector2(1f, 1f), 0.2f).SetDelay(0.15f);

        tween.TweenProperty(upperContainer, "position:y", lowerOriginalY, 0.4f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);

        tween.TweenProperty(lowerContainer, "position:y", lowerOriginalY + 60f, 0.3f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
        tween.TweenProperty(lowerContainer, "modulate:a", 0f, 0.3f);

        tween.Chain().TweenCallback(Callable.From(() =>
        {
            upperContainer.Position = new Vector2(upperContainer.Position.X, upperOriginalY - 60f);
            upperContainer.Modulate = new Color(1, 1, 1, 0f);

            lowerContainer.Position = new Vector2(lowerContainer.Position.X, lowerOriginalY);
            lowerContainer.Modulate = new Color(1, 1, 1, 1f);

            UpdateUI();

            Tween enterTween = CreateTween();
            enterTween.SetParallel(true);
            enterTween.TweenProperty(upperContainer, "position:y", upperOriginalY, 0.3f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
            enterTween.TweenProperty(upperContainer, "modulate:a", 1f, 0.2f);

            enterTween.Chain().TweenCallback(Callable.From(() => { isAnimating = false; }));

            playerContainer.SelfModulate = Colors.White  ;

        }));
    }

    private void RankDownSequence()
    {
        isAnimating = true;

        playerRanking++;
        nextMoneyCap = lastMoneyCap;
        lastMoneyCap = Mathf.RoundToInt(playerStatusRes.playerCurrentMoney * (rand.RandfRange(0.7f, 0.95f)));

        playerContainer.SelfModulate = lastMoneyLabelRef.LabelSettings.FontColor;

        Tween tween = CreateTween();
        tween.SetParallel(true);

        playerContainer.PivotOffset = playerContainer.Size / 2;
        tween.TweenProperty(playerContainer, "scale", new Vector2(0.9f, 0.9f), 0.15f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
        tween.TweenProperty(playerContainer, "scale", new Vector2(1f, 1f), 0.2f).SetDelay(0.15f);

        tween.TweenProperty(lowerContainer, "position:y", upperOriginalY, 0.4f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);

        tween.TweenProperty(upperContainer, "position:y", upperOriginalY - 60f, 0.3f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
        tween.TweenProperty(upperContainer, "modulate:a", 0f, 0.3f);

        tween.Chain().TweenCallback(Callable.From(() =>
        {
            lowerContainer.Position = new Vector2(lowerContainer.Position.X, lowerOriginalY + 60f);
            lowerContainer.Modulate = new Color(1, 1, 1, 0f);

            upperContainer.Position = new Vector2(upperContainer.Position.X, upperOriginalY);
            upperContainer.Modulate = new Color(1, 1, 1, 1f);

            UpdateUI();

            Tween enterTween = CreateTween();
            enterTween.SetParallel(true);
            enterTween.TweenProperty(lowerContainer, "position:y", lowerOriginalY, 0.3f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
            enterTween.TweenProperty(lowerContainer, "modulate:a", 1f, 0.2f);

            enterTween.Chain().TweenCallback(Callable.From(() => { isAnimating = false; }));
            playerContainer.SelfModulate = Colors.White;

        }));
    }

    private void UpdateUI()
    {
        nextMoneyLabelRef.Text = nextMoneyCap.ToString();
        lastMoneyLabelRef.Text = lastMoneyCap.ToString();

        playerRankLabel.Text = "#" + playerRanking.ToString();
        nexRankLabel.Text = "#" + (playerRanking - 1).ToString();
        lastRankLabel.Text = "#" + (playerRanking + 1).ToString();
    }
}