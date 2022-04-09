using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CS5410
{
    public class YouWinView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "You Won!   --   Press Enter to Advance";
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
                oldKBS = kBS;
                return GameStateEnum.MainMenu;
            }
            if (kBS.IsKeyUp(Keys.Enter) && oldKBS.IsKeyDown(Keys.Enter)){
                oldKBS = kBS;
                return GameStateEnum.GamePlay;
            }
            oldKBS = kBS;
            return GameStateEnum.YouWin;
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
