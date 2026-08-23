using Godot;
using System;

public partial class BingoManager : Node
{
    [Export] private TextureButton bingoButtonRef;
    [Export] private TicketFullScreen ticketFullScreenRef;

    public override void _Ready()
    {
        ticketFullScreenRef.OnTicketUpdate += UpdateBingoButton;

    }

    public void UpdateBingoButton( Ticket pTicketRef )
    {

        bingoButtonRef.Visible = pTicketRef.CheckForCompletedLines();

    }

    public void HandleBingoCall()
    {
        GD.Print("BIIIIINGOOOOO");


    }
}
