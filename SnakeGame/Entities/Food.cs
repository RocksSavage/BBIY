using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Food
    {
        public static Entity create(Texture2D square, int x, int y)
        {
            var food = new Entity();

            food.Add(new Components.Appearance(square, new Color(255, 128, 0), Color.Black));
            food.Add(new Components.Position(x, y));
            food.Add(new Components.Collision());
            food.Add(new Components.Food());

            return food;
        }
    }
}