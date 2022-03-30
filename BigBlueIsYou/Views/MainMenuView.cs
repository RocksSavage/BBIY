using Microsoft.Xna.Framework;
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
        private SpriteFont m_fontTitle;
        private SpriteFont m_fontMenu;
        private SpriteFont m_fontMenuSelect;
        private const string Title = "BBIY";

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
            m_fontTitle = contentManager.Load<SpriteFont>("Fonts/gameTitle");
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

            Vector2 stringSize1 = m_fontTitle.MeasureString(Title);
            Vector2 stringSize = new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize1.X / 2, m_graphics.PreferredBackBufferHeight/8);
            Printer.PrintWithOutline(Title, m_graphics, m_spriteBatch, stringSize, m_fontTitle, Color.Green, Color.White);

            float bottom = drawMenuItem(m_currentSelection == MenuState.NewGame ? m_fontMenuSelect : m_fontMenu, "New Game", m_graphics.PreferredBackBufferHeight/2, m_currentSelection == MenuState.NewGame ? Color.Black : Color.Blue);
            bottom = drawMenuItem(m_currentSelection == MenuState.Controls ? m_fontMenuSelect : m_fontMenu, "Controls", bottom, m_currentSelection == MenuState.Controls ? Color.Black : Color.Blue);
            bottom = drawMenuItem(m_currentSelection == MenuState.Credits ? m_fontMenuSelect : m_fontMenu, "Credits", bottom, m_currentSelection == MenuState.Credits ? Color.Black : Color.Blue);

            m_spriteBatch.End();
        }

        private float drawMenuItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize1 = font.MeasureString(text);
            // m_spriteBatch.DrawString(font, text, new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y), color);
            
            Vector2 stringSize = new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize1.X / 2, y);
            Printer.PrintWithOutline(text, m_graphics, m_spriteBatch, stringSize, font, color, Color.White);

            return y + stringSize1.Y;
        }
    }
}