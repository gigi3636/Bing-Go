using Godot;

public partial class CustomCursor : Node
{
    [Export] private Texture2D _curseurNormal;
    [Export] private Texture2D _curseurClic;

    private Texture2D _curseurNormalRedimensionne;
    private Texture2D _curseurClicRedimensionne;
    private Vector2 hotspot = new Vector2(18, 0);

    public override void _Ready()
    {
        Vector2I nouvelleTaille = new Vector2I(17*3, 23*3);

        _curseurNormalRedimensionne = RedimensionnerTexture(_curseurNormal, nouvelleTaille);
        _curseurClicRedimensionne = RedimensionnerTexture(_curseurClic, nouvelleTaille);

        Input.SetCustomMouseCursor(_curseurNormalRedimensionne, Input.CursorShape.Arrow, hotspot);
    }

    private Texture2D RedimensionnerTexture(Texture2D textureOriginale, Vector2I taille)
    {
        if (textureOriginale == null) return null;

        Image image = textureOriginale.GetImage();


        image.Resize(taille.X, taille.Y, Image.Interpolation.Nearest);

        return ImageTexture.CreateFromImage(image);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {

                if (mouseEvent.Pressed)
                {
                    Input.SetCustomMouseCursor(_curseurClicRedimensionne, Input.CursorShape.Arrow, hotspot);
                    AudioManager.Instance.PlaySfx("MouseClick");

                }
                else
                {
                    Input.SetCustomMouseCursor(_curseurNormalRedimensionne, Input.CursorShape.Arrow, hotspot);
                }
            }
        }
    }
}