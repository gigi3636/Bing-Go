using Godot;
public partial class BingoManager : Node2D
{
    [ExportGroup("References de Base")]
    [Export] private TextureButton bingoButtonRef;
    [Export] private TicketFullScreen ticketFullScreenRef;
    [Export] private PlayerStatus playerStatusRef;

    [ExportGroup("UI Compteur ")]
    [Export] private Control scoreHudContainerRef;
    [Export] private Label baseScoreLabelRef;
    [Export] private Label multScoreLabelRef;
    [Export] private Label totalScoreLabelRef;
    [Export] private Label floatingPopupTemplateRef;

    [ExportGroup("Particules")]
    [Export] private GpuParticles2D confettiParticlesRef;
    [Export] private GpuParticles2D cellBurstParticlesRef;

    [ExportGroup("Reglages Rythme & Juice")]
    [Export] private float baseTimeBetweenCells = 0.36f;
    [Export] private float minTimeBetweenCells = 0.14f;
    [Export] private float accelerationPerCell = 0.035f;
    [Export] private Color baseFlashColor = new Color(1.6f, 2.2f, 2.5f);
    [Export] private Color multFlashColor = new Color(2.8f, 1.0f, 1.8f);
    [Export] private Color totalFlashColor = new Color(2.8f, 2.5f, 0.8f);

    private Ticket currentTicketRef;
    private Tween _bingoPulseTween;
    private bool _isScoringInProgress = false;

    private Vector2 _initialBaseLabelPos;
    private Vector2 _initialMultLabelPos;

    public override void _Ready()
    {
        ticketFullScreenRef.OnTicketUpdate += UpdateBingoButton;
        ticketFullScreenRef.OnTicketClose += StopParticles;
        TicketEventBus.OnBingoCalled += HandleBingoCall;

        if (scoreHudContainerRef != null)
            scoreHudContainerRef.Visible = false;

        if (floatingPopupTemplateRef != null)
            floatingPopupTemplateRef.Visible = false;

        if (baseScoreLabelRef != null)
            _initialBaseLabelPos = baseScoreLabelRef.Position;

        if (multScoreLabelRef != null)
            _initialMultLabelPos = multScoreLabelRef.Position;
    }

    public override void _ExitTree()
    {
        ticketFullScreenRef.OnTicketUpdate -= UpdateBingoButton;
        ticketFullScreenRef.OnTicketClose -= StopParticles;

        TicketEventBus.OnBingoCalled -= HandleBingoCall;
        base._ExitTree();
    }

    private void SpawnFloatingPopup(Vector2 globalPos, string text, Color color, float scaleMultiplier)
    {
        if (floatingPopupTemplateRef == null) return;

        Label popup = (Label)floatingPopupTemplateRef.Duplicate();
        AddChild(popup);

        popup.Visible = true;
        popup.TopLevel = true;
        popup.ZIndex = 150;
        popup.Text = text;
        popup.Modulate = color;
        popup.PivotOffset = popup.Size / 2f;
        popup.GlobalPosition = globalPos - (popup.Size / 2f);
        popup.Scale = Vector2.Zero;
        popup.RotationDegrees = (float)GD.RandRange(-14.0, 14.0);

        Vector2 targetPos = popup.GlobalPosition + new Vector2((float)GD.RandRange(-35.0, 35.0), -85f);

        Tween t = CreateTween();
        t.SetParallel(true);
        t.TweenProperty(popup, "scale", Vector2.One * scaleMultiplier, 0.18f)
         .SetTrans(Tween.TransitionType.Elastic)
         .SetEase(Tween.EaseType.Out);
        t.TweenProperty(popup, "global_position", targetPos, 0.55f)
         .SetTrans(Tween.TransitionType.Quint)
         .SetEase(Tween.EaseType.Out);
        t.TweenProperty(popup, "rotation_degrees", 0f, 0.25f)
         .SetTrans(Tween.TransitionType.Back)
         .SetEase(Tween.EaseType.Out);

        t.SetParallel(false);
        t.TweenProperty(popup, "scale", Vector2.Zero, 0.16f)
         .SetTrans(Tween.TransitionType.Back)
         .SetEase(Tween.EaseType.In);
        t.TweenCallback(Callable.From(popup.QueueFree));
    }

