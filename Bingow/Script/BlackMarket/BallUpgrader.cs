using Godot;
using System;
using System.Linq;

public partial class BallUpgrader : Node
{
    [Export] private RichTextLabel textLabelRef;
    [Export] private TextureButton buyButtonRef;

    public Action OnUpgradeBought;
    private Tween upgradeTween;

    public override void _Ready()
    {
        if (buyButtonRef == null)
        {
            buyButtonRef = GetNodeOrNull<TextureButton>("../TextureRect4/BuyButton");
        }

        if (buyButtonRef != null)
        {
            Callable.From(() => buyButtonRef.PivotOffset = buyButtonRef.Size / 2f).CallDeferred();
            buyButtonRef.MouseEntered += OnButtonHover;
            buyButtonRef.MouseExited += OnButtonExit;
        }
    }

    private void OnButtonHover()
    {
        Tween t = CreateTween();
        t.TweenProperty(buyButtonRef, "scale", new Vector2(1.08f, 1.08f), 0.15f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
    }

    private void OnButtonExit()
    {
        Tween t = CreateTween();
        t.TweenProperty(buyButtonRef, "scale", Vector2.One, 0.15f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
    }

    public void UpdateVisuel(BallRes ballData)
    {
        int lvl = ballData.ballLevel;
        int nextLvl = lvl + 1;

        int dropIndex = Math.Min(lvl, Math.Max(0, ballData.dropPercents.Length - 1));
        int nextDropIndex = Math.Min(nextLvl, Math.Max(0, ballData.dropPercents.Length - 1));

        int ballIndex = Math.Min(lvl, Math.Max(0, ballData.ballNumbers.Length - 1));
        int nextBallIndex = Math.Min(nextLvl, Math.Max(0, ballData.ballNumbers.Length - 1));

        textLabelRef.Text =
            $"[center]Level {lvl} => [wave amp=25 freq=6][color=green]Level {nextLvl}[/color][/wave]\n" +
            $"[font_size=8] \n[/font_size]" + 
            $"Drop : {ballData.dropPercents[dropIndex]}% => [color=green]{ballData.dropPercents[nextDropIndex]}%[/color]\n" +
            $"Balls : {ballData.ballNumbers[ballIndex]} => [color=green]{ballData.ballNumbers[nextBallIndex]}[/color][/center]";

        textLabelRef.PivotOffset = textLabelRef.Size / 2f;
        upgradeTween?.Kill();
        upgradeTween = CreateTween();
        textLabelRef.Scale = new Vector2(1.12f, 1.12f);
        upgradeTween.TweenProperty(textLabelRef, "scale", Vector2.One, 0.2f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
    }

    private void _on_buy_button_pressed()
    {
        if (buyButtonRef != null)
        {
            buyButtonRef.PivotOffset = buyButtonRef.Size / 2f;
            buyButtonRef.Scale = new Vector2(0.85f, 0.85f);

            Tween btnTween = CreateTween();
            btnTween.TweenProperty(buyButtonRef, "scale", new Vector2(1.08f, 1.08f), 0.3f)
                .SetTrans(Tween.TransitionType.Elastic)
                .SetEase(Tween.EaseType.Out);
        }

        if (textLabelRef != null)
        {
            textLabelRef.PivotOffset = textLabelRef.Size / 2f;
            textLabelRef.Scale = new Vector2(1.25f, 1.25f);

            Tween lblTween = CreateTween();
            lblTween.TweenProperty(textLabelRef, "scale", Vector2.One, 0.3f)
                .SetTrans(Tween.TransitionType.Elastic)
                .SetEase(Tween.EaseType.Out);
        }

        OnUpgradeBought?.Invoke();
    }
}