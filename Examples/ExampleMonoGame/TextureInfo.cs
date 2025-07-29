using Microsoft.Xna.Framework.Graphics;

namespace MonoGameExample;

public sealed class TextureInfo
{
    public Texture2D Texture { get; set; }
    public bool IsManaged { get; set; }
}
