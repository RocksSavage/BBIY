﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CS5410
{
    public class ECSBigBlueIsYou : Game
    {
        private GraphicsDeviceManager m_graphics;
        private IGameState m_currentState;
        private GameStateEnum m_nextStateEnum = GameStateEnum.MainMenu;
        private Dictionary<GameStateEnum, IGameState> m_states;
        public List<Keys> m_controls = new List<Keys> {Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space};

        public ECSBigBlueIsYou()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Set window size preferences
            m_graphics.IsFullScreen = false;
            m_graphics.PreferredBackBufferWidth = 1080;
            m_graphics.PreferredBackBufferHeight = 720;

            m_graphics.ApplyChanges();

            // Create all the game states here
            m_states = new Dictionary<GameStateEnum, IGameState>();
            m_states.Add(GameStateEnum.MainMenu, new MainMenuView());
            m_states.Add(GameStateEnum.GamePlay, new GamePlayView());
            m_states.Add(GameStateEnum.Controls, new ControlsView());
            m_states.Add(GameStateEnum.Credits, new CreditsView());

            // We are starting with the main menu
            m_currentState = m_states[GameStateEnum.MainMenu];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Give all game states a chance to load their content
            foreach (var item in m_states)
            {
                item.Value.initialize(this.GraphicsDevice, m_graphics, m_controls);
                item.Value.loadContent(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            m_nextStateEnum = m_currentState.processInput(gameTime);

            // // Special case for exiting the game
            if (m_nextStateEnum == GameStateEnum.Exit)
            {
                Exit();
            }

            m_currentState.update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            m_currentState.render(gameTime);

            if (m_currentState != m_states[m_nextStateEnum])
            {
                m_currentState = m_states[m_nextStateEnum];
                m_currentState.initializeSession();
            }
            

            base.Draw(gameTime);
        }
    }
}
