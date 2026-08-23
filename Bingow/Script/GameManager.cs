using Godot;
using System;

public partial class GameManager : Node
{
    [Export] private PlayerStatus playerStatusRes;


    public override void _Ready()
    {
        base._Ready();
        StartNewRun();
    }

    private void StartNewRun()
    {
        playerStatusRes.StartNewRun();
    }
}
