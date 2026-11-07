using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class BallRes : Resource
{
    [Export] public EBallId ballEffect { get; private set; }
    [Export] public string ballDesc { get; private set; }
    [Export] public int[] dropPercents { get; private set; }
    [Export] public int[] ballNumbers { get; private set; }


    [Export] public Texture2D miniTexture { get; private set; }
    [Export] public Texture2D texture { get; private set; }

    [Export] public bool isUnlocked { get; private set; }

    [Export] public int ballLevel { get; private set; }

}
