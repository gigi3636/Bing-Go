using Godot;
using System;

public partial class BlackMarrketBallVisuel : TextureRect
{
    public Action<BallRes> OnBallClicked;
    private BallRes ballRes;
    private Tween activeTween;

    public void Initialize(BallRes pBallRes)
    {
        Texture = pBallRes.texture;
        ballRes = pBallRes;

        ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        TextureFilter = CanvasItem.TextureFilterEnum.Nearest;

        if (!ballRes.isUnlocked)
            Modulate = Color.Color8(15, 15, 15, 255);

        Callable.From(SetupJuiciness).CallDeferred();
    }

    private void SetupJuiciness()
    {
        PivotOffset = Size / 2f;

        Control targetControl = GetChildCount() > 0 && GetChild(0) is Control btn ? btn : this;
        targetControl.MouseEntered += OnMouseEntered;
        targetControl.MouseExited += OnMouseExited;
    }

    public async void DemarrerApparition(float delai)
    {
        Scale = Vector2.Zero;

        if (!IsInsideTree())
        {
            await ToSignal(this, Node.SignalName.TreeEntered);
        }

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        PivotOffset = Size / 2f;

        Tween tween = CreateTween();
        tween.TweenInterval(0.1f + delai); 
        tween.TweenProperty(this, "scale", Vector2.One, 0.4f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
    }
    private void OnMouseEntered()
    {
        activeTween?.Kill();
        activeTween = CreateTween().SetParallel(true);

        if (ballRes != null && !ballRes.isUnlocked)
        {
            activeTween.TweenProperty(this, "scale", new Vector2(0.95f, 0.95f), 0.15f);
            return;
        }

        activeTween.TweenProperty(this, "scale", new Vector2(1.18f, 1.18f), 0.2f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
        activeTween.TweenProperty(this, "rotation_degrees", 6f, 0.2f)
            .SetTrans(Tween.TransitionType.Sine);
    }

    private void OnMouseExited()
    {
        activeTween?.Kill();
        activeTween = CreateTween().SetParallel(true);
        activeTween.TweenProperty(this, "scale", Vector2.One, 0.2f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
        activeTween.TweenProperty(this, "rotation_degrees", 0f, 0.2f)
            .SetTrans(Tween.TransitionType.Sine);
    }

    private void _on_button_pressed()
    {
        PivotOffset = Size / 2f;
        activeTween?.Kill();

        if (ballRes != null && !ballRes.isUnlocked)
        {
            activeTween = CreateTween();
            activeTween.TweenProperty(this, "rotation_degrees", -12f, 0.05f);
            activeTween.TweenProperty(this, "rotation_degrees", 12f, 0.05f);
            activeTween.TweenProperty(this, "rotation_degrees", -6f, 0.05f);
            activeTween.TweenProperty(this, "rotation_degrees", 0f, 0.05f);
            return;
        }

        Scale = new Vector2(0.75f, 0.75f);
        activeTween = CreateTween();
        activeTween.TweenProperty(this, "scale", new Vector2(1.18f, 1.18f), 0.25f)
            .SetTrans(Tween.TransitionType.Elastic)
            .SetEase(Tween.EaseType.Out);

        OnBallClicked?.Invoke(ballRes);
    }
}