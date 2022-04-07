using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CS5410
{
    public class Printer
    {
        public static void PrintWithOutline(string message,SpriteBatch m_spriteBatch, Vector2 stringSize, SpriteFont m_font, Color fillColor, Color outlineColor)

        {
            m_spriteBatch.DrawString(m_font, message, new Vector2(stringSize.X - 1, stringSize.Y), outlineColor);
            m_spriteBatch.DrawString(m_font, message, new Vector2(stringSize.X + 1, stringSize.Y), outlineColor);
            m_spriteBatch.DrawString(m_font, message, new Vector2(stringSize.X, stringSize.Y - 1), outlineColor);
            m_spriteBatch.DrawString(m_font, message, new Vector2(stringSize.X, stringSize.Y + 1), outlineColor);
            m_spriteBatch.DrawString(m_font, message, new Vector2(stringSize.X, stringSize.Y), fillColor);
        }

        public static void PrintThing(Thing t, Cell c, SpriteBatch spriteBatch, SpriteFont font)
        {
            // so there s something inthere water.
            
            
            String s = Char.ToString(t.m_name);
            Vector2 stringSize = new Vector2(c.X * 30, (c.Y + 2) * 30);
            Printer.PrintWithOutline(s, spriteBatch, stringSize, font, Color.Green, Color.White);


        }
    }
}
