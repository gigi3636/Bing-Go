using Godot;
using System;

public partial class BingoManager : Node
{
    [Export] private TextureButton bingoButtonRef;
    [Export] private TicketFullScreen ticketFullScreenRef;
    [Export] private PlayerStatus playerStatusRef;

    private float timeBetweenBall = 0.4f;
    private Ticket currentTicketRef;

    public override void _Ready()
    {
        ticketFullScreenRef.OnTicketUpdate += UpdateBingoButton;
        TicketEventBus.OnBingoCalled += HandleBingoCall;
    }

    public override void _ExitTree()
    {
        ticketFullScreenRef.OnTicketUpdate -= UpdateBingoButton;
        TicketEventBus.OnBingoCalled -= HandleBingoCall;

        base._ExitTree();
    }

    public void UpdateBingoButton(Ticket pTicketRef)
    {
        currentTicketRef = pTicketRef;
        bingoButtonRef.Visible = pTicketRef.isCompleted;
    }


    private void _on_bingo_text_pressed()
    {
        HandleBingoCall(currentTicketRef);
    }

    public async void HandleBingoCall(Ticket pTicket)
    {
        // unable to bingo more than 1 time
        bingoButtonRef.Disabled = true;

        BingoRewardData lReward = new BingoRewardData { BaseMoney = 0, Multiplier = 1.0f };

        // Split the value of the ticket by each case
        float lValuePerCell = pTicket.Value / (float)pTicket.winningLineNumbers.Count;

        // look if the bingo call is legit
        bool isLigneValide = true;

        foreach (int number in pTicket.winningLineNumbers)
        {
            // if one number is not valid unable the price
            if (!pTicket.IsNumberValidAndStamped(number))
            {
                isLigneValide = false;
                break; // one number is enough
            }
        }

        if (!isLigneValide)
        {
            GD.Print("ticket pas valide");

            pTicket.DeleteTicket();
            ticketFullScreenRef.CloseTicket();
            bingoButtonRef.Disabled = false;

            return; 
        }


        // case by case effect 
        foreach (int lCurrentNumber in pTicket.winningLineNumbers)
        {
            lReward.BaseMoney += lValuePerCell;

            // save the effect of this number
            IBallEffect lEffect = pTicket.GetEffectOfNumber(lCurrentNumber);

            if (lEffect != null && !(lEffect is BasicBall))
            {
                // Apply the effect
                lEffect.ApplyOnValidation(lReward);

                GD.Print($"Effet applique sur le numero {lCurrentNumber} !");
            }
            else
            {
                // no effect
                GD.Print($"Numero {lCurrentNumber} classique : + {lValuePerCell}$");
            }

            // wait s before next number
            await ToSignal(GetTree().CreateTimer(timeBetweenBall), SceneTreeTimer.SignalName.Timeout);
        }

        // end of effect
        int lFinalMoney = lReward.CalculateTotal();

        // recap of total money
        playerStatusRef.AddMoney(lFinalMoney);
        GD.Print($"BINGO TERMINE : Total gagne = {lFinalMoney}$");

        pTicket.DeleteTicket();
        ticketFullScreenRef.CloseTicket();

        bingoButtonRef.Disabled = false;
    }
}

public class BingoRewardData
{
    public float BaseMoney { get; set; }
    public float Multiplier { get; set; } = 1.0f;

    public int CalculateTotal()
    {
        return (int)(BaseMoney * Multiplier);
    }
}
