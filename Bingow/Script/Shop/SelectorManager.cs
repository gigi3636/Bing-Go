using Godot;
using System;

public partial class SelectorManager : Control
{
    [Export] private ScrollContainer scrollContainerRef;
    [Export] private HBoxContainer hBoxContainerRef;
    [Export] private TextureButton previousButton;
    [Export] private TextureButton nextButton;

    [Export] private int itemWidth = 200; 
    [Export] private int spacing = 10;    

    private int targetScroll = 0;
    private Tween currentTween;

    public override void _Ready()
    {
        previousButton.Pressed += OnLeftButtonPressed;
        nextButton.Pressed += OnRightButtonPressed;
    }

    private void OnLeftButtonPressed()
    {
        ScrollToTarget(-1);
    }

    private void OnRightButtonPressed()
    {
        ScrollToTarget(1);
    }

    private void ScrollToTarget(int direction)
    {
        int stepSize = itemWidth + spacing;
        targetScroll += stepSize * direction;

        int maxScroll = Mathf.Max(0, (int)(hBoxContainerRef.Size.X - scrollContainerRef.Size.X));

        targetScroll = Mathf.Clamp(targetScroll, 0, maxScroll);

        currentTween?.Kill();

        currentTween = CreateTween();

        currentTween.TweenProperty(scrollContainerRef, "scroll_horizontal", targetScroll, 0.3f)
                     .SetTrans(Tween.TransitionType.Quad)
                     .SetEase(Tween.EaseType.Out);
    }

    public int GetCurrentTicketIndex()
    {
        int stepSize = itemWidth + spacing;
        return targetScroll / stepSize;
    }

    public TicketData GetCurrentTicket()
    {
        int index = GetCurrentTicketIndex();

        if (index >= 0 && index < hBoxContainerRef.GetChildCount())
        {
            return hBoxContainerRef.GetChild<PrinterTicketVisual>(index).ticketRef;
        }

        return null;
    }
}