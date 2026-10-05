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

        // C'est beaucoup plus lisible ! On assigne par groupe :
        AssignSpecialBalls(new int[] { 4, 5, 6, 7 }, () => new CrossBall(), ballsTextures[1]);
        AssignSpecialBalls(new int[] { 8,9,10,11 }, () => new MultiplierBall(1), ballsTextures[2]);
        AssignSpecialBalls(new int[] { 12,13,14 }, () => new RandomBall(), ballsTextures[3]);
        AssignSpecialBalls(new int[] { 15,16 }, () => new GoldenBall(), ballsTextures[4]);
        AssignSpecialBalls(new int[] { 17,18,19 }, () => new SquareBall(), ballsTextures[5]);

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
    }
    #endregion


    // Create the dictionnary of each ball with his effetc
    private void InitializeRegistry()
    {
        ballDataRegistry.Clear();
        ballWeights.Clear(); 
        for (int i = 0; i <= playerStatusRef.CurrentBingoBallsAmount - 1; i++)
        {
            ballDataRegistry[i] = new BallData(i, new BasicBall(), ballsTextures[0]);
            ballWeights[i] = baseBallWeight; // Initial weight of probability
        }
    }

    // Add 10 ball to the dictionnary of balls
    public void ExpandRegistry(int newTotalAmount)
    {
        for (int i = ballDataRegistry.Count + 1; i <= newTotalAmount; i++)
        {
            ballDataRegistry[i] = new BallData(i, new BasicBall(), ballsTextures[0]);
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

        ballListManagerRef.AddBall(newBall);

        ballVisualDisplayerRef.AddVisualElement(newBall);

        OnNewBall?.Invoke(lSelectedBall);
    }

    public void ReplaceBallEffect(int pTargetBallNumber, IBallEffect pNewEffect, Texture2D pBallTexture)
    {
        if (ballDataRegistry.ContainsKey(pTargetBallNumber))
        {
            // On remplace l'ancien effet (souvent NoEffect) par le nouveau
            ballDataRegistry[pTargetBallNumber] = new BallData(pTargetBallNumber, pNewEffect, pBallTexture);
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