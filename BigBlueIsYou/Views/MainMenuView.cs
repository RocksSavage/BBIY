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
        private bool levelSelect = false;
        private Keys key;
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
            if (levelSelect){
                key = Keys.None;
                if(kBS.IsKeyDown(Keys.D1)) {key = Keys.D1; }
                if(kBS.IsKeyDown(Keys.D2)) {key = Keys.D2; }
                if(kBS.IsKeyDown(Keys.D3)) {key = Keys.D3; }
                if(kBS.IsKeyDown(Keys.D4)) {key = Keys.D4; }
                if(kBS.IsKeyDown(Keys.D5)) {key = Keys.D5; }
                if(kBS.IsKeyDown(Keys.NumPad1)) {key = Keys.NumPad1; }
                if(kBS.IsKeyDown(Keys.NumPad2)) {key = Keys.NumPad2; }
                if(kBS.IsKeyDown(Keys.NumPad3)) {key = Keys.NumPad3; }
                if(kBS.IsKeyDown(Keys.NumPad4)) {key = Keys.NumPad4; }
                if(kBS.IsKeyDown(Keys.NumPad5)) {key = Keys.NumPad5; }
                if (key != Keys.None){
                    if(key == Keys.D1 || key == Keys.NumPad1) { m_level[0] = 1; levelSelect = false;}
                    if(key == Keys.D2 || key == Keys.NumPad2) { m_level[0] = 2; levelSelect = false;}
                    if(key == Keys.D3 || key == Keys.NumPad3) { m_level[0] = 3; levelSelect = false;}
                    if(key == Keys.D4 || key == Keys.NumPad4) { m_level[0] = 4; levelSelect = false;}
                    if(key == Keys.D5 || key == Keys.NumPad5) { m_level[0] = 5; levelSelect = false;}
                    oldKBS = kBS;
                    return GameStateEnum.GamePlay;
                }
            } else {
                // Arrow keys to navigate the menu
                if (kBS.IsKeyUp(Keys.Down) && oldKBS.IsKeyDown(Keys.Down) && m_currentSelection != MenuState.Credits){
                    m_currentSelection = m_currentSelection + 1;
                }
                if (kBS.IsKeyUp(Keys.Up) && oldKBS.IsKeyDown(Keys.Up) && m_currentSelection != MenuState.NewGame){
                    m_currentSelection = m_currentSelection - 1;
                }
                
                if (kBS.IsKeyUp(Keys.Escape) && oldKBS.IsKeyDown(Keys.Escape)){
                    return GameStateEnum.Exit;
                }
                if (!(kBS.IsKeyUp(Keys.Enter) && oldKBS.IsKeyDown(Keys.Enter))) {
                    oldKBS = kBS; 
                    return GameStateEnum.MainMenu;
                }
                oldKBS = kBS;
                if (m_currentSelection == MenuState.NewGame) {
                    levelSelect = true;
                }
                if (m_currentSelection == MenuState.Controls){
                    oldKBS = kBS;
                    return GameStateEnum.Controls;
                }
                if (m_currentSelection == MenuState.Credits) {
                    oldKBS = kBS;
                    return GameStateEnum.Credits;
                }

            }
            return GameStateEnum.MainMenu;
        }
        public override void update(GameTime gameTime)
        {
        }
        public override void render(GameTime gameTime)
        {
            if (levelSelect == false){

                m_spriteBatch.Begin();

                Vector2 stringSize1 = m_fontTitle.MeasureString(Title);
                Vector2 stringSize = new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize1.X / 2, m_graphics.PreferredBackBufferHeight/8);
                Printer.PrintWithOutline(Title, m_spriteBatch, stringSize, m_fontTitle, Color.Green, Color.White);

                float bottom = drawMenuItem(m_currentSelection == MenuState.NewGame ? m_fontMenuSelect : m_fontMenu, "New Game", m_graphics.PreferredBackBufferHeight/2, m_currentSelection == MenuState.NewGame ? Color.Black : Color.Blue);
                bottom = drawMenuItem(m_currentSelection == MenuState.Controls ? m_fontMenuSelect : m_fontMenu, "Controls", bottom, m_currentSelection == MenuState.Controls ? Color.Black : Color.Blue);
                bottom = drawMenuItem(m_currentSelection == MenuState.Credits ? m_fontMenuSelect : m_fontMenu, "Credits", bottom, m_currentSelection == MenuState.Credits ? Color.Black : Color.Blue);

                m_spriteBatch.End();
            } else {
                m_spriteBatch.Begin();

                string usrMssg = "pick a level 1, 2, 3, 4, 5";
                Vector2 stringSize = m_fontMenu.MeasureString(usrMssg);
                m_spriteBatch.DrawString(m_fontMenu, usrMssg,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.Yellow);

                m_spriteBatch.End();
            }
        }

        private float drawMenuItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize1 = font.MeasureString(text);
            Vector2 stringSize = new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize1.X / 2, y);
            Printer.PrintWithOutline(text, m_spriteBatch, stringSize, font, color, Color.White);

            return y + stringSize1.Y;
        }
    }
}