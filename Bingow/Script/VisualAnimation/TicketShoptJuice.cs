using Godot;
using System;

public partial class TicketShoptJuice : Node
{
    public void OnButtonClicked(TextureButton bouton)
    {
        bouton.PivotOffset = bouton.Size / 2f;

        Tween tween = GetTree().CreateTween();

        tween.SetTrans(Tween.TransitionType.Expo);
        tween.SetEase(Tween.EaseType.Out);

        tween.TweenProperty(bouton, "scale", new Vector2(1.15f, 1.15f), 0.1f);

        tween.SetTrans(Tween.TransitionType.Back);
        tween.SetEase(Tween.EaseType.Out);

        tween.TweenProperty(bouton, "scale", new Vector2(1.0f, 1.0f), 0.15f);
    }
}