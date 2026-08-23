using Godot;

[GlobalClass]

// Resource of every Ticket
public partial class TicketData : Resource
{
    [Export] public int Cost { get; private set; } = 1;
    [Export] public int Column { get; private set; } = 5;
    [Export] public int Row { get; private set; } = 5;
    [Export] public int HigherNumber { get; private set; } = 99;
    [Export] public PackedScene TicketScene { get; private set; }

    [Export] public Texture2D TicketVisual { get; private set; }

    [Export] public PackedScene CellScene { get; private set; }

}