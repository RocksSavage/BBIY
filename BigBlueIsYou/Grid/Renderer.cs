using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CS5410
{
    public class Renderer
    {
        // global variables (look away, grader!)
        public static Point cellDim = new Point(30, 30);
        public static Point sourceDim = new Point(24, 24);
        // the Great Wall of Sprites
        static Texture2D grass;

        public static void loadSprites(ContentManager contentManager)
        {
            grass = contentManager.Load<Texture2D>("Sprites/grass");
        }

        public static void PrintThing(Thing t, Cell c, SpriteBatch spriteBatch, SpriteFont font)
        {
            // so there s something inthere water.
            //{'W', 'R', 'F', 'B', 'I', 'S', 'P', 'V', 'A', 'Y', 'X', 'N', 'K'}
            //{'w', 'r', 'f', 'b', 'l', 'g', 'a', 'v', 'h'}
            if (t.m_name == 'h')
            {
                spriteBatch.Draw(grass, new Rectangle(c.coord * cellDim, cellDim), new Rectangle(new Point(0, 0), sourceDim), Color.Green);
            }
            else
            {
                String s = Char.ToString(t.m_name);
                Vector2 stringSize = new Vector2(c.X * 30, (c.Y + 2) * 30);
                Printer.PrintWithOutline(s, spriteBatch, stringSize, font, Color.Green, Color.White);
            }
        }
}
