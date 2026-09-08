using Godot;

// Script in  each shop button
public partial class TicketShoptButton : TextureButton
{
    [Export] private TicketData ticketRequestedData;
    [Export] private PlayerStatus playerStatusRes;
    [Export] private TicketShoptJuice ticketShoptJuiceRef;

    private void _on_pressed()
    {
        GD.Print("buy");

        // Check if the purchase can be done 
        if (ticketRequestedData.Cost <= playerStatusRes.playerCurrentMoney)
        {
            playerStatusRes.DiscountMoney(ticketRequestedData.Cost);

            // Request to create a new ticket with a specific data
            TicketEventBus.PublishTicketRequested(ticketRequestedData);

            ticketShoptJuiceRef.OnButtonClicked(this);

        }
    }
}
