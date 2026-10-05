using Godot;
using System;

public partial class BlackMarket : Control
{
    [Export] private MarketInventory inventoryRef;

    public override void _Ready()
    {
        //ShowMarket();
    }
    
    public void ShowMarket()
    {
        Visible = true;
        inventoryRef.Initialize(3, 5, () => new CrossBall() , 20);
    }

    public void CloseMarket()
    {
        Visible = false;
    }


}
