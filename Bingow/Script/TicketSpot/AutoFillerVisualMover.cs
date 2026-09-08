using Godot;

public partial class AutoFillerVisualMover : Node
{
    [Export] private Texture2D mouseTexture;

    public void AutoFillerMove(Ticket pTicketToMove, TicketSpot pTicketSpot, float pDuration)
    {
        Vector2 startPosition = pTicketToMove.GlobalPosition;

        Sprite2D mouseSprite = new Sprite2D();
        mouseSprite.ZIndex = 10;
        mouseSprite.Scale = new Vector2(0.3f,0.3f);
        mouseSprite.Texture = mouseTexture;

        AddChild(mouseSprite);
        mouseSprite.GlobalPosition = startPosition;

        Tween lTween = CreateTween();

        lTween.SetParallel(true);
        lTween.TweenProperty(pTicketToMove, "global_position", pTicketSpot.GlobalPosition, pDuration);
        lTween.TweenProperty(mouseSprite, "global_position", pTicketSpot.GlobalPosition, pDuration);

        lTween.SetParallel(false);

        lTween.TweenProperty(mouseSprite, "global_position", startPosition, 0.3f);
        lTween.TweenCallback(Callable.From(mouseSprite.QueueFree));

    }
}