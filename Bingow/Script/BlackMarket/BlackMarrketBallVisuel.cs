using Godot;
using System;

public partial class BlackMarrketBallVisuel : Node
{
    [Export] private TextureRect ballTextureRef;

    public Action<BallRes> OnBallClicked;
    private BallRes ballRes;
    public void Initialize (BallRes pBallRes)
    {
        ballTextureRef.Texture = pBallRes.texture;
        ballRes = pBallRes;

        ballTextureRef.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        ballTextureRef.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        ballTextureRef.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;
        if (!ballRes.isUnlocked) ballTextureRef.Modulate = Color.Color8(15, 15, 15, 255);


    }

    private void _on_button_pressed()
    {
        OnBallClicked?.Invoke(ballRes);
    }
}
