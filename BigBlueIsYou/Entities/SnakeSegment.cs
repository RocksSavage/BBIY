using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Entities
{
    public class SnakeSegment
    {
        private const int MOVE_INTERVAL = 150; // milliseconds
        public static Entity create(Texture2D square, int x, int y)
        {
            var snake = new Entity();

            snake.Add(new Components.Appearance(square, Color.White, Color.Black));
            snake.Add(new Components.Position(x, y));
            snake.Add(new Components.Collision());
            snake.Add(new Components.Movable(Components.Direction.Stopped, MOVE_INTERVAL));
            snake.Add(new Components.KeyboardControlled(
                new Dictionary<Keys, Components.Direction>
                {
                        { Keys.Up, Components.Direction.Up },
                        { Keys.Down, Components.Direction.Down },
                        { Keys.Left, Components.Direction.Left },
                        { Keys.Right, Components.Direction.Right }
                }));

            return snake;
        }
    }
}