using Godot;
using System;
using System.Collections.Generic;

public partial class CardInteraction : Node2D
{
    [ExportGroup("References des Cartes")]
    [Export] public Control CardLeft;
    [Export] public Control CardMid;
    [Export] public Control CardRight;

    [ExportGroup("Parametres du Survol (Hover)")]
    [Export] public float HoverScale = 1.12f;
    [Export] public float HoverLift = -25.0f; 
    [Export] public float MaxTiltAngle = 6.0f; 

    private Dictionary<Control, Vector2> _initialPositions = new();
    private Dictionary<Control, Tween> _activeTweens = new();

    private Control _hoveredCard = null;
    private bool _hasSelected = false;

    [Signal]
    public delegate void CardChosenEventHandler(int cardIndex);

    public override void _Ready()
    {
        CardLeft ??= GetNode<Control>("../CardLeft");
        CardMid ??= GetNode<Control>("../CardMid");
        CardRight ??= GetNode<Control>("../CarRight"); 

        Control[] cards = { CardLeft, CardMid, CardRight };

        for (int i = 0; i < cards.Length; i++)
        {
            Control card = cards[i];
            int index = i;

            Callable.From(() => SetupCard(card, index)).CallDeferred();
        }
    }

    private void SetupCard(Control card, int index)
    {
        card.PivotOffset = card.Size / 2.0f;
        _initialPositions[card] = card.Position;

        card.MouseFilter = Control.MouseFilterEnum.Stop;
        foreach (Node child in card.GetChildren())
        {
            if (child is Control childControl)
                childControl.MouseFilter = Control.MouseFilterEnum.Ignore;
        }

        card.MouseEntered += () => OnCardHoverEnter(card);
        card.MouseExited += () => OnCardHoverExit(card);
        card.GuiInput += (InputEvent @event) => OnCardGuiInput(@event, card, index);
    }

    public override void _Process(double delta)
    {
        if (_hasSelected) return;

        if (_hoveredCard != null && IsInstanceValid(_hoveredCard))
        {
            Vector2 localMouse = _hoveredCard.GetLocalMousePosition();
            float centerX = _hoveredCard.Size.X / 2.0f;
            float percentX = Mathf.Clamp((localMouse.X - centerX) / centerX, -1.0f, 1.0f);

            float targetRotation = percentX * MaxTiltAngle;
            _hoveredCard.RotationDegrees = Mathf.Lerp(_hoveredCard.RotationDegrees, targetRotation, (float)delta * 15.0f);
        }

        foreach (var card in _initialPositions.Keys)
        {
            if (card != _hoveredCard && Mathf.Abs(card.RotationDegrees) > 0.05f)
            {
                card.RotationDegrees = Mathf.Lerp(card.RotationDegrees, 0.0f, (float)delta * 12.0f);
            }
        }
    }

    private void OnCardHoverEnter(Control card)
    {
        if (_hasSelected) return;

        _hoveredCard = card;
        card.ZIndex = 10; 

        KillCardTween(card);
        Tween tween = CreateTween().SetParallel(true);
        _activeTweens[card] = tween;

        tween.SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
        tween.TweenProperty(card, "scale", Vector2.One * HoverScale, 0.25f);
        tween.TweenProperty(card, "position:y", _initialPositions[card].Y + HoverLift, 0.25f);
    }

    private void OnCardHoverExit(Control card)
    {
        if (_hasSelected) return;

        if (_hoveredCard == card)
            _hoveredCard = null;

        card.ZIndex = 0;

        KillCardTween(card);
        Tween tween = CreateTween().SetParallel(true);
        _activeTweens[card] = tween;

        tween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
        tween.TweenProperty(card, "scale", Vector2.One, 0.2f);
        tween.TweenProperty(card, "position", _initialPositions[card], 0.2f);
    }

    private void OnCardGuiInput(InputEvent @event, Control selectedCard, int index)
    {
        if (_hasSelected) return;

        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            SelectCardAndClose(selectedCard, index);
        }
    }

    private void SelectCardAndClose(Control selectedCard, int index)
    {
        _hasSelected = true;
        _hoveredCard = null;
        selectedCard.ZIndex = 20;

        foreach (var card in _initialPositions.Keys)
            KillCardTween(card);

        Tween masterTween = CreateTween();

        masterTween.TweenProperty(selectedCard, "scale", Vector2.One * 0.9f, 0.07f)
            .SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
        masterTween.Parallel().TweenProperty(selectedCard, "rotation_degrees", 0.0f, 0.07f);

        masterTween.Parallel().TweenProperty(selectedCard, "modulate", new Color(1.8f, 1.8f, 1.8f, 1.0f), 0.07f);

        masterTween.TweenProperty(selectedCard, "scale", Vector2.One * 1.25f, 0.25f)
            .SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
        masterTween.Parallel().TweenProperty(selectedCard, "modulate", Colors.White, 0.2f);

        foreach (var card in _initialPositions.Keys)
        {
            if (card != selectedCard)
            {
                masterTween.Parallel().TweenProperty(card, "position:y", card.Position.Y + 180.0f, 0.25f)
                    .SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.In);
                masterTween.Parallel().TweenProperty(card, "modulate:a", 0.0f, 0.2f);
                masterTween.Parallel().TweenProperty(card, "scale", Vector2.One * 0.8f, 0.25f);
            }
        }

        masterTween.TweenInterval(0.25f);

        masterTween.TweenProperty(selectedCard, "scale", Vector2.Zero, 0.2f)
            .SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.In);
        masterTween.Parallel().TweenProperty(selectedCard, "modulate:a", 0.0f, 0.15f);

        Node parentScene = GetParent();
        if (parentScene is CanvasItem canvasParent)
        {
            masterTween.Parallel().TweenProperty(canvasParent, "modulate:a", 0.0f, 0.25f);
        }

        masterTween.TweenCallback(Callable.From(() =>
        {
            EmitSignal(SignalName.CardChosen, index);
            parentScene.QueueFree(); 
        }));
    }

    private void KillCardTween(Control card)
    {
        if (_activeTweens.ContainsKey(card) && _activeTweens[card] != null && _activeTweens[card].IsValid())
        {
            _activeTweens[card].Kill();
        }
    }
}