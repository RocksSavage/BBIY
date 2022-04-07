using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CS5410
{
    public class GameOverView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "You Died";
        private KeyboardState kBS;
        private KeyboardState oldKBS;

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            kBS = Keyboard.GetState();
            if (kBS.IsKeyUp(Keys.Escape) && oldKBS.IsKeyDown(Keys.Escape)){
                return GameStateEnum.MainMenu;
            }
            oldKBS = kBS;
            return GameStateEnum.GameOver;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            Vector2 stringSize1 = m_font.MeasureString(MESSAGE);
            Vector2 stringSize = new Vector2( m_graphics.GraphicsDevice.Viewport.Width / 2 - stringSize1.X/2, m_graphics.GraphicsDevice.Viewport.Height / 2- stringSize1.Y/2);
            Printer.PrintWithOutline(MESSAGE, m_spriteBatch, stringSize, m_font, Color.Green, Color.White);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
