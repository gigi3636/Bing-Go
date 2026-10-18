using Godot;

public partial class Shake : Node
{
    [Export] private Control mouseArea;
    [Export] private Control nodeToShakeRef;

    [Export] private float ShakeAngle = 5.0f;
    [Export] private float ShakeDuration = 0.05f;
    [Export] private float scaleUp = 1.2f;

    private Tween shakeTween;
    private Vector2 baseScale;
    private float baseRoation;

    public override void _Ready()
    {
        base._Ready();

        if (nodeToShakeRef != null)
        {
            baseScale = nodeToShakeRef.Scale;
            baseRoation = nodeToShakeRef.RotationDegrees;
        }


        if (mouseArea != null)
        {
            mouseArea.MouseEntered += StartShake;
            mouseArea.MouseExited += StopShake;
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (mouseArea != null)
        {
            mouseArea.MouseEntered -= StartShake;
            mouseArea.MouseExited -= StopShake;
        }
    }

    public void StartShake()
    {
        shakeTween?.Kill();
        shakeTween = CreateTween();
        shakeTween.SetParallel(true);

        shakeTween.TweenProperty(nodeToShakeRef, "scale", baseScale * scaleUp, ShakeDuration);
        shakeTween.TweenProperty(nodeToShakeRef, "rotation_degrees", baseRoation + ShakeAngle, ShakeDuration);

        shakeTween.SetParallel(false);
        shakeTween.TweenProperty(nodeToShakeRef, "rotation_degrees", baseRoation - ShakeAngle, ShakeDuration * 2);
        shakeTween.TweenProperty(nodeToShakeRef, "rotation_degrees", baseRoation, ShakeDuration);
        shakeTween.TweenProperty(nodeToShakeRef, "scale", baseScale, ShakeDuration);
    }

    public void StopShake()
    {
        shakeTween?.Kill();
        Tween resetTween = CreateTween();
        resetTween.SetParallel(true);
        resetTween.TweenProperty(nodeToShakeRef, "scale", baseScale, 0.1f);
        resetTween.TweenProperty(nodeToShakeRef, "rotation_degrees", baseRoation, 0.1f);
    }
}