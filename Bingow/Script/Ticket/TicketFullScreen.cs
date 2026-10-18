using Godot;
using System;

public partial class TicketFullScreen : Control
{
    [Export] private GridContainer gridContainerRef;
    [Export] private Sprite2D ticketFullVisual;
    private RandomNumberGenerator rand = new RandomNumberGenerator();
    private Ticket currentTicketToShowRef;
    private Vector2 _baseVisualScale;
    private Tween _openCloseTween;

    public event Action<int, Ticket> OnTicketStamped;
    public event Action<Ticket> OnTicketUpdate;
    public event Action OnTicketClose;

    public Ticket CurrentTicket => currentTicketToShowRef;

    public override void _Ready()
    {
        TicketEventBus.OnTicketOpened += ShowTicket;
        TicketEventBus.OnTicketAutoStamped += UpdateVisual;
        if (ticketFullVisual != null)
        {
            _baseVisualScale = ticketFullVisual.Scale;
        }
        Visible = false;
    }

    public void ShowTicket(Ticket pTicket)
    {
        Visible = true;
        currentTicketToShowRef = pTicket;
        UpdateVisual();

        _openCloseTween?.Kill();
        _openCloseTween = CreateTween();
        _openCloseTween.SetParallel(true);

        if (ticketFullVisual != null)
        {
            ticketFullVisual.Scale = _baseVisualScale * 0.25f;
            ticketFullVisual.RotationDegrees = rand.RandfRange(-6f, 6f);
            _openCloseTween.TweenProperty(ticketFullVisual, "scale", _baseVisualScale, 0.24f)
                           .SetTrans(Tween.TransitionType.Back)
                           .SetEase(Tween.EaseType.Out);
            _openCloseTween.TweenProperty(ticketFullVisual, "rotation_degrees", 0f, 0.24f)
                           .SetTrans(Tween.TransitionType.Back)
                           .SetEase(Tween.EaseType.Out);
        }

        if (gridContainerRef != null)
        {
            gridContainerRef.Modulate = new Color(1, 1, 1, 0);
            _openCloseTween.TweenProperty(gridContainerRef, "modulate:a", 1.0f, 0.18f);
        }
    }

    private void UpdateVisual()
    {
        if (currentTicketToShowRef is null) return;
        ticketFullVisual.Texture = currentTicketToShowRef.ticketFullVisual;
        UpdateGrid();
        OnTicketUpdate?.Invoke(currentTicketToShowRef);
    }

    private void UpdateGrid()
    {
        var lTicketNumbers = currentTicketToShowRef.GetTicketNumbers();

        gridContainerRef.Columns = currentTicketToShowRef.Column;
        gridContainerRef.Position = currentTicketToShowRef.gridDisplayPosition;
        gridContainerRef.Size = currentTicketToShowRef.gridDisplaySize;

        if (gridContainerRef.GetChildCount() == lTicketNumbers.Count)
        {
            for (int i = 0; i < lTicketNumbers.Count; i++)
            {
                int lCellNumber = lTicketNumbers[i];
                if (gridContainerRef.GetChild(i) is BingoCell lCell)
                {
                    lCell.UpdateCellState(currentTicketToShowRef.IsNumberStamped(lCellNumber));
                }
            }
            return;
        }

        ClearGrid();

        for (int i = 0; i < lTicketNumbers.Count; i++)
        {
            int lCellNumber = lTicketNumbers[i];
            int lTicketPosX = i % currentTicketToShowRef.Column;
            int lTicketPosY = i / currentTicketToShowRef.Column;

            BingoCell lCell = (BingoCell)currentTicketToShowRef.CellScene.Instantiate();
            gridContainerRef.AddChild(lCell);

            lCell.Initialize(lCellNumber, new Vector2I(lTicketPosX, lTicketPosY), currentTicketToShowRef.IsNumberStamped(lCellNumber), currentTicketToShowRef.cellScale);
            lCell.OnCellStamped += NumbersStamped;
        }
    }

    private void _on_button_button_down() => CloseTicket();
    private void _on_button_pressed() => CloseTicket();

    private void NumbersStamped(int pCellNumbers)
    {
        OnTicketStamped?.Invoke(pCellNumbers, currentTicketToShowRef);
        UpdateVisual();
    }

    public void CloseTicket()
    {
        if (!Visible) return;
        OnTicketClose?.Invoke();

        if (_openCloseTween.IsRunning() )  return;

        _openCloseTween?.Kill();
        _openCloseTween = CreateTween();
        _openCloseTween.SetParallel(true);

        if (ticketFullVisual != null)
        {
            _openCloseTween.TweenProperty(ticketFullVisual, "scale", _baseVisualScale * 0.2f, 0.14f)
                           .SetTrans(Tween.TransitionType.Back)
                           .SetEase(Tween.EaseType.In);
        }
        if (gridContainerRef != null)
        {
            _openCloseTween.TweenProperty(gridContainerRef, "modulate:a", 0.0f, 0.1f);
        }

        _openCloseTween.SetParallel(false);
        _openCloseTween.TweenCallback(Callable.From(() =>
        {
            ClearGrid();
            Visible = false;
            if (ticketFullVisual != null) ticketFullVisual.Scale = _baseVisualScale;
        }));
    }

    private void ClearGrid()
    {
        foreach (Node child in gridContainerRef.GetChildren())
        {
            if (child is BingoCell lCell)
            {
                lCell.OnCellStamped -= NumbersStamped;
            }
            child.QueueFree();
        }
    }

    public BingoCell GetCellByNumber(int number)
    {
        if (gridContainerRef == null) return null;
        foreach (Node child in gridContainerRef.GetChildren())
        {
            if (child is BingoCell cell && cell.CellNumber == number)
            {
                return cell;
            }
        }
        return null;
    }

    public override void _ExitTree()
    {
        TicketEventBus.OnTicketOpened -= ShowTicket;
        TicketEventBus.OnTicketAutoStamped -= UpdateVisual;
    }
}