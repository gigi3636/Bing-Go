using Godot;
using System.Collections.Generic;

public partial class BallVisualDisplayer : Node2D
{
    [Export] private Marker2D startSpawnMarker; 
    [Export] private Marker2D tubeEntryMarker;  
    [Export] private Marker2D endBallMarker;   

    [Export] private NinePatchRect tubeBack;    
    [Export] private NinePatchRect tubeFront;   

    [Export] private float ballSpacing = 85f;   
    [Export] private float baseTubeWidth = 64f;

    public void UpdateTubeSize(int capacity)
    {
        float totalVisualWidth = baseTubeWidth + ((capacity - 0.5f )* ballSpacing);

        float newInternalWidth = totalVisualWidth / tubeBack.Scale.X;

        float rightEdgeX = tubeBack.Position.X + (tubeBack.Size.X * tubeBack.Scale.X);

        tubeBack.Size = new Vector2(newInternalWidth, tubeBack.Size.Y);

        tubeBack.Position = new Vector2(rightEdgeX - totalVisualWidth, tubeBack.Position.Y);

        if (tubeFront != null)
        {
            tubeFront.Size = tubeBack.Size;
            tubeFront.Position = tubeBack.Position;
        }

    }

    public void AddAndShiftBalls(BingoBall newBall, Queue<BingoBall> currentBalls)
    {
        AddChild(newBall);

        newBall.GlobalPosition = startSpawnMarker.GlobalPosition;

        BingoBall[] ballsArray = currentBalls.ToArray();

        for (int i = 0; i < ballsArray.Length; i++)
        {
            BingoBall ball = ballsArray[i];

            int slotFromRight = (ballsArray.Length - 1) - i;
            Vector2 targetPos = tubeEntryMarker.GlobalPosition + new Vector2(-slotFromRight * ballSpacing, 0);

            Tween tween = CreateTween();
            tween.TweenProperty(ball, "global_position", targetPos, 0.25f)
                 .SetTrans(Tween.TransitionType.Sine)
                 .SetEase(Tween.EaseType.Out);

            ball.PlayRollShiftAnimation();
        }
    }

    public void EjectBall(BingoBall oldBall)
    {
        Tween tween = CreateTween();

        tween.TweenProperty(oldBall, "global_position", endBallMarker.GlobalPosition, 0.2f)
             .SetTrans(Tween.TransitionType.Linear);
        tween.TweenCallback(Callable.From(() => oldBall.ZIndex = -9));

        Vector2 fallPosition = endBallMarker.GlobalPosition + new Vector2(0, 150f);
        tween.TweenProperty(oldBall, "global_position", fallPosition, 0.35f)
             .SetTrans(Tween.TransitionType.Quad)
             .SetEase(Tween.EaseType.In);

        tween.Parallel().TweenProperty(oldBall, "modulate:a", 0f, 0.35f);

        tween.TweenCallback(Callable.From(oldBall.QueueFree));
    }
}