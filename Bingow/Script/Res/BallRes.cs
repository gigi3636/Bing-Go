using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class BallRes : Resource
{
    [Export] public EBallId ballEffect { get; private set; }
    [Export] public string ballDesc { get; private set; }


    [Export] public Texture2D texture { get; private set; }

    [Export] public bool isUnlocked { get; private set; }

    public int ballLevel { get; private set; }

}
