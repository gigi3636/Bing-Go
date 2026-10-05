using Godot;
using System;

public partial class BallBag : Control
{
    public int bagPrice { get; private set; }
    public BagContaining bagContaining {  get; private set; }

    public void Initialize(int pBagPrice , int pBallAmount, Func<IBallEffect> createBallFunc, int pPlayerNumberBalls )
    {
        bagPrice = pBagPrice;
        bagContaining =  new BagContaining(pBallAmount, createBallFunc, pPlayerNumberBalls);

    }

    private void _on_buy_button_pressed()
    {


        //if (bagPrice <= playerStatusRes.playerCurrentMoney)
        //{

        //    foreach (var item in bagContaining.bagBalls)
        //    {
        //        GD.Print(item);
        //    }

        //}

    }
}
