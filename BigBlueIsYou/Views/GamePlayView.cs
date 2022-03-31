using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CS5410.Input;
using System;

namespace CS5410
{
    public class GamePlayView : GameStateView
    {
        private KeyboardState kBS;
        private KeyboardState oldKBS;
        private SpriteFont m_font;
        // private Player m_player;
        private Texture2D m_texture;
        private ContentManager m_contentManager;
        public List<Keys> m_inUseControls = new List<Keys> {Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space};
        public Grid grid;
        public bool gameOver = false;
        public int gridXOffset = 510; // number of pixels from the left side of the screen to where the grid starts
        public Random rnd = new Random();
        // private bool saving = false;
        public int currentLevel = 1;



        public override void loadContent(ContentManager contentManager)
        {
            m_contentManager = contentManager;
            grid = new Grid(currentLevel);
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");  
            m_texture = new Texture2D(m_graphics.GraphicsDevice, 1, 1);
            m_texture.SetData(new Color[] { Color.White});
           

            updateControls();

        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if(m_inUseControls[0] != m_controls[0] || m_inUseControls[1] != m_controls[1] || m_inUseControls[2] != m_controls[2] || m_inUseControls[3] != m_controls[3] || m_inUseControls[4] != m_controls[4]) {
                updateControls();
            }
        
            kBS = Keyboard.GetState();

            if (kBS.IsKeyUp(Keys.Escape) && oldKBS.IsKeyDown(Keys.Escape))
            {
                oldKBS = kBS;
                loadContent(m_contentManager);
                return GameStateEnum.MainMenu;
            }

            m_inputKeyboard.Update(gameTime);
            if (gameOver){
                loadContent(m_contentManager);
                gameOver = false;
                return GameStateEnum.GameOver;
            }

            oldKBS = kBS;
            return GameStateEnum.GamePlay;
        }
        public override void update(GameTime gameTime)
        {


        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();


            grid.renderLevel(m_graphics, m_spriteBatch, m_font);


            m_spriteBatch.End();
        }

        // public bool isPathClear(Rectangle square1, Rectangle square2){ // detect for collisions
        //     if (square1.X       < square2.X      + square2.Width     &&
        //         square1.X       + square1.Width  > square2.X         &&
        //         square1.Y       < square2.Y      + square2.Height    &&
        //         square1.Y       + square1.Height > square2.Y)        {
        //         return false;                                               // collision
        //     } else {
        //         return true;                                                // no collision
        //     }
        // }

        public void updateControls (){
            m_inputKeyboard.removeCommand(m_inUseControls[0]);
            m_inputKeyboard.removeCommand(m_inUseControls[1]);
            m_inputKeyboard.removeCommand(m_inUseControls[2]);
            m_inputKeyboard.removeCommand(m_inUseControls[3]);
            m_inputKeyboard.registerCommand(m_controls[0], false, new InputDeviceHelper.CommandDelegate(onMoveUp));
            m_inputKeyboard.registerCommand(m_controls[1], false, new InputDeviceHelper.CommandDelegate(onMoveDown));
            m_inputKeyboard.registerCommand(m_controls[2], false, new InputDeviceHelper.CommandDelegate(onMoveLeft));
            m_inputKeyboard.registerCommand(m_controls[3], false, new InputDeviceHelper.CommandDelegate(onMoveRight));
        }

        public void onMoveUp(GameTime gameTime, float scale){
            // if (m_player.body.Top > m_graphics.GraphicsDevice.Viewport.Height / 2){
            // if (m_player.body.Top > 5){
            //     int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * m_player.SPRITE_MOVE_PIXELS_PER_MS * scale);
            //     Rectangle temp = m_player.body;
            //     temp.Y = Math.Max(m_player.body.Y - moveDistance, 0);
            //     foreach(Mushroom m in grid.m_mushroomList){
            //         if (isPathClear(m.body, temp)){ continue; }
            //         else{ return; }
            //     }
            //     m_player.body.Y = temp.Y;
            // }
        }

        public void onMoveDown(GameTime gameTime, float scale){
            // if ((m_player.body.Bottom) < m_graphics.GraphicsDevice.Viewport.Height - m_player.body.Height){
            //     int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * m_player.SPRITE_MOVE_PIXELS_PER_MS * scale);
            //     Rectangle temp = m_player.body;
            //     temp.Y = Math.Min(m_player.body.Y + moveDistance, m_graphics.GraphicsDevice.Viewport.Height);
            //     foreach(Mushroom m in grid.m_mushroomList){
            //         if (isPathClear(m.body, temp)){ continue; }
            //         else{ return; }
            //     }
            //     m_player.body.Y = temp.Y;
            // }
        }

        public void onMoveLeft(GameTime gameTime, float scale){
            // if ((m_player.body.Left) > 510){
            //     int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * m_player.SPRITE_MOVE_PIXELS_PER_MS * scale);
            //     Rectangle temp = m_player.body;
            //     temp.X = Math.Max(m_player.body.X - moveDistance, 0);
            //     foreach(Mushroom m in grid.m_mushroomList){
            //         if (isPathClear(m.body, temp)){ continue; }
            //         else{ return; }
            //     }
            //     m_player.body.X = temp.X;
            // }
        }

        public void onMoveRight(GameTime gameTime, float scale){
            // if ((m_player.body.Right) < m_graphics.GraphicsDevice.Viewport.Width - 510){
            //     int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * m_player.SPRITE_MOVE_PIXELS_PER_MS * scale);
            //     Rectangle temp = m_player.body;
            //     temp.X = Math.Min(m_player.body.X + moveDistance, m_graphics.GraphicsDevice.Viewport.Width);
            //     foreach(Mushroom m in grid.m_mushroomList){
            //         if (isPathClear(m.body, temp)){ continue; }
            //         else{ return; }
            //     }
            //     m_player.body.X = temp.X;
            // }
        }
    }
}