    private void PulseCounterLabel(Label label, float intensity, Color flashColor)
    {
        if (label == null) return;
        label.PivotOffset = label.Size / 2f;
        label.Scale = Vector2.One * intensity;
        label.RotationDegrees = (float)GD.RandRange(-8.0, 8.0) * (intensity - 1.0f);
        label.Modulate = flashColor;

        Tween t = CreateTween();
        t.SetParallel(true);
        t.TweenProperty(label, "scale", Vector2.One, 0.32f)
         .SetTrans(Tween.TransitionType.Elastic)
         .SetEase(Tween.EaseType.Out);
        t.TweenProperty(label, "rotation_degrees", 0f, 0.32f)
         .SetTrans(Tween.TransitionType.Elastic)
         .SetEase(Tween.EaseType.Out);
        t.TweenProperty(label, "modulate", Colors.White, 0.25f);
    }

    public void UpdateBingoButton(Ticket pTicketRef)
    {
        currentTicketRef = pTicketRef;
        if (_isScoringInProgress) return;

        bool wasVisible = bingoButtonRef.Visible;
        bingoButtonRef.Visible = pTicketRef.isCompleted;

        if (pTicketRef.isCompleted && !wasVisible)
        {
            TriggerBingoShowJuice();
        }
        else if (!pTicketRef.isCompleted)
        {
            _bingoPulseTween?.Kill();
            bingoButtonRef.Scale = Vector2.One;
        }
    }

    private async void TriggerBingoShowJuice()
    {
        bingoButtonRef.PivotOffset = bingoButtonRef.Size / 2f;
        bingoButtonRef.Scale = new Vector2(2.8f, 2.8f);
        bingoButtonRef.RotationDegrees = -8f;
        bingoButtonRef.Modulate = new Color(2.5f, 2.5f, 1.5f, 1f);

        Engine.TimeScale = 0.08;
        await ToSignal(GetTree().CreateTimer(0.08f, true, false, true), SceneTreeTimer.SignalName.Timeout);
        Engine.TimeScale = 1.0;

        ShakeCamera2D.Instance?.AddTrauma(2.75f);
        if (confettiParticlesRef != null)
        {
            confettiParticlesRef.Restart();
            confettiParticlesRef.Emitting = true;
        }

        _bingoPulseTween?.Kill();
        Tween introTween = CreateTween();
        introTween.SetParallel(true);
        introTween.TweenProperty(bingoButtonRef, "scale", Vector2.One, 0.45f)
                  .SetTrans(Tween.TransitionType.Elastic)
                  .SetEase(Tween.EaseType.Out);
        introTween.TweenProperty(bingoButtonRef, "rotation_degrees", 0f, 0.45f)
                  .SetTrans(Tween.TransitionType.Elastic)
                  .SetEase(Tween.EaseType.Out);
        introTween.TweenProperty(bingoButtonRef, "modulate", Colors.White, 0.25f);

        introTween.Finished += StartIdleBingoPulse;
    }

    private void StartIdleBingoPulse()
    {
        if (!bingoButtonRef.Visible) return;
        _bingoPulseTween?.Kill();
        _bingoPulseTween = CreateTween().SetLoops();
        _bingoPulseTween.TweenProperty(bingoButtonRef, "scale", new Vector2(1.06f, 0.94f), 0.35f)
                        .SetTrans(Tween.TransitionType.Sine)
                        .SetEase(Tween.EaseType.InOut);
        _bingoPulseTween.TweenProperty(bingoButtonRef, "scale", new Vector2(0.96f, 1.04f), 0.35f)
                        .SetTrans(Tween.TransitionType.Sine)
                        .SetEase(Tween.EaseType.InOut);
    }

    private void _on_bingo_text_pressed()
    {
        HandleBingoCall(currentTicketRef);
    }

