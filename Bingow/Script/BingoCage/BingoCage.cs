using Godot;
using System;
using System.Collections.Generic;

public partial class BingoCage : Node
{
    [Export] private BallSpawner ballSpawnerRef;
    [Export] private BallListManager ballListManagerRef;
    [Export] private AutoBall autoBallRef;
    [Export] private BallVisualDisplayer ballVisualDisplayerRef;
    [Export] private PlayerStatus playerStatusRef;
    [Export] private CrankManager crankManagerRef;

    [Export] private Texture2D[] ballsTextures;

    private RandomNumberGenerator rand = new RandomNumberGenerator();

    public event Action<int> OnNewBall;

    public BingoCageUpgrade bingoCageUpgrade { get; private set; } = new BingoCageUpgrade();

    
    private Dictionary<int, BallData> ballDataRegistry = new Dictionary<int, BallData>();
    private Dictionary<int, int> ballWeights = new Dictionary<int, int>(); // weight of propability 

    private int lowestBallWeight = 1;
    private int increaseBallWeight = 4;
    private int baseBallWeight = 10;

    #region ready
    public override void _Ready()
    {
        autoBallRef.OnBallSpawnerTimeout += HandleNewBallRequest;
        crankManagerRef.OnCrankActionned += HandleNewBallRequest;
        TicketEventBus.OnUpgradeBought += UpdateUpgradesStatus;
        InitializeRegistry();
        //AssignSpecialBalls(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 }, () => new MultiplierBall(1), ballsTextures[2]);
        //AssignSpecialBalls(new int[] {  1 , 3 , 5 , 10 }, () => new MultiplierBall(1), ballsTextures[2]);

        AssignSpecialBalls(new int[] { 4, 19, 37, 52, 68, 83, 99, 11, 27, 43 }, () => new CrossBall(), ballsTextures[1]);

        AssignSpecialBalls(new int[] { 5, 20, 36, 51, 67, 82, 98, 13, 29, 45, 61, 76, 92, 7, 23, 38, 54, 70, 86, 101, 16, 32, 48, 64, 80, 96, 9, 25, 41, 66, 21, 35, 49, 72, 88, 102, 46, 84, 31, 47, 63, 79, 95, 8, 24, 40, 65, 71, 87, 103, 15, 58, 74, 90, 6, 22, 39, 55 }, () => new MultiplierBall(1), ballsTextures[2]);

        AssignSpecialBalls(new int[] { 12, 28, 44, 59, 75, 91, 18 }, () => new RandomBall(), ballsTextures[3]);

        AssignSpecialBalls(new int[] { 10, 26, 42, 56, 73, 89, 14, 30, 46, 62, 78, 94, 17, 33, 53, 77, 93, 36, 60, 97, 34, 50, 69, 85, 100 }, () => new SquareBall(), ballsTextures[5]);

        UpdateUpgradesStatus();
    }


    private void AssignSpecialBalls(int[] pNumbers, Func<IBallEffect> pEffectCreator, Texture2D pTexture)
    {
        foreach (int num in pNumbers)
        {
            ReplaceBallEffect(num, pEffectCreator(), pTexture);
        }
    }

    public override void _ExitTree()
    {
        autoBallRef.OnBallSpawnerTimeout -= HandleNewBallRequest;
        crankManagerRef.OnCrankActionned -= HandleNewBallRequest;

        TicketEventBus.OnUpgradeBought -= UpdateUpgradesStatus;

    }

    private void UpdateUpgradesStatus()
    {
        autoBallRef.UpdateUpgradeStatus(bingoCageUpgrade.ballPerS.level);
        ballListManagerRef.UpdateUpgradeStatus(bingoCageUpgrade.cageCapacity.level);
        crankManagerRef.UpdateUpgradeStatus(bingoCageUpgrade.ballManualCd.level);

        ballVisualDisplayerRef.UpdateTubeSize(ballListManagerRef.ContainerCapacity);
    }
    #endregion


