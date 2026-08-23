using Godot;
using System;

public partial class PlayerStatus : Resource
{
    private int startMoneyAmount = 100;
    public int playerCurrentMoney {  get; private set; }

    public event Action ActualiseMoney ;


    public void DiscountMoney( int pPmount )
    {
        playerCurrentMoney -= pPmount;
        ActualiseMoney?.Invoke();
    }

    public void StartNewRun()
    {
        playerCurrentMoney = startMoneyAmount;

        ActualiseMoney?.Invoke();

    }
}
