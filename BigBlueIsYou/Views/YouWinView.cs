using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
// using CS5410.Particles;
using System;
using System.Collections.Generic;

namespace CS5410
{
    public class YouWinView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "You Won!   --   Press Enter to return to main menu";
        private KeyboardState kBS;
        private KeyboardState oldKBS;
        private SoundEffect winSound;
        private bool winSoundCanPlay = true;
        private Texture2D fire;
        private List<ParticleEmitterLine> particles = new List<ParticleEmitterLine>();
        private ParticleEmitterLine m_emitter1;
        private MyRandom m_random = new MyRandom();

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            fire = contentManager.Load<Texture2D>("Images/fire");
            winSound = contentManager.Load<SoundEffect>("Audio/WinSound");
            winSoundCanPlay = true;
            int middleY = 10;
            for (int i = 0; i < 1000; i++){
                int middleX = (int)m_random.nextRange(0, 1900);
                int spawnTime = (int)m_random.nextRange(500, 10000);
                m_emitter1 = new ParticleEmitterLine(
                    fire,
                    new TimeSpan(0, 0, 0, 0, spawnTime),
                    middleX, middleY,
                    20,
                    -1,
                    new TimeSpan(0, 0, 4));
                particles.Add(m_emitter1);
            }
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
            foreach(ParticleEmitterLine pe in particles){
                pe.draw(m_spriteBatch);
            }


            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
            if(winSoundCanPlay){
                winSound.Play();
                winSoundCanPlay = false;
            }
            
            MediaPlayer.Stop();
            foreach(ParticleEmitterLine pe in particles){
                pe.update(gameTime);
            }
        }
    }
}
