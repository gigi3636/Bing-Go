using Godot;
using System;

public partial class LockedItem : Control
{
    [Export] private Label priceLabel;
    [Export] private AnimatedSprite2D lockedSpriteAnim;

    [Export] private GpuParticles2D unlockParticles; 

    private TicketSpot ticketSpotRef;

    public void Initialize(TicketSpot pTicketSpot)
    {
        ticketSpotRef = pTicketSpot;
        priceLabel.Text = pTicketSpot.unlockCost.ToString() + " $";

        PivotOffset = Size / 2;
    }

    private void _on_unlock_button_pressed()
    {
        ticketSpotRef.OnPurchaseRequested?.Invoke(ticketSpotRef.unlockCost, ticketSpotRef.UnlockTicket);
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (ticketSpotRef.isUnlocked)
        {
            PlayJuicyUnlockAnimation();
        }
    }

    private void PlayJuicyUnlockAnimation()
    {
        GetNode<Button>("UnlockButton").Disabled = true;

        lockedSpriteAnim.Play();

        if (unlockParticles != null)
        {
            unlockParticles.Emitting = true;
        }

        Tween tween = CreateTween();

        Vector2 originalPos = lockedSpriteAnim.Position;
        Vector2 originalScale = lockedSpriteAnim.Scale;

        tween.TweenProperty(lockedSpriteAnim, "scale", originalScale * 1.3f, 0.15f)
             .SetTrans(Tween.TransitionType.Back)
             .SetEase(Tween.EaseType.Out);

        tween.TweenProperty(lockedSpriteAnim, "position", originalPos + new Vector2(5, 0), 0.05f);
        tween.TweenProperty(lockedSpriteAnim, "position", originalPos + new Vector2(-5, 0), 0.05f);
        tween.TweenProperty(lockedSpriteAnim, "position", originalPos + new Vector2(3, 0), 0.05f);
        tween.TweenProperty(lockedSpriteAnim, "position", originalPos, 0.05f);

        tween.TweenInterval(0.3f);

        tween.TweenProperty(this, "scale", Vector2.Zero, 0.3f)
             .SetTrans(Tween.TransitionType.Back)
             .SetEase(Tween.EaseType.In);

        tween.TweenCallback(Callable.From(() =>
        {
            Visible = false;
            QueueFree();
        }));
    }
}