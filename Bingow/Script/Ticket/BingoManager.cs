using Godot;
using System;

public partial class BingoManager : Node
{
    [Export] private TextureButton bingoButtonRef;
    [Export] private TicketFullScreen ticketFullScreenRef;
    [Export] private PlayerStatus playerStatusRef;

    private Ticket currentTicketRef;

    public override void _Ready()
    {
        ticketFullScreenRef.OnTicketUpdate += UpdateBingoButton;
        TicketEventBus.OnBingoCalled += HandleBingoCall;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public void UpdateBingoButton( Ticket pTicketRef )
    {
        currentTicketRef = pTicketRef;
        bingoButtonRef.Visible = pTicketRef.CheckForCompletedLines(pTicketRef.IsNumberStamped);

    }

    private void _on_bingo_text_pressed()
    {
        HandleBingoCall(currentTicketRef);
    }

    public void HandleBingoCall(Ticket pTicket)
    {
        GD.Print("BIIIIINGOOOOO");

        GD.Print(pTicket.CheckForCompletedLines(pTicket.IsNumberValidAndStamped));

        playerStatusRef.AddMoney(pTicket.Value);
        pTicket.DeleteTicket();
        ticketFullScreenRef.CloseTicket();

    }
}
