using Godot;
using System.Collections.Generic;

public partial class BingoCage : Node
{
    [Export] private BallSpawner ballSpawnerRef;
    [Export] private BallListManager ballListManagerRef;
    [Export] private BallTimer ballTimerRef;
    [Export] private BallVisualDisplayer ballVisualDisplayerRef;

    private RandomNumberGenerator rand = new RandomNumberGenerator();

    public override void _Ready()
    {
        ballTimerRef.OnBallSpawnerTimeout += HandleNewBallRequest;
    }

    private void HandleNewBallRequest()
    {
        // Get all the numbers alredy present in the screen
        List<int> lBallNumbersUsed = ballListManagerRef.GetBallNumbers();

        int lNextNumbers = GetNextBallNumbers(lBallNumbersUsed);

        // Create new ball and take reference for adding it to the list
        BingoBall newBall = ballSpawnerRef.SpawnBall(lNextNumbers);

        ballListManagerRef.AddBall(newBall);

        ballVisualDisplayerRef.AddVisualElement(newBall);
    }

    // Return a non used numbers 
    private int GetNextBallNumbers(List<int> lBallNumbersUsed)
    {
        int lBallNumbers = rand.RandiRange(0, 99); ;


        while (lBallNumbersUsed.Contains(lBallNumbers))
        {
            lBallNumbers = rand.RandiRange(0, 99);

        }

        return lBallNumbers;
    }

    public bool IsNumbersAllowed(int pNumbers)
    {
        return ballListManagerRef.GetBallNumbers().Contains(pNumbers);
       
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        ballTimerRef.OnBallSpawnerTimeout -= HandleNewBallRequest;
    }
}