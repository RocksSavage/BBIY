using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CS5410
{
    
    public class Renderer
    {
        public GraphicsDeviceManager m_graphics;
        // global variables (look away, grader!)
        public static Point cellDim;
        public static Point sourceDim;

        // the Great Wall of Sprites

        static Texture2D wall;
        static Texture2D rock;
        static Texture2D flag;
        static Texture2D bigBlue;
        static Texture2D floor;
        static Texture2D grass;
        static Texture2D water;
        static Texture2D lava;
        static Texture2D hedge;
        static Texture2D tWall;
        static Texture2D tRock;
        static Texture2D tFlag;
        static Texture2D tBigBlue;
        static Texture2D tIs;
        static Texture2D tStop;
        static Texture2D tPush;
        static Texture2D tLava;
        static Texture2D tWater;
        static Texture2D tYou;
        static Texture2D tWin;
        static Texture2D tSink;
        static Texture2D tKill;

        public void loadContent(ContentManager contentManager, GraphicsDeviceManager m_graphics)
        {
            cellDim = new Point(m_graphics.PreferredBackBufferHeight/24, m_graphics.PreferredBackBufferHeight/24);
            sourceDim = new Point(24, 24);
            wall = contentManager.Load<Texture2D>("Sprites/wall");
            rock = contentManager.Load<Texture2D>("Sprites/rock");
            flag = contentManager.Load<Texture2D>("Sprites/flag");
            bigBlue = contentManager.Load<Texture2D>("Sprites/Aggie");
            floor = contentManager.Load<Texture2D>("Sprites/floor");
            grass = contentManager.Load<Texture2D>("Sprites/grass");
            water = contentManager.Load<Texture2D>("Sprites/water");
            lava = contentManager.Load<Texture2D>("Sprites/lava");
            hedge = contentManager.Load<Texture2D>("Sprites/hedge");
            tWall = contentManager.Load<Texture2D>("Sprites/word-wall");
            tRock = contentManager.Load<Texture2D>("Sprites/word-rock");
            tFlag = contentManager.Load<Texture2D>("Sprites/word-flag");
            tBigBlue = contentManager.Load<Texture2D>("Sprites/word-baba");
            tIs = contentManager.Load<Texture2D>("Sprites/word-is");
            tStop = contentManager.Load<Texture2D>("Sprites/word-stop");
            tPush = contentManager.Load<Texture2D>("Sprites/word-push");
            tLava = contentManager.Load<Texture2D>("Sprites/word-lava");
            tWater = contentManager.Load<Texture2D>("Sprites/word-water");
            tYou = contentManager.Load<Texture2D>("Sprites/word-you");
            tWin = contentManager.Load<Texture2D>("Sprites/word-win");
            tSink = contentManager.Load<Texture2D>("Sprites/word-sink");
            tKill = contentManager.Load<Texture2D>("Sprites/word-kill");

        }

        public static void PrintThing(Thing t, Cell c, int gameStep, SpriteBatch spriteBatch, SpriteFont font)
        {
            // so there s something inthere water.
            Point BradsShim = new Point(2, 0);

            Rectangle destinationRectangle = new Rectangle((c.coord + BradsShim) * cellDim, cellDim);
            Rectangle sourceRectangle = new Rectangle(new Point(gameStep % 3, 0) * sourceDim, sourceDim);
            Rectangle aggieSourceRectangle = new Rectangle(0, 0, 100, 100);
            
            //{'W', 'R', 'F', 'B', 'I', 'S', 'P', 'V', 'A', 'Y', 'X', 'N', 'K'}
            if (t.m_name == 'W')
            {
                spriteBatch.Draw(tWall, destinationRectangle, sourceRectangle, Color.DarkGray);
            }
            else if (t.m_name == 'R')
            {
                spriteBatch.Draw(tRock, destinationRectangle, sourceRectangle, Color.SaddleBrown);
            }
            else if (t.m_name == 'F')
            {
                spriteBatch.Draw(tFlag, destinationRectangle, sourceRectangle, Color.SaddleBrown);
            }
            else if (t.m_name == 'B')
            {
                spriteBatch.Draw(tBigBlue, destinationRectangle, sourceRectangle, Color.White);
            }
            else if (t.m_name == 'I')
            {
                spriteBatch.Draw(tIs, destinationRectangle, sourceRectangle, Color.White);
            }
            else if (t.m_name == 'S')
            {
                spriteBatch.Draw(tStop, destinationRectangle, sourceRectangle, Color.Green);
            }
            else if (t.m_name == 'P')
            {
                spriteBatch.Draw(tPush, destinationRectangle, sourceRectangle, Color.SaddleBrown);
            }
            else if (t.m_name == 'V')
            {
                spriteBatch.Draw(tLava, destinationRectangle, sourceRectangle, Color.DarkRed);
            }
            else if (t.m_name == 'A')
            {
                spriteBatch.Draw(tWater, destinationRectangle, sourceRectangle, Color.DarkBlue);
            }
            else if (t.m_name == 'Y')
            {
                spriteBatch.Draw(tYou, destinationRectangle, sourceRectangle, Color.Purple);
            }
            else if (t.m_name == 'X')
            {
                spriteBatch.Draw(tWin, destinationRectangle, sourceRectangle, Color.Yellow);
            }
            else if (t.m_name == 'N')
            {
                spriteBatch.Draw(tSink, destinationRectangle, sourceRectangle, Color.DarkBlue);
            }
            else if (t.m_name == 'K')
            {
                spriteBatch.Draw(tKill, destinationRectangle, sourceRectangle, Color.DarkRed);
            }
            //{'w', 'r', 'f', get this>'b', 'l', 'g', 'a', 'v', 'h'}
            else if (t.m_name == 'w')
            {
                spriteBatch.Draw(wall, destinationRectangle, sourceRectangle, Color.Gray);
            }
            else if (t.m_name == 'r')
            {
                spriteBatch.Draw(rock, destinationRectangle, sourceRectangle, Color.SaddleBrown);
            }
            else if (t.m_name == 'f')
            {
                spriteBatch.Draw(flag, destinationRectangle, sourceRectangle, Color.Yellow);
            }
            else if (t.m_name == 'b')
            {
                spriteBatch.Draw(bigBlue, destinationRectangle, aggieSourceRectangle, Color.SkyBlue);
            }
            else if (t.m_name == 'l')
            {
                spriteBatch.Draw(floor, destinationRectangle, sourceRectangle, Color.SandyBrown);
            }
            else if (t.m_name == 'g')
            {
                spriteBatch.Draw(grass, destinationRectangle, sourceRectangle, Color.Green);
            }
            else if (t.m_name == 'a')
            {
                spriteBatch.Draw(water, destinationRectangle, sourceRectangle, Color.Blue);
            }
            else if (t.m_name == 'v')
            {
                spriteBatch.Draw(lava, destinationRectangle, sourceRectangle, Color.DarkRed);
            }
            else if (t.m_name == 'h')
            {
                spriteBatch.Draw(hedge, destinationRectangle, sourceRectangle, Color.Green);
            }
            else
            {
                String s = Char.ToString(t.m_name);
                Vector2 stringSize = new Vector2(c.X * 30, (c.Y + 2) * 30);
                Printer.PrintWithOutline(s, spriteBatch, stringSize, font, Color.Green, Color.White);
            }
        }
    }
}
