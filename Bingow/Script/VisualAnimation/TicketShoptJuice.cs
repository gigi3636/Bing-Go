using Godot;
using System;

public partial class TicketShoptJuice : Node
{
    public void OnButtonClicked(TextureButton bouton)
    {
        Tween tween = GetTree().CreateTween();

        tween.SetTrans(Tween.TransitionType.Elastic);
        tween.SetEase(Tween.EaseType.Out);

        tween.TweenProperty(bouton, "scale", new Vector2(1.2f, 1.2f), 0.5f);

        tween.TweenProperty(bouton, "scale", new Vector2(1.0f, 1.0f), 0.5f);
    }
}
