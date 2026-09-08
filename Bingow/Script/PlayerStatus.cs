using Godot;
using System;

public partial class PlayerStatus : Resource
{
    private const int STARTIN_BINGO_BALLS_AMOUNT = 20;
    private const int BINGO_BALLS_INCREASE_AMOUNT  = 10;

    private const int BINGO_BALLS_START_PRICE  = 1;
    private const int BINGO_BALLS_INCREASE_PRICE  = 1;

    private int startMoneyAmount = 10000;
    public int playerCurrentMoney {  get; private set; }

    public int bingoBallsCurrentPrice = 1;

    private int currentBingoBallsAmount;
    public int CurrentBingoBallsAmount => currentBingoBallsAmount;

    public event Action ActualiseMoney ;


    public void DiscountMoney( int pPmount )
    {
        playerCurrentMoney -= pPmount;
        ActualiseMoney?.Invoke();
    }

    public void AddMoney( int pPmount)
    {
        playerCurrentMoney += pPmount;
        ActualiseMoney?.Invoke();

    }


    public void AddBalls()
    {
        currentBingoBallsAmount += BINGO_BALLS_INCREASE_AMOUNT;
    }

    public void IncreaseBallsPrice()
    {
        bingoBallsCurrentPrice += BINGO_BALLS_INCREASE_PRICE;
    }

    public void BuyBall()
    {
        DiscountMoney(bingoBallsCurrentPrice);
    }

    public void BuyTick()
    {

    }

    public void StartNewRun()
    {
        playerCurrentMoney = startMoneyAmount;
        currentBingoBallsAmount = STARTIN_BINGO_BALLS_AMOUNT;

        bingoBallsCurrentPrice = BINGO_BALLS_START_PRICE;
        ActualiseMoney?.Invoke();

    }
}
