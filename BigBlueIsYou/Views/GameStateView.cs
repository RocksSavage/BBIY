using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
// using System;
using CS5410.Input;

namespace CS5410
{
    public abstract class GameStateView : IGameState
    {
        protected GraphicsDeviceManager m_graphics;
        protected SpriteBatch m_spriteBatch;
        protected KeyboardInput m_inputKeyboard;
        public List<Keys> m_controls;

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, KeyboardInput inputKeyboard, List<Keys> controls)
        {
            m_graphics = graphics;
            m_spriteBatch = new SpriteBatch(graphicsDevice);
            m_inputKeyboard = inputKeyboard;
            m_controls = controls;
        }

        public virtual void initializeSession() { }
        public abstract void loadContent(ContentManager contentManager);
        public abstract GameStateEnum processInput(GameTime gameTime);
        public abstract void render(GameTime gameTime);
        public abstract void update(GameTime gameTime);
    }
}
