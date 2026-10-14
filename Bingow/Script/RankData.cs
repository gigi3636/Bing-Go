using Godot;

public partial class RankData : RefCounted
{
    public string Name { get; set; }
    public int MinScore { get; set; }
    public string IconPath { get; set; } // Chemin optionnel vers l'image du rang

    public RankData(string name, int minScore, string iconPath = "")
    {
        Name = name;
        MinScore = minScore;
        IconPath = iconPath;
    }
}