using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Obstacle
    {
        public static Entity create(Texture2D square, int x, int y)
        {
            var obstacle = new Entity();

            obstacle.Add(new Components.Appearance(square, new Color(0, 255, 0), Color.Black));
            obstacle.Add(new Components.Position(x, y));
            obstacle.Add(new Components.Collision());

            return obstacle;
        }
    }
}