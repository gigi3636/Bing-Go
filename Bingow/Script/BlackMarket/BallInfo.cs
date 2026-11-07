using Godot;
using System;

public partial class BallInfo : Node
{
    [Export] private TextureRect ballTxRectRef;
    [Export] private Label ballName;
    [Export] private Label ballDesc;

    private Tween infoTween;
    private float floatTimer = 0f;
    private Vector2 baseBallPos;
    private bool isInitialized = false;

    public override void _Ready()
    {
        Callable.From(() =>
        {
            if (ballTxRectRef != null)
            {
                ballTxRectRef.PivotOffset = ballTxRectRef.Size / 2f;
                baseBallPos = ballTxRectRef.Position;
                isInitialized = true;
            }
            if (ballName != null)
            {
                ballName.PivotOffset = ballName.Size / 2f;
            }
        }).CallDeferred();
    }

    public override void _Process(double delta)
    {
        if (!isInitialized || ballTxRectRef == null) return;

        floatTimer += (float)delta * 2.5f;
        ballTxRectRef.Position = baseBallPos + new Vector2(0f, Mathf.Sin(floatTimer) * 6f);
    }

    public void ShowBall(Texture2D pBallTexture, string pBallDesc, string pBallName)
    {
        ballTxRectRef.Texture = pBallTexture;
        ballName.Text = pBallName;
        ballDesc.Text = pBallDesc;

        ballTxRectRef.PivotOffset = ballTxRectRef.Size / 2f;
        ballName.PivotOffset = ballName.Size / 2f;

        infoTween?.Kill();
        infoTween = CreateTween();

        ballTxRectRef.Scale = new Vector2(0.6f, 0.6f);
        ballTxRectRef.RotationDegrees = -12f;

        infoTween.SetParallel(true);
        infoTween.TweenProperty(ballTxRectRef, "scale", Vector2.One, 0.35f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
        infoTween.TweenProperty(ballTxRectRef, "rotation_degrees", 0f, 0.35f)
            .SetTrans(Tween.TransitionType.Elastic)
            .SetEase(Tween.EaseType.Out);

        ballName.Scale = new Vector2(1.15f, 1.15f);
        infoTween.TweenProperty(ballName, "scale", Vector2.One, 0.25f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);

        ballDesc.VisibleRatio = 0f;
        infoTween.TweenProperty(ballDesc, "visible_ratio", 1f, 0.25f)
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.Out);
    }
}