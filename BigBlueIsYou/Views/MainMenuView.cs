﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CS5410
{
    public class MainMenuView : GameStateView
    {
        private KeyboardState kBS;
        private KeyboardState oldKBS;
        private SpriteFont m_fontMenu;
        private SpriteFont m_fontMenuSelect;

        private enum MenuState
        {
            NewGame,
            Controls,
            Credits,
            Exit
        }

        private MenuState m_currentSelection = MenuState.NewGame;

        public override void loadContent(ContentManager contentManager)
        {
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");
        }
        public override GameStateEnum processInput(GameTime gameTime)
        {
            kBS = Keyboard.GetState();
            // Arrow keys to navigate the menu
            if (kBS.IsKeyUp(Keys.Down) && oldKBS.IsKeyDown(Keys.Down) && m_currentSelection != MenuState.Credits){
                m_currentSelection = m_currentSelection + 1;
            }
            if (kBS.IsKeyUp(Keys.Up) && oldKBS.IsKeyDown(Keys.Up) && m_currentSelection != MenuState.NewGame){
                m_currentSelection = m_currentSelection - 1;
            }
            
            if (kBS.IsKeyUp(Keys.Escape) && oldKBS.IsKeyDown(Keys.Escape)){
                Console.WriteLine("This Happens eh");
                return GameStateEnum.Exit;
            }
            if (!(kBS.IsKeyUp(Keys.Enter) && oldKBS.IsKeyDown(Keys.Enter))) {
                oldKBS = kBS; 
                return GameStateEnum.MainMenu;
            }
            oldKBS = kBS;
            if (m_currentSelection == MenuState.NewGame) {
                oldKBS = kBS;
                return GameStateEnum.GamePlay;
            }
            if (m_currentSelection == MenuState.Controls){
                oldKBS = kBS;
                Console.WriteLine("This Happens");
                return GameStateEnum.Controls;
            }
            if (m_currentSelection == MenuState.Credits) {
                oldKBS = kBS;
                return GameStateEnum.Credits;
            }

            return GameStateEnum.MainMenu;
        }
        public override void update(GameTime gameTime)
        {
        }
        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // I split the first one's parameters on separate lines to help you see them better
            float bottom = drawMenuItem(
                m_currentSelection == MenuState.NewGame ? m_fontMenuSelect : m_fontMenu, 
                "New Game",
                200, 
                m_currentSelection == MenuState.NewGame ? Color.Yellow : Color.Blue);
            // bottom = drawMenuItem(m_currentSelection == MenuState.HighScores ? m_fontMenuSelect : m_fontMenu, "High Scores", bottom, m_currentSelection == MenuState.HighScores ? Color.Yellow : Color.Blue);
            bottom = drawMenuItem(m_currentSelection == MenuState.Controls ? m_fontMenuSelect : m_fontMenu, "Controls", bottom, m_currentSelection == MenuState.Controls ? Color.Yellow : Color.Blue);
            // bottom = drawMenuItem(m_currentSelection == MenuState.Help ? m_fontMenuSelect : m_fontMenu, "Help", bottom, m_currentSelection == MenuState.Help ? Color.Yellow : Color.Blue);
            bottom = drawMenuItem(m_currentSelection == MenuState.Credits ? m_fontMenuSelect : m_fontMenu, "Credits", bottom, m_currentSelection == MenuState.Credits ? Color.Yellow : Color.Blue);
            // drawMenuItem(m_currentSelection == MenuState.Quit ? m_fontMenuSelect : m_fontMenu, "Quit", bottom, m_currentSelection == MenuState.Quit ? Color.Yellow : Color.Blue);

            m_spriteBatch.End();
        }

        private float drawMenuItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize = font.MeasureString(text);
            m_spriteBatch.DrawString(
                font,
                text,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y),
                color);

            return y + stringSize.Y;
        }
    }
}