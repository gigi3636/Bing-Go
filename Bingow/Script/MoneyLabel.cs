using Godot;
using System;

public partial class MoneyLabel : Label
{
    [Export] private PlayerStatus playerStatusRes;

    public override void _Ready()
    {
        playerStatusRes.ActualiseMoney += ActualiseMoney;
        ActualiseMoney();
    }

    public void ActualiseMoney()
    {
        Text = playerStatusRes.playerCurrentMoney.ToString();
    }

    public override void _ExitTree()
    {
        playerStatusRes.ActualiseMoney -= ActualiseMoney;
    }
}
