using Godot;
using System;
using System.Collections.Generic;

public partial class CrankManager : Upgrade
{
	public Action OnCrankActionned;

	private Timer crankTimer;

	public override void _Ready()
	{
        upgradesLevelsAmount = new List<float> { 4, 3, 2.5f, 2, 1, 0.5f, 0.1f };
		reactionTimeTimer = new Timer();
		reactionTimeTimer.OneShot = true;
        AddChild(reactionTimeTimer);    
    }

    public void _on_crank_button_pressed()
    {
        if ( reactionTimeTimer.TimeLeft == 0 )
		{
            OnCrankActionned?.Invoke();
			reactionTimeTimer.Start();

        }
    }

}
