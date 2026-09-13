using Godot;
using System.Threading.Tasks;

public partial class AutoTicketPrinting : Upgrade
{
    [Export] private SelectorManager selectorManagerRef;
    [Export] private PlayerStatus playerStatusRes;
    [Export] private Control printerScreen;
    private TicketData ticketToPrint;

    public int sizeAvailable { get; private set; }

    private bool isPrinting;
    private int currentSessionId = 0;

    private int currentSpeedLevel = 0;

    [Export] private float timeReductionPerLevel = 0.3f;
    private const float MIN_PRINT_TIME = 0.1f;

    public void Initialize()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        printerScreen.Visible = isActive;

    }

    private void _on_print_button_pressed()
    {
        ticketToPrint = selectorManagerRef.GetCurrentTicket();

        if (ticketToPrint != null && !isPrinting)
        {
            StartPrinting();
        }
    }

    private async void StartPrinting()
    {
        if (isActive && !isPrinting)
        {
            isPrinting = true;
            currentSessionId++;
            await PrintLoop(currentSessionId);
        }
    }

    private async Task PrintLoop(int sessionId)
    {
        while (isPrinting && isActive && sessionId == currentSessionId)
        {
            TicketData currentTicket = ticketToPrint;

            if (currentTicket == null)
            {
                isPrinting = false;
                break;
            }

            bool printSuccess = await TryBuyAndPrintTicket(sessionId, currentTicket);

            if (!isPrinting || !isActive || sessionId != currentSessionId)
            {
                break;
            }

            if (!printSuccess)
            {
                await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
            }
        }
    }

    private async Task<bool> TryBuyAndPrintTicket(int sessionId, TicketData currentTicket)
    {
        if (currentTicket.Cost <= playerStatusRes.playerCurrentMoney)
        {
            playerStatusRes.DiscountMoney(currentTicket.Cost);

            float actualPrintTime = GetActualPrintTime(currentTicket);
            await ToSignal(GetTree().CreateTimer(actualPrintTime), SceneTreeTimer.SignalName.Timeout);

            if (isPrinting && isActive && sessionId == currentSessionId)
            {
                TicketEventBus.PublishTicketRequested(currentTicket, true);
            }

            return true;
        }

        return false;
    }

    private float GetActualPrintTime(TicketData ticket)
    {
        float calculatedTime = ticket.TimeToPrint - (currentSpeedLevel * timeReductionPerLevel);

        return Mathf.Max(MIN_PRINT_TIME, calculatedTime);
    }

    public void UpdateUpgradeStatus(bool pIsUnlock, int pPrintingSpeedLevel, int pPrintingSizeLevel)
    {
        isActive = pIsUnlock;
        currentSpeedLevel = pPrintingSpeedLevel;
        sizeAvailable = pPrintingSizeLevel;

        UpdateVisual();

    }
}