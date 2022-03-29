using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Components
{
    public class Position : Component
    {
        public List<Point> segments = new List<Point>();
        public int x { get { return segments[0].X; } }
        public int y { get { return segments[0].Y; } }

        public Position(int x, int y)
        {
            segments.Add(new Point(x, y));
        }
    }
}
