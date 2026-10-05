using Godot;
using System.Collections.Generic;

public abstract partial class Upgrade : Node
{
    protected bool isActive = false;
    protected float currentReactionTime;
    protected Timer reactionTimeTimer;

    protected List<float> upgradesLevelsAmount = new List<float>();

    public virtual void UpdateUpgradeStatus(int pCurrentLevel)
    {
        isActive = pCurrentLevel > 0;

        if (isActive && upgradesLevelsAmount.Count >= pCurrentLevel)
        {
            currentReactionTime = upgradesLevelsAmount[pCurrentLevel - 1];

            if (reactionTimeTimer != null)
            {
                reactionTimeTimer.WaitTime = currentReactionTime;
            }
        }
    }
}