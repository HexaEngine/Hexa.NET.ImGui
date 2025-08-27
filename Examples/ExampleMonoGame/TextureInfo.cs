using Microsoft.Xna.Framework.Graphics;

namespace ExampleMonoGame;

public sealed class TextureInfo
{
    public Texture2D Texture { get; set; }
    public bool IsManaged { get; set; }
}
