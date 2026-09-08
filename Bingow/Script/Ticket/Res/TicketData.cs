using Godot;

[GlobalClass]

// Resource of every Ticket
public partial class TicketData : Resource
{
    [Export] public int Cost { get; private set; } = 1;
    [Export] public int Value { get; private set; } = 5;
    [Export] public int Column { get; private set; } = 5;
    [Export] public int Row { get; private set; } = 5;
    [Export] public int SizeLevel { get; private set; } = 1; // Level requiered for the spot to be allowed to set this ticket 
    [Export] public Vector2 GridDisplayPosition { get; private set; } // Starting point (X, Y)
    [Export] public Vector2 GridDisplaySize { get; private set; }     //  lenght & height
    [Export] public int CellScale { get; private set; }   

    [Export] public PackedScene TicketScene { get; private set; }

    [Export] public Texture2D TicketTableVisual { get; private set; }
    [Export] public Texture2D TicketFullVisual { get; private set; }

    [Export] public PackedScene CellScene { get; private set; }

}