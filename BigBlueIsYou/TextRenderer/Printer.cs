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
    }
}
