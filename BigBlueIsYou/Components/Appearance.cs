using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Components
{
    public class Appearance : Component
    {
        public Texture2D image;
        public Color fill;
        public Color stroke;

        public Appearance(Texture2D image, Color fill, Color stroke)
        {
            this.image = image;
            this.fill = fill;
            this.stroke = stroke;
        }
    }
}
