using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CS5410
{
    public class GamePlayView : GameStateView
    {
        private KeyboardState kBS;
        private KeyboardState oldKBS;
        ContentManager m_content;
        private GameModel m_gameModel;

        public override void initializeSession()
        {
            m_gameModel = new GameModel(m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight);
            m_gameModel.Initialize(m_content, m_spriteBatch);
        }

        public override void loadContent(ContentManager content)
        {
            m_content = content;
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            kBS = Keyboard.GetState();
            if (kBS.IsKeyUp(Keys.Escape) && oldKBS.IsKeyDown(Keys.Escape)) {
                oldKBS = kBS;
                initializeSession();
                return GameStateEnum.MainMenu;
            }
            if (kBS.IsKeyUp(Keys.R) && oldKBS.IsKeyDown(Keys.R)){
                initializeSession();
            }

            oldKBS = kBS;
            return GameStateEnum.GamePlay;
        }

        public override void render(GameTime gameTime)
        {
            m_gameModel.Draw(gameTime);
        }

        public override void update(GameTime gameTime)
        {
            m_gameModel.Update(gameTime);
        }
    }
}
