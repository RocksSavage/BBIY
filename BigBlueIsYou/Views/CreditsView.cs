using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CS5410
{
    public class CreditsView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "Big Blue is You Written by Trent Savage and Bradley Sherman!";

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.Credits;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            Vector2 stringSize1 = m_font.MeasureString(MESSAGE);
            Vector2 stringSize = new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize1.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize1.Y);
            Printer.PrintWithOutline(MESSAGE, m_spriteBatch, stringSize, m_font, Color.Blue, Color.Yellow);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
