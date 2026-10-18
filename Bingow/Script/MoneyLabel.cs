using Godot;
using System;

public partial class MoneyLabel : Label
{
    [Export] private PlayerStatus playerStatusRes;
    private int _previousMoney = -1;
    private Tween _moneyTween;

    public override void _Ready()
    {
        PivotOffset = Size / 2f;
        playerStatusRes.ActualiseMoney += ActualiseMoney;
        _previousMoney = playerStatusRes.playerCurrentMoney;
        ActualiseMoney();
    }

    public void ActualiseMoney()
    {
        int current = playerStatusRes.playerCurrentMoney;
        Text = current.ToString();
        PivotOffset = Size / 2f;

        if (_previousMoney != -1 && current != _previousMoney)
        {
            _moneyTween?.Kill();
            _moneyTween = CreateTween();

            if (current > _previousMoney)
            {
                Scale = new Vector2(1.45f, 1.45f);
                Modulate = new Color(0.4f, 2.0f, 0.5f);
                _moneyTween.SetParallel(true);
                _moneyTween.TweenProperty(this, "scale", Vector2.One, 0.35f)
                           .SetTrans(Tween.TransitionType.Elastic)
                           .SetEase(Tween.EaseType.Out);
                _moneyTween.TweenProperty(this, "modulate", Colors.White, 0.3f);
            }
            else
            {
                Scale = new Vector2(0.88f, 1.12f);
                Modulate = new Color(2.0f, 0.35f, 0.35f);
                _moneyTween.SetParallel(true);
                _moneyTween.TweenProperty(this, "scale", Vector2.One, 0.22f)
                           .SetTrans(Tween.TransitionType.Back)
                           .SetEase(Tween.EaseType.Out);
                _moneyTween.TweenProperty(this, "modulate", Colors.White, 0.22f);
            }
        }

        _previousMoney = current;
    }

    public override void _ExitTree()
    {
        playerStatusRes.ActualiseMoney -= ActualiseMoney;
    }
}