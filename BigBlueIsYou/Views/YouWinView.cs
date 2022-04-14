using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CS5410.Particles;
using System;

namespace CS5410
{
    public class YouWinView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "You Won!   --   Press Enter to return to main menu";
        private KeyboardState kBS;
        private KeyboardState oldKBS;
        private Texture2D fire;
        private ParticleEmitter m_emitter1;

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            fire = contentManager.Load<Texture2D>("Images/fire");
            int middleX = m_graphics.GraphicsDevice.Viewport.Width / 2;
            int middleY = 10;
            m_emitter1 = new ParticleEmitter(
                fire,
                new TimeSpan(0, 0, 0, 0, 5),
                middleX, middleY,
                20,
                1,
                new TimeSpan(0, 0, 4),
                new TimeSpan(0, 0, 0, 0, 3000));
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
                return GameStateEnum.MainMenu;
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
            m_emitter1.draw(m_spriteBatch);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
            
            m_emitter1.update(gameTime);
        }
    }
}
