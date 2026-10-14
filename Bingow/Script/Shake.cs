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


    public override void _Ready()
    {
        base._Ready();

        mouseArea.MouseEntered += StartShake;
        mouseArea.MouseExited += StopShake;

        baseScale = nodeToShakeRef.Scale;

    }

    public override void _ExitTree()
    {
        base._ExitTree();

        mouseArea.MouseEntered -= StartShake;
        mouseArea.MouseExited -= StopShake;

    }

    private void StartShake()
    {
        shakeTween?.Kill();

        shakeTween = CreateTween();

        shakeTween.SetParallel(true);

        shakeTween.TweenProperty(nodeToShakeRef, "scale", baseScale * scaleUp, ShakeDuration);

        shakeTween.TweenProperty(nodeToShakeRef, "rotation_degrees", ShakeAngle, ShakeDuration);

        shakeTween.SetParallel(false);

        shakeTween.TweenProperty(nodeToShakeRef, "rotation_degrees", -ShakeAngle, ShakeDuration * 2);

        shakeTween.TweenProperty(nodeToShakeRef, "rotation_degrees", 0f, ShakeDuration);

        shakeTween.TweenProperty(nodeToShakeRef, "scale", baseScale, ShakeDuration);

    }

    private void StopShake()
    {
    }
}
