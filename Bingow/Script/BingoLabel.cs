using Godot;

public partial class BingoLabel : RichTextLabel
{
    [Export] private Font font { get; set; }
    public override void _Ready()
    {
        BbcodeEnabled = true;
        FitContent = true;
        ClipContents = false;
        TextureFilter = TextureFilterEnum.Nearest;

        Text = "[center][wave amp=45.0 freq=3.5 connected=1][outline_size=18][outline_color=#3d1d04][color=#f7d038][font_size=120]BINGO![/font_size][/color][/outline_color][/outline_size][/wave][/center]";

        Font maPolice = font;
        AddThemeFontOverride("normal_font", maPolice);
    }
}