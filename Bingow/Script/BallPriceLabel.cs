using Godot;
using System;

public partial class BallPriceLabel : Label
{
	[Export] private MoneyController moneyControllerRef;


	public override void _Ready()
	{
        moneyControllerRef.OnUpdateBallPrice += UpdatePrice;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public void UpdatePrice(int pPrice)
	{
        Text = $"{pPrice} $";

    }

    public override void _ExitTree()
    {
        base._ExitTree();
        moneyControllerRef.OnUpdateBallPrice -= UpdatePrice;
    }

}
