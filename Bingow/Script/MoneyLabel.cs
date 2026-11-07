using Godot;
using System;
using System.Globalization;

public partial class MoneyLabel : Label
{
    [Export] private PlayerStatus playerStatusRes;

    private double previousMoney = -1;
    private Tween moneyTween;

    public override void _Ready()
    {
        PivotOffset = Size / 2f;
        playerStatusRes.ActualiseMoney += ActualiseMoney;
        previousMoney = playerStatusRes.playerCurrentMoney;
        ActualiseMoney();
    }

    public void ActualiseMoney()
    {
        double current = playerStatusRes.playerCurrentMoney;
        Text = FormatMoney(current);
        PivotOffset = Size / 2f;

        if (previousMoney != -1 && !Mathf.IsEqualApprox(current, previousMoney))
        {
            moneyTween?.Kill();
            moneyTween = CreateTween();

            if (current > previousMoney)
            {
                Scale = new Vector2(1.45f, 1.45f);
                Modulate = new Color(0.4f, 2.0f, 0.5f);
                moneyTween.SetParallel(true);
                moneyTween.TweenProperty(this, "scale", Vector2.One, 0.35f)
                          .SetTrans(Tween.TransitionType.Elastic)
                          .SetEase(Tween.EaseType.Out);
                moneyTween.TweenProperty(this, "modulate", Colors.White, 0.3f);
            }
            else
            {
                Scale = new Vector2(0.88f, 1.12f);
                Modulate = new Color(2.0f, 0.35f, 0.35f);
                moneyTween.SetParallel(true);
                moneyTween.TweenProperty(this, "scale", Vector2.One, 0.22f)
                          .SetTrans(Tween.TransitionType.Back)
                          .SetEase(Tween.EaseType.Out);
                moneyTween.TweenProperty(this, "modulate", Colors.White, 0.22f);
            }
        }

        previousMoney = current;
    }

    private string FormatMoney(double amount)
    {
        double absAmount = Math.Abs(amount);
        string sign = amount < 0 ? "-" : "";

        // Moins de 6 chiffres (< 100 000) : affichage normal (max 5 chiffres)
        if (absAmount < 100_000)
        {
            return ((long)amount).ToString();
        }

        // Au-dela des Trillions (>= 10^15) : Notation puissance de 10
        if (absAmount >= 1e15)
        {
            int exponent = (int)Math.Floor(Math.Log10(absAmount));
            double mantissa = absAmount / Math.Pow(10, exponent);
            // Exemple : "1.2e15" ou "1e15"
            return $"{sign}{mantissa.ToString("0.#", CultureInfo.InvariantCulture)}e{exponent}";
        }

        // suffixe and pallier
        double divisor;
        string suffix;

        if (absAmount >= 1e12) { divisor = 1e12; suffix = "T"; }
        else if (absAmount >= 1e9) { divisor = 1e9; suffix = "B"; }
        else if (absAmount >= 1e6) { divisor = 1e6; suffix = "M"; }
        else { divisor = 1e3; suffix = "K"; }

        double shortValue = absAmount / divisor;

        string formattedNumber;
        if (shortValue >= 100)
        {
            formattedNumber = Math.Floor(shortValue).ToString("0", CultureInfo.InvariantCulture);
        }
        else if (shortValue >= 10)
        {
            formattedNumber = (Math.Floor(shortValue * 10) / 10).ToString("0.#", CultureInfo.InvariantCulture);
        }
        else
        {
            formattedNumber = (Math.Floor(shortValue * 100) / 100).ToString("0.##", CultureInfo.InvariantCulture);
        }

        return $"{sign}{formattedNumber}{suffix}";
    }

    public override void _ExitTree()
    {
        playerStatusRes.ActualiseMoney -= ActualiseMoney;
    }
}