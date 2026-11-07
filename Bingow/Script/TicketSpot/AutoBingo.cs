using Godot;
using System;
using System.Reflection.Emit;

public partial class AutoBingo : Upgrade
{
    [Export] private AnimatedSprite2D autobingoAnim;

    public void HandleTicketCompleted(Ticket pTicket)
    {
        if (!isActive) return;
        autobingoAnim.Frame = 1;

        Tween tween = CreateTween();

        tween.TweenProperty(autobingoAnim, "global_position", autobingoAnim.GlobalPosition, 1);
        tween.TweenCallback(Callable.From(() => autobingoAnim.Frame = 0));


        TicketEventBus.PublishBingoCalled(pTicket);
    }

    public override void UpdateUpgradeStatus(int pCurrentLevel)
    {
        base.UpdateUpgradeStatus(pCurrentLevel);

        if (isActive)
        {
            autobingoAnim.Frame = 0;
            autobingoAnim.Visible = true;
        }
    }
}
