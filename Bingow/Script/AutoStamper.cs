using Godot;
using System;
using System.Collections.Generic;

public partial class AutoStamper : Upgrade
{
    public Ticket ticketContainer { get; private set; }
    private bool isMoving;
    private int currentNumberStamping;
    
    private Func<int, bool> checkNumberMethod; // CHech if the number stamping is still present in the cage
    private Func<List<int>> getActiveNumbersMethod;

    private Func<int, IBallEffect> getBallEffectMethod; // return the effetc of the number stamped
    public void SetNewTicket(Ticket pNewTicket)
    {
        ticketContainer = pNewTicket;

    }


    public void Initialize(Func<int, bool> pCheckMethod, Func<List<int>> pGetListMethod, Func<int, IBallEffect> pGetEffectMethod)
    {
        upgradesLevelsAmount = new List<float> { 1f, 0.8f, 0.5f, 0.3f, 0.1f };
        reactionTimeTimer = new Timer();
        reactionTimeTimer.OneShot = true;


        checkNumberMethod = pCheckMethod;
        getActiveNumbersMethod = pGetListMethod;
        reactionTimeTimer.Timeout += AutoStampNumber;
        getBallEffectMethod = pGetEffectMethod; 

        AddChild(reactionTimeTimer);


    }


    // Check if the new ball is in the grid  
    public void VerifyGrid(int pNumberToVerify)
    {
        if (!isActive) return; // if level 0 so locked
        if (ticketContainer == null) return;
        if (isMoving) return;

        if (ticketContainer.GetTicketNumbers().Contains(pNumberToVerify))
        {
            if (!ticketContainer.IsNumberStamped(pNumberToVerify))
            {
                StartMoving(pNumberToVerify);
            }
        }
    }

    private void StartMoving(int pNumberToStamp)
    {
        currentNumberStamping = pNumberToStamp;
        isMoving = true;
        // STart animation of stamping

        reactionTimeTimer.Start();
    }


    private void AutoStampNumber()
    {
        if (checkNumberMethod(currentNumberStamping))
        {
            // get the effect of the number stamped
            IBallEffect lCurrentEffect = getBallEffectMethod(currentNumberStamping);

            // autoStamp
            ticketContainer.UpdateStampedNumber(currentNumberStamping, true, lCurrentEffect, true);

            TicketEventBus.UpdateAutoStampedTicket();
        }
        isMoving = false;
        VerifyBalls();
    }

    private void VerifyBalls()
    {
        foreach (int lBallNumber in getActiveNumbersMethod())
        {
            VerifyGrid(lBallNumber);
        }
    }

}