    // Create the dictionnary of each ball with his effetc
    private void InitializeRegistry()
    {
        ballDataRegistry.Clear();
        ballWeights.Clear(); 
        for (int i = 0; i <= playerStatusRef.CurrentBingoBallsAmount - 1; i++)
        {
            ballDataRegistry[i] = new BallData(i, new BasicBall(), ballsTextures[0], EBallId.Basic);
            ballWeights[i] = baseBallWeight; // Initial weight of probability
        }
    }

    // Add 10 ball to the dictionnary of balls
    public void ExpandRegistry(int newTotalAmount)
    {
        for (int i = ballDataRegistry.Count + 1; i <= newTotalAmount; i++)
        {
            ballDataRegistry[i] = new BallData(i, new BasicBall(), ballsTextures[0], EBallId.Basic);
        }


    }

    private void HandleNewBallRequest()
    {
        int lTotalWeight = 0;
        List<int> lAvailableBalls = new List<int>();
        // Get all the numbers alredy present in the screen
        List<int> lBallNumbersUsed = ballListManagerRef.GetBallNumbers();

        // calcul all the weight of the ball in the bingo cage
        for (int i = 0; i <= playerStatusRef.CurrentBingoBallsAmount - 1; i++)
        {
            // dont take the number alredy out of the bingo cage 
            if (!lBallNumbersUsed.Contains(i))
            {
                lTotalWeight += ballWeights[i];
                lAvailableBalls.Add(i);
            }
        }

        // Take a random number betwen the total weight
        int lRandomValue = rand.RandiRange(1, lTotalWeight);
        int lSelectedBall = lAvailableBalls[0];
        int lCurrentSum = 0;

        // check if the weight picked is in on number weight
        foreach (int lBall in lAvailableBalls)
        {
            lCurrentSum += ballWeights[lBall];

            // if the number is lower than this its this ball picked
            if (lRandomValue <= lCurrentSum)
            {
                lSelectedBall = lBall;
                break;
            }
        }

        // Total weight adjustement
        foreach (int lBall in lAvailableBalls)
        {
            if (lBall == lSelectedBall)
            {
                //  this ball juste get out , put his weight at lowest
                ballWeights[lBall] = lowestBallWeight;
            }
            else
            {
                // this ball didnt get out , increase her probability of being picked
                ballWeights[lBall] += increaseBallWeight;
            }
        }


        // Create new ball and take reference for adding it to the list
        BingoBall newBall = ballSpawnerRef.SpawnBall(lSelectedBall, ballDataRegistry[lSelectedBall]);

        BingoBall ejectedBall = ballListManagerRef.AddBall(newBall);
        if (ejectedBall != null)
        {
            ballVisualDisplayerRef.EjectBall(ejectedBall);
        }

        ballVisualDisplayerRef.AddAndShiftBalls(newBall, ballListManagerRef.CurrentBingoBalls);

        OnNewBall?.Invoke(lSelectedBall);
    }

    public void ReplaceBallEffect(int pTargetBallNumber, IBallEffect pNewEffect, Texture2D pBallTexture)
    {
        if (ballDataRegistry.ContainsKey(pTargetBallNumber))
        {
            // On remplace l'ancien effet (souvent NoEffect) par le nouveau
            ballDataRegistry[pTargetBallNumber] = new BallData(pTargetBallNumber, pNewEffect, pBallTexture, EBallId.Basic);
        }
        else
        {
            GD.PrintErr($"imposible la balle {pTargetBallNumber} nexiste pas encore.");
        }
    }

    public bool IsNumbersAllowed(int pNumbers)
    {   
        return ballListManagerRef.GetBallNumbers().Contains(pNumbers);
       
    }

    public IBallEffect GetNumberEffect(int pNumber)
    {
        return ballDataRegistry[pNumber].ballEffect;
    }

    public List<int> GetCurrentBalls()
    {
        return ballListManagerRef.GetBallNumbers();

    }

}