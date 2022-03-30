using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace CS5410
{
    public class ControlsView : GameStateView
    {
        private KeyboardState kBS;
        private KeyboardState oldKBS;
        private SpriteFont m_font;
        private SpriteFont m_fontMenu;
        private SpriteFont m_fontMenuSelect;
        private const string MESSAGE = "                      Enter the new key: \r\n (Alpha Numeric and directional keys only)";
        private bool m_enterNewKey = false;

        private Selection m_currentSelection = Selection.UP;
        private List<string> controlStrings;
        private List<string> controlStringsWithInput;
        private Keys key;
        private enum Selection
        {
            UP,
            DOWN,
            LEFT, 
            RIGHT, 
            SHOOT, 
            QUIT
        }

        public override void loadContent(ContentManager contentManager){
            controlStrings = new List<string> {"UP", "DOWN", "LEFT", "RIGHT", "SHOOT", "QUIT"};
            controlStringsWithInput = new List<string> {"UP", "DOWN", "LEFT", "RIGHT", "SHOOT", "QUIT"};
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");
            updateControlStringsWithInput();
        }

        public override GameStateEnum processInput(GameTime gameTime){
            kBS = Keyboard.GetState();
            if (m_enterNewKey){
                key = Keys.None;
                if(kBS.IsKeyDown(Keys.A)) {key = Keys.A; }
                if(kBS.IsKeyDown(Keys.B)) {key = Keys.B; }
                if(kBS.IsKeyDown(Keys.C)) {key = Keys.C; }
                if(kBS.IsKeyDown(Keys.D)) {key = Keys.D; }
                if(kBS.IsKeyDown(Keys.E)) {key = Keys.E; }
                if(kBS.IsKeyDown(Keys.F)) {key = Keys.F; }
                if(kBS.IsKeyDown(Keys.G)) {key = Keys.G; }
                if(kBS.IsKeyDown(Keys.H)) {key = Keys.H; }
                if(kBS.IsKeyDown(Keys.I)) {key = Keys.I; }
                if(kBS.IsKeyDown(Keys.J)) {key = Keys.J; }
                if(kBS.IsKeyDown(Keys.K)) {key = Keys.K; }
                if(kBS.IsKeyDown(Keys.L)) {key = Keys.L; }
                if(kBS.IsKeyDown(Keys.M)) {key = Keys.M; }
                if(kBS.IsKeyDown(Keys.N)) {key = Keys.N; }
                if(kBS.IsKeyDown(Keys.O)) {key = Keys.O; }
                if(kBS.IsKeyDown(Keys.P)) {key = Keys.P; }
                if(kBS.IsKeyDown(Keys.Q)) {key = Keys.Q; }
                if(kBS.IsKeyDown(Keys.R)) {key = Keys.R; }
                if(kBS.IsKeyDown(Keys.S)) {key = Keys.S; }
                if(kBS.IsKeyDown(Keys.T)) {key = Keys.T; }
                if(kBS.IsKeyDown(Keys.U)) {key = Keys.U; }
                if(kBS.IsKeyDown(Keys.V)) {key = Keys.V; }
                if(kBS.IsKeyDown(Keys.W)) {key = Keys.W; }
                if(kBS.IsKeyDown(Keys.X)) {key = Keys.X; }
                if(kBS.IsKeyDown(Keys.Y)) {key = Keys.Y; }
                if(kBS.IsKeyDown(Keys.D0)) {key = Keys.D0; }
                if(kBS.IsKeyDown(Keys.D1)) {key = Keys.D1; }
                if(kBS.IsKeyDown(Keys.D2)) {key = Keys.D2; }
                if(kBS.IsKeyDown(Keys.D3)) {key = Keys.D3; }
                if(kBS.IsKeyDown(Keys.D4)) {key = Keys.D4; }
                if(kBS.IsKeyDown(Keys.D5)) {key = Keys.D5; }
                if(kBS.IsKeyDown(Keys.D6)) {key = Keys.D6; }
                if(kBS.IsKeyDown(Keys.D7)) {key = Keys.D7; }
                if(kBS.IsKeyDown(Keys.D8)) {key = Keys.D8; }
                if(kBS.IsKeyDown(Keys.D9)) {key = Keys.D9; }
                if(kBS.IsKeyDown(Keys.NumPad0)) {key = Keys.NumPad0; }
                if(kBS.IsKeyDown(Keys.NumPad1)) {key = Keys.NumPad1; }
                if(kBS.IsKeyDown(Keys.NumPad2)) {key = Keys.NumPad2; }
                if(kBS.IsKeyDown(Keys.NumPad3)) {key = Keys.NumPad3; }
                if(kBS.IsKeyDown(Keys.NumPad4)) {key = Keys.NumPad4; }
                if(kBS.IsKeyDown(Keys.NumPad5)) {key = Keys.NumPad5; }
                if(kBS.IsKeyDown(Keys.NumPad6)) {key = Keys.NumPad6; }
                if(kBS.IsKeyDown(Keys.NumPad7)) {key = Keys.NumPad7; }
                if(kBS.IsKeyDown(Keys.NumPad8)) {key = Keys.NumPad8; }
                if(kBS.IsKeyDown(Keys.D9)) {key = Keys.D9; }
                if(kBS.IsKeyDown(Keys.Up)) {key = Keys.Up; }
                if(kBS.IsKeyDown(Keys.Down)) {key = Keys.Down; }
                if(kBS.IsKeyDown(Keys.Left)) {key = Keys.Left; }
                if(kBS.IsKeyDown(Keys.Right)) {key = Keys.Right; }
                if (key != Keys.None){
                    // if(m_currentSelection == Selection.UP) {m_controls[0] = key; m_enterNewKey = false;}
                    // else if(m_currentSelection == Selection.DOWN) {m_controls[1] = key; m_enterNewKey = false;}
                    // else if(m_currentSelection == Selection.LEFT) {m_controls[2] = key; m_enterNewKey = false;}
                    // else if(m_currentSelection == Selection.RIGHT) {m_controls[3] = key; m_enterNewKey = false;}
                    // else if(m_currentSelection == Selection.SHOOT) {m_controls[4] = key; m_enterNewKey = false;}
                    updateControlStringsWithInput();
                }

            }else{

                // Arrow keys to navigate the menu
                if (kBS.IsKeyUp(Keys.Down) && oldKBS.IsKeyDown(Keys.Down) && m_currentSelection != Selection.QUIT)
                {
                    m_currentSelection = m_currentSelection + 1;
                }
                if (kBS.IsKeyUp(Keys.Up) && oldKBS.IsKeyDown(Keys.Up) && m_currentSelection != Selection.UP)
                {
                    m_currentSelection = m_currentSelection - 1;
                }
                
                // If enter is pressed, return the appropriate new state
                if (kBS.IsKeyUp(Keys.Enter) && oldKBS.IsKeyDown(Keys.Enter) && m_currentSelection == Selection.UP){
                    m_enterNewKey = true;
                    oldKBS = kBS;
                    return GameStateEnum.Controls;
                }
                if (kBS.IsKeyUp(Keys.Enter) && oldKBS.IsKeyDown(Keys.Enter) && m_currentSelection == Selection.DOWN){
                    m_enterNewKey = true;
                    oldKBS = kBS;
                    return GameStateEnum.Controls;
                }
                if (kBS.IsKeyUp(Keys.Enter) && oldKBS.IsKeyDown(Keys.Enter) && m_currentSelection == Selection.LEFT){
                    m_enterNewKey = true;
                    oldKBS = kBS;
                    return GameStateEnum.Controls;
                }
                if (kBS.IsKeyUp(Keys.Enter) && oldKBS.IsKeyDown(Keys.Enter) && m_currentSelection == Selection.RIGHT){
                    m_enterNewKey = true;
                    oldKBS = kBS;
                    return GameStateEnum.Controls;
                }
                if (kBS.IsKeyUp(Keys.Enter) && oldKBS.IsKeyDown(Keys.Enter) && m_currentSelection == Selection.SHOOT){
                    m_enterNewKey = true;
                    oldKBS = kBS;
                    return GameStateEnum.Controls;
                }
                if (kBS.IsKeyUp(Keys.Enter) && oldKBS.IsKeyDown(Keys.Enter) && m_currentSelection == Selection.QUIT){
                    oldKBS = kBS;
                    return GameStateEnum.MainMenu;
                }
                if (kBS.IsKeyUp(Keys.Escape) && oldKBS.IsKeyDown(Keys.Escape)){
                    oldKBS = kBS;
                    return GameStateEnum.MainMenu;
                }
            }
            
            oldKBS = kBS;
            return GameStateEnum.Controls;
        }

        public override void update(GameTime gameTime){
        }
        public override void render(GameTime gameTime){
            if (m_enterNewKey == true){
                m_spriteBatch.Begin();

                Vector2 stringSize = m_font.MeasureString(MESSAGE);
                m_spriteBatch.DrawString(m_font, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.Yellow);

                m_spriteBatch.End();
            }else{

                m_spriteBatch.Begin();

                
                Printer p = new Printer();
                // Vector2 stringSize1 = m_fontTitle.MeasureString(Title);
                // Vector2 stringSize = new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize1.X / 2, 100);
                // p.printWithOutline(Title, m_graphics, m_spriteBatch, stringSize, m_fontTitle, Color.Green, Color.White);

                float bottom = drawMenuItem(m_currentSelection == Selection.UP ? m_fontMenuSelect : m_fontMenu, controlStringsWithInput[0], m_graphics.PreferredBackBufferWidth/4, m_currentSelection == Selection.UP ? Color.Black : Color.Blue);
                bottom = drawMenuItem(m_currentSelection == Selection.DOWN ? m_fontMenuSelect : m_fontMenu, controlStringsWithInput[1], bottom, m_currentSelection == Selection.DOWN ? Color.Black : Color.Blue);
                bottom = drawMenuItem(m_currentSelection == Selection.LEFT ? m_fontMenuSelect : m_fontMenu, controlStringsWithInput[2], bottom, m_currentSelection == Selection.LEFT ? Color.Black : Color.Blue);
                bottom = drawMenuItem(m_currentSelection == Selection.RIGHT ? m_fontMenuSelect : m_fontMenu, controlStringsWithInput[3], bottom, m_currentSelection == Selection.RIGHT ? Color.Black : Color.Blue);
                bottom = drawMenuItem(m_currentSelection == Selection.SHOOT ? m_fontMenuSelect : m_fontMenu, controlStringsWithInput[4], bottom, m_currentSelection == Selection.SHOOT ? Color.Black : Color.Blue);
                drawMenuItem(m_currentSelection == Selection.QUIT ? m_fontMenuSelect : m_fontMenu, controlStringsWithInput[5], bottom, m_currentSelection == Selection.QUIT ? Color.Black : Color.Blue);

                m_spriteBatch.End();
            }
        }

        private float drawMenuItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize1 = font.MeasureString(text);
            Vector2 stringSize = new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize1.X / 2, y);
            Printer.PrintWithOutline(text, m_graphics, m_spriteBatch, stringSize, font, color, Color.White);
            return y + stringSize1.Y;
        }

        private void updateControlStringsWithInput(){
            controlStringsWithInput.Clear(); 
            controlStringsWithInput.Add(controlStrings[0] + ": " + m_controls[0].ToString()); 
            controlStringsWithInput.Add(controlStrings[1] + ": " + m_controls[1].ToString()); 
            controlStringsWithInput.Add(controlStrings[2] + ": " + m_controls[2].ToString()); 
            controlStringsWithInput.Add(controlStrings[3] + ": " + m_controls[3].ToString()); 
            controlStringsWithInput.Add(controlStrings[4] + ": " + m_controls[4].ToString()); 
            controlStringsWithInput.Add(controlStrings[5]); 
        }

        // private Keys  (Keyboard kBS) {

        // }
    }
}