    public async void HandleBingoCall(Ticket pTicket)
    {
        if (pTicket == null || _isScoringInProgress) return;
        _isScoringInProgress = true;
        bingoButtonRef.Disabled = true;
        _bingoPulseTween?.Kill();

        bool isVisualValidation = ticketFullScreenRef.Visible && ticketFullScreenRef.CurrentTicket == pTicket;

        if (isVisualValidation)
        {
            Tween clickTween = CreateTween();
            clickTween.TweenProperty(bingoButtonRef, "scale", new Vector2(1.25f, 0.75f), 0.06f);
            clickTween.TweenProperty(bingoButtonRef, "scale", Vector2.Zero, 0.16f)
                      .SetTrans(Tween.TransitionType.Back)
                      .SetEase(Tween.EaseType.In);
            clickTween.TweenCallback(Callable.From(() => bingoButtonRef.Visible = false));
        }

        BingoRewardData lReward = new BingoRewardData { BaseMoney = 0, Multiplier = 1.0f };
        float lValuePerCell = pTicket.Value / (float)pTicket.winningLineNumbers.Count;

        bool isLigneValide = true;
        foreach (int number in pTicket.winningLineNumbers)
        {
            if (!pTicket.IsNumberValidAndStamped(number))
            {
                isLigneValide = false;
                break;
            }
        }

        if (!isLigneValide)
        {
            GD.Print("ticket pas valide");
            ShakeCamera2D.Instance?.AddDirectionalShake(Vector2.Left, 6f);
            pTicket.DeleteTicket();
            if (isVisualValidation) ticketFullScreenRef.CloseTicket();
            bingoButtonRef.Disabled = false;
            bingoButtonRef.Scale = Vector2.One;
            _isScoringInProgress = false;
            return;
        }

        if (isVisualValidation && scoreHudContainerRef != null)
        {
            scoreHudContainerRef.Visible = true;
            if (baseScoreLabelRef != null)
            {
                baseScoreLabelRef.Visible = true;
                baseScoreLabelRef.Position = _initialBaseLabelPos;
                baseScoreLabelRef.Scale = Vector2.One;
                baseScoreLabelRef.Text = "0 $";
            }
            if (multScoreLabelRef != null)
            {
                multScoreLabelRef.Visible = true;
                multScoreLabelRef.Position = _initialMultLabelPos;
                multScoreLabelRef.Scale = Vector2.One;
                multScoreLabelRef.Text = "x1";
            }
            if (totalScoreLabelRef != null)
            {
                totalScoreLabelRef.Visible = false;
            }

            scoreHudContainerRef.PivotOffset = scoreHudContainerRef.Size / 2f;
            scoreHudContainerRef.Scale = Vector2.Zero;

            Tween hudIntro = CreateTween();
            hudIntro.TweenProperty(scoreHudContainerRef, "scale", Vector2.One, 0.28f)
                    .SetTrans(Tween.TransitionType.Back)
                    .SetEase(Tween.EaseType.Out);

            await ToSignal(GetTree().CreateTimer(0.18f), SceneTreeTimer.SignalName.Timeout);
        }

        int comboStep = 0;
        int specialChainCount = 0;

        foreach (int lCurrentNumber in pTicket.winningLineNumbers)
        {
            comboStep++;
            lReward.BaseMoney += lValuePerCell;
            int displayCellVal = Mathf.Max(1, Mathf.RoundToInt(lValuePerCell));
            int currentBaseTotal = Mathf.RoundToInt(lReward.BaseMoney);

            IBallEffect lEffect = pTicket.GetEffectOfNumber(lCurrentNumber);
            bool hasSpecialEffect = lEffect != null && !(lEffect is BasicBall);

            if (isVisualValidation)
            {
                BingoCell cellNode = ticketFullScreenRef.GetCellByNumber(lCurrentNumber);
                Vector2 cellCenter = cellNode != null
                    ? cellNode.GlobalPosition + (cellNode.Size / 2f)
                    : ticketFullScreenRef.GlobalPosition;

                cellNode?.PlayScoringPop(false, comboStep);
                if (baseScoreLabelRef != null)
                {
                    baseScoreLabelRef.Text = $"{currentBaseTotal} $";
                    float baseScale = 1.25f + Mathf.Min(comboStep * 0.08f, 0.55f);
                    PulseCounterLabel(baseScoreLabelRef, baseScale, baseFlashColor);
                }

                SpawnFloatingPopup(cellCenter, $"+{displayCellVal} $", baseFlashColor, 1.0f + (comboStep * 0.06f));

                if (cellBurstParticlesRef != null)
                {
                    cellBurstParticlesRef.GlobalPosition = cellCenter;
                    cellBurstParticlesRef.Restart();
                    cellBurstParticlesRef.Emitting = true;
                }

                ShakeCamera2D.Instance?.AddTrauma(0.18f + Mathf.Min(comboStep * 0.05f, 0.35f));

                float currentDelay = Mathf.Max(minTimeBetweenCells, baseTimeBetweenCells - (comboStep * accelerationPerCell));
                await ToSignal(GetTree().CreateTimer(currentDelay), SceneTreeTimer.SignalName.Timeout);

                if (hasSpecialEffect)
                {
                    specialChainCount++;
                    float prevMult = lReward.Multiplier;
                    lEffect.ApplyOnValidation(lReward);
                    float newMult = lReward.Multiplier;

                    Engine.TimeScale = 0.06;
                    await ToSignal(GetTree().CreateTimer(0.05f + Mathf.Min(specialChainCount * 0.02f, 0.08f), true, false, true), SceneTreeTimer.SignalName.Timeout);
                    Engine.TimeScale = 1.0;

                    cellNode?.PlayScoringPop(true, comboStep + specialChainCount * 2);

                    string effectPopupText = newMult > prevMult
                        ? $"x{newMult:0.#} MULT!"
                        : "BONUS!";

                    if (multScoreLabelRef != null)
                    {
                        multScoreLabelRef.Text = $"x{newMult:0.#}";
                        float multPulse = 1.55f + Mathf.Min(specialChainCount * 0.18f, 0.9f);
                        PulseCounterLabel(multScoreLabelRef, multPulse, multFlashColor);
                    }

                    SpawnFloatingPopup(cellCenter + new Vector2(0, -25f), effectPopupText, multFlashColor, 1.35f + (specialChainCount * 0.12f));

                    if (cellBurstParticlesRef != null)
                    {
                        cellBurstParticlesRef.GlobalPosition = cellCenter;
                        cellBurstParticlesRef.Restart();
                        cellBurstParticlesRef.Emitting = true;
                    }

                    ShakeCamera2D.Instance?.AddTrauma(0.45f + Mathf.Min(specialChainCount * 0.15f, 0.5f));
                    await ToSignal(GetTree().CreateTimer(currentDelay * 0.85f), SceneTreeTimer.SignalName.Timeout);
                }
            }
            else
            {
                if (hasSpecialEffect)
                {
                    lEffect.ApplyOnValidation(lReward);
                }
            }
        }

        int lFinalMoney = lReward.CalculateTotal();

        if (isVisualValidation && scoreHudContainerRef != null)
        {
            if (baseScoreLabelRef != null && multScoreLabelRef != null)
            {
                float meetX = (_initialBaseLabelPos.X + _initialMultLabelPos.X) / 2f;
                Tween slamTween = CreateTween();
                slamTween.SetParallel(true);
                slamTween.TweenProperty(baseScoreLabelRef, "position:x", meetX, 0.12f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.In);
                slamTween.TweenProperty(multScoreLabelRef, "position:x", meetX, 0.12f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.In);
                slamTween.TweenProperty(baseScoreLabelRef, "scale", Vector2.Zero, 0.12f);
                slamTween.TweenProperty(multScoreLabelRef, "scale", Vector2.Zero, 0.12f);

                await ToSignal(slamTween, Tween.SignalName.Finished);

                baseScoreLabelRef.Visible = false;
                multScoreLabelRef.Visible = false;
            }

            Engine.TimeScale = 0.05;
            await ToSignal(GetTree().CreateTimer(0.09f, true, false, true), SceneTreeTimer.SignalName.Timeout);
            Engine.TimeScale = 1.0;

            if (totalScoreLabelRef != null)
            {
                totalScoreLabelRef.Visible = true;
                totalScoreLabelRef.Text = $"+{lFinalMoney} $ !";
                PulseCounterLabel(totalScoreLabelRef, 1.85f, totalFlashColor);
            }

            if (confettiParticlesRef != null)
            {
                confettiParticlesRef.Restart();
                confettiParticlesRef.Emitting = true;
            }

            ShakeCamera2D.Instance?.AddTrauma(0.95f);
            await ToSignal(GetTree().CreateTimer(0.65f), SceneTreeTimer.SignalName.Timeout);

            Tween hideHud = CreateTween();
            hideHud.TweenProperty(scoreHudContainerRef, "scale", Vector2.Zero, 0.16f)
                   .SetTrans(Tween.TransitionType.Back)
                   .SetEase(Tween.EaseType.In);
            await ToSignal(hideHud, Tween.SignalName.Finished);
            scoreHudContainerRef.Visible = false;
        }

        playerStatusRef.AddMoney(lFinalMoney);
        GD.Print($"BINGO TERMINE : Total gagne = {lFinalMoney}$");

        pTicket.DeleteTicket();
        if (isVisualValidation)
        {
            ticketFullScreenRef.CloseTicket();
        }

        bingoButtonRef.Disabled = false;
        bingoButtonRef.Scale = Vector2.One;
        _isScoringInProgress = false;
    }

    private void StopParticles()
    {
        confettiParticlesRef.Restart();
        cellBurstParticlesRef.Restart();
        confettiParticlesRef.Emitting = false;
        cellBurstParticlesRef.Emitting = false;
    }
}

public class BingoRewardData
{
    public float BaseMoney { get; set; }
    public float Multiplier { get; set; } = 1.0f;

    public int CalculateTotal()
    {
        return (int)(BaseMoney * Multiplier);
    }
}
