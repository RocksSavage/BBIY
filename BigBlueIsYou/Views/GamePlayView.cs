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
        private Texture2D m_spriteSheet;
        // private Player m_player;
        private Texture2D m_texture;
        private ContentManager m_contentManager;
        private int score;
        private int lives;
        public List<Keys> m_inUseControls = new List<Keys> {Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space};
        public Grid grid;
        public bool gameOver = false;
        public int gridXOffset = 510; // number of pixels from the left side of the screen to where the grid starts
        // public Centipede centipede;
        // public FleaHelper fleaHelper;
        // public ScorpionHelper scorpionHelper;
        // public SpiderHelper spiderHelper;
        public int shootTimer;
        public int newFleaTimer = 29000;
        public int newScorpionTimer = 40000;
        public int newSpiderTimer = 25000;
        public Random rnd = new Random();
        public int poisonMushroomRenderOffset = 170;
        private bool saving = false;        
        private Song lazerSound;
        private Song playerExplosionSound;
        private Song kickSound;
        private int animatedFleaHelper;
        private int animatedFleaHelper2;
        private int animatedCentipedeHelper;
        private int animatedCentipedeHelper2;
        private int animatedSpiderHelper;
        private int animatedSpiderHelper2;
        private int animatedScorpionHelper;
        private int animatedScorpionHelper2;




        public override void loadContent(ContentManager contentManager)
        {
            // Content.RootDirectory = "Content";
            m_contentManager = contentManager;
            // m_player = new Player(m_graphics.GraphicsDevice.Viewport.Width/2, m_graphics.GraphicsDevice.Viewport.Height/4*3);
            // m_spriteSheet = contentManager.Load<Texture2D>("Images/spriteSheet");
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            lives = 3;
            score = 0;            
            m_texture = new Texture2D(m_graphics.GraphicsDevice, 1, 1);
            m_texture.SetData(new Color[] { Color.White});
            int gridSize = 28;
            grid = new Grid(gridSize);
            // centipede = new Centipede();
            // fleaHelper = new FleaHelper();
            // scorpionHelper = new ScorpionHelper();
            // spiderHelper = new SpiderHelper();
            // newFleaTimer = 29000;
            // newScorpionTimer = 40000;
            // newSpiderTimer = 25000;
            // lazerSound = contentManager.Load<Song>("Audio/Lazer");
            // playerExplosionSound = contentManager.Load<Song>("Audio/explosion");
            // kickSound = contentManager.Load<Song>("Audio/kick");
           

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
            // makeNewSpider(gameTime);
            // moveSpider(gameTime, 1);
            // makeNewScorpion(gameTime);
            // moveScorpion(gameTime, 1);
            // makeNewFlea(gameTime);
            // moveFlea(gameTime, 1);
            // makeNewCentipede();
            // moveCentipede(gameTime, 1);
            // grid.checkMushrooms();
            // moveLazers(gameTime, 1);


        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // drawMushrooms();
            // drawFlea(gameTime);
            // drawScorpion(gameTime);
            // drawSpider(gameTime);
            // drawCentipede(gameTime);
            // drawPlayer();
            // drawLazers();
            // drawScore();
            // drawLives();


            m_spriteBatch.End();
        }

        // public void makeNewSpider(GameTime gameTime){
        //     newSpiderTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        //     if (newSpiderTimer > 30000 && spiderHelper.spiderList.Count < 1){
        //         spiderHelper.makeSpider();
        //         newSpiderTimer = shootTimer % 30000;
        //     }
        // }
        // public void makeNewScorpion(GameTime gameTime){
        //     newScorpionTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        //     if (newScorpionTimer > 40000){
        //         scorpionHelper.makeScorpion();
        //         newScorpionTimer = shootTimer % 40000;
        //     }
        // }
        // public void makeNewFlea(GameTime gameTime){
        //     newFleaTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        //     if (newFleaTimer > 30000){
        //         fleaHelper.makeFlea();
        //         newFleaTimer = shootTimer % 30000;
        //     }
        // }

        // public void makeNewCentipede(){
        //     if(centipede.segmentList.Count == 0){
        //         centipede.makeCentipede();
        //     }
        // }

        public bool isPathClear(Rectangle square1, Rectangle square2){ // detect for collisions
            if (square1.X       < square2.X      + square2.Width     &&
                square1.X       + square1.Width  > square2.X         &&
                square1.Y       < square2.Y      + square2.Height    &&
                square1.Y       + square1.Height > square2.Y)        {
                return false;                                               // collision
            } else {
                return true;                                                // no collision
            }
        }
        // public void moveScorpion(GameTime gameTime, float scale){
        //     bool timeToKillScorpion = false;
        //     foreach (Scorpion s in scorpionHelper.scorpionList) {
        //         if (s.body.X < m_graphics.GraphicsDevice.Viewport.Width - gridXOffset){
        //             int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * s.scorpionSpeed * scale);
        //             s.body.X = Math.Max(s.body.X + moveDistance, 0);
        //         }else{ timeToKillScorpion = true;}
        //         if (s.body.Y % 30 == 0 && s.body.Y < m_graphics.GraphicsDevice.Viewport.Height-200){
        //             // if (rnd.Next(100) < 20){
        //             //     grid.addMushroomToMushroomList(s.body.X, s.body.Y);
        //             // }
        //         }
        //         foreach(Mushroom m in grid.m_mushroomList){
        //            if(isPathClear(m.body, s.body)){
        //                continue;
        //            }else{
        //                m.isPoisoned = true;
        //            }
        //         }
        //         if (isPathClear(s.body, m_player.body)){ continue; }
        //         else{ 
        //             m_player.body.X = m_graphics.GraphicsDevice.Viewport.Width/2;
        //             m_player.body.Y = m_graphics.GraphicsDevice.Viewport.Height/4*3;
        //             lives -= 1;
        //             MediaPlayer.Play(playerExplosionSound);
        //             if(lives == 0){
        //                 gameOver = true;
        //             }
        //         }
        //     }
        //     if (timeToKillScorpion == true){
        //         scorpionHelper.scorpionList.Remove(scorpionHelper.scorpionList[0]);
        //     }
        // }
        // public void moveFlea(GameTime gameTime, float scale){
        //     bool timeToKillFlea = false;
        //     foreach (Flea f in fleaHelper.fleaList) {
        //         if (f.body.Y < m_graphics.GraphicsDevice.Viewport.Height){
        //             int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * f.fleaSpeed * scale);
        //             f.body.Y = Math.Max(f.body.Y + moveDistance, 0);
        //         }else{ timeToKillFlea = true;}
        //         if (f.body.Y % 30 == 0 && f.body.Y < m_graphics.GraphicsDevice.Viewport.Height-200){
        //             if (rnd.Next(100) < 20){
        //                 grid.addMushroomToMushroomList(f.body.X, f.body.Y);
        //             }
        //         }
        //         if (isPathClear(f.body, m_player.body)){ continue; }
        //         else{ 
        //             m_player.body.X = m_graphics.GraphicsDevice.Viewport.Width/2;
        //             m_player.body.Y = m_graphics.GraphicsDevice.Viewport.Height/4*3;
        //             lives -= 1;
        //             MediaPlayer.Play(playerExplosionSound);
        //             if(lives == 0){
        //                 gameOver = true;
        //             }
        //         }
        //     }
        //     if (timeToKillFlea == true){
        //         fleaHelper.fleaList.Remove(fleaHelper.fleaList[0]);
        //     }
        // }
        // public void moveSpider(GameTime gameTime, float scale){
        //     if (spiderHelper.spiderList.Count > 0){
        //         foreach (Spider s in spiderHelper.spiderList) {
        //             int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * s.spiderSpeed * scale);
        //             if (s.direction == 10){ // move up to the left
        //                 s.body.X = Math.Max(s.body.X - moveDistance, 0);
        //                 s.body.Y = Math.Max(s.body.Y - moveDistance, 0); 
        //             }
        //             if (s.direction == 2){ // move up to the left
        //                 s.body.X = Math.Max(s.body.X + moveDistance, 0);
        //                 s.body.Y = Math.Max(s.body.Y - moveDistance, 0);
        //             }
        //             if (s.direction == 6){ // move down
        //                 s.body.Y = Math.Max(s.body.Y + moveDistance*2, 0);
        //             }
        //             if (s.body.X < gridXOffset + 5){
        //                 s.movingLeft = false;
        //             }
        //             if (s.body.X > m_graphics.GraphicsDevice.Viewport.Width - gridXOffset -5){
        //                 s.movingLeft = true;
        //             }
        //             if (s.body.Y < m_graphics.GraphicsDevice.Viewport.Height - 500){
        //                     s.direction = 6;
        //             }
        //             if (s.body.Y > m_graphics.GraphicsDevice.Viewport.Height - 100 || s.body.X > m_graphics.GraphicsDevice.Viewport.Width - gridXOffset -5 || s.body.X < gridXOffset + 5){
        //                 if(s.movingLeft){
        //                     s.direction = 10;
        //                 } else {
        //                     s.direction = 2;
        //                 }
        //             }
        //             if (isPathClear(s.body, m_player.body)){ continue; }
        //             else{                                                                           // player dies
        //                 m_player.body.X = m_graphics.GraphicsDevice.Viewport.Width/2;
        //                 m_player.body.Y = m_graphics.GraphicsDevice.Viewport.Height/4*3;
        //                 lives -= 1;
        //                 MediaPlayer.Play(playerExplosionSound);
        //                 if(lives == 0){
        //                     saveHighScores();
        //                     gameOver = true;
        //                 }
        //             }
        //         }
        //     }
        // }
        // public void moveCentipede(GameTime gameTime, float scale){
        //     foreach (Segment s in centipede.segmentList) {
        //         // Console.WriteLine("s.Y" + s.body.Y);
        //         if (s.direction == 6 && s.body.Y < m_graphics.GraphicsDevice.Viewport.Height){ // move down
        //             int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * s.centipedeSpeed * scale);
        //             s.body.Y = Math.Max(s.body.Y + moveDistance, 0);
        //         }
        //         if (s.direction == 9 && s.body.X > gridXOffset){ // move left
        //             int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * s.centipedeSpeed * scale);
        //             bool danger = false;
        //             foreach(Mushroom m in grid.m_mushroomList){
        //                 if (isPathClear(m.body, s.body)){ continue; }
        //                 else{ 
        //                     danger = true;
        //                     break;
        //                 }
        //             }
        //             if(danger){
        //                 // turn centipede down 
        //                 s.lastDirection = s.direction;
        //                 s.direction = 6;
        //                 s.body.Y = Math.Max(s.body.Y + 1, 0);
        //             }else{
        //                 s.body.X = Math.Max(s.body.X - moveDistance, 0);
        //             }
        //         }
        //         if (s.direction == 3 && s.body.X < m_graphics.GraphicsDevice.Viewport.Width - gridXOffset){ // move right
        //             int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * s.centipedeSpeed * scale);
        //             bool danger = false;
        //             foreach(Mushroom m in grid.m_mushroomList){
        //                 if (isPathClear(m.body, s.body)){ continue; }
        //                 else{ 
        //                     if(m.isPoisoned){
        //                         Console.WriteLine("centipede touched a poisoned mushroom");

        //                     }else{
        //                         danger = true;
        //                     }
        //                     break;
        //                 }
        //             }
        //             if(danger){
        //                 // turn centipede down 
        //                 s.lastDirection = s.direction;
        //                 s.direction = 6;
        //                 s.body.Y = Math.Max(s.body.Y + 1, 0);
        //             }else{
        //                 s.body.X = Math.Max(s.body.X + moveDistance, 0);
        //             }
                    
        //         }
        //         if (s.direction == 6 && (s.body.Y % 30) == 0){ // when the centipede goes down far enough turn it right
        //             if(s.lastDirection == 9){
        //                 s.direction = 3;
        //             }
        //             else if(s.lastDirection == 3){              // or turn it left
        //                 s.direction = 9;
        //             }
        //         }
        //         if (s.direction == 9 && s.body.X <= gridXOffset){ // when the centipede hits the left wall turn it down
        //             s.lastDirection = s.direction;
        //             s.direction = 6;
        //             s.body.Y = Math.Max(s.body.Y + 1, 0);
        //         }
        //         if (s.direction == 3 && s.body.X >= m_graphics.GraphicsDevice.Viewport.Width - gridXOffset){ // // when the centipede hits the right wall turn it down
        //             s.lastDirection = s.direction;
        //             s.direction = 6;
        //             s.body.Y = Math.Max(s.body.Y + 1, 0);
        //         }
        //         if (isPathClear(s.body, m_player.body)){ continue; }
        //         else{ 
        //             m_player.body.X = m_graphics.GraphicsDevice.Viewport.Width/2;
        //             m_player.body.Y = m_graphics.GraphicsDevice.Viewport.Height/4*3;
        //             lives -= 1;
        //             MediaPlayer.Play(playerExplosionSound);
        //             if(lives == 0){
        //                 gameOver = true;
        //             }
        //         }
        //     }
        // }
        // public void moveLazers(GameTime gameTime, float scale){
        //     List<Lazer> deadShots = new List<Lazer>();
        //     List<Segment> deadSegments = new List<Segment>();
        //     List<Flea> deadFlea = new List<Flea>();
        //     List<Scorpion> deadScorpion = new List<Scorpion>();
        //     List<Spider> deadSpider = new List<Spider>();
        //     foreach (Lazer l in m_player.shots){
        //         if (l.body.Y > l.body.Height){
        //             int moveDistance = (int)(gameTime.ElapsedGameTime.TotalMilliseconds * l.lazerSpeed * scale);
        //             l.body.Y = Math.Max(l.body.Y - moveDistance, 0);
        //         } else {
        //             deadShots.Add(l);
        //         }
        //         foreach(Mushroom m in grid.m_mushroomList){
        //             if(isPathClear(m.body, l.body)){
        //                 continue;
        //             }else{
        //                 m.health--;
        //                 deadShots.Add(l);
        //                 if(m.health == 0){
        //                     MediaPlayer.Play(kickSound);
        //                     score += 4;
        //                 }
        //             }
        //         }
        //         foreach(Scorpion s in scorpionHelper.scorpionList){
        //             if(isPathClear(s.body, l.body)){
        //                 continue;
        //             }else{
        //                 deadScorpion.Add(s);
        //                 deadShots.Add(l);
        //                 MediaPlayer.Play(kickSound);
        //                 score += 200;
        //             }
        //         }
        //         foreach(Flea f in fleaHelper.fleaList){
        //             if(isPathClear(f.body, l.body)){
        //                 continue;
        //             }else{
        //                 deadFlea.Add(f);
        //                 deadShots.Add(l);
        //                 MediaPlayer.Play(kickSound);
        //                 score += 200;
        //             }
        //         }
        //         foreach(Spider s in spiderHelper.spiderList){
        //             if(isPathClear(s.body, l.body)){
        //                 continue;
        //             }else{
        //                 deadSpider.Add(s);
        //                 deadShots.Add(l);
        //                 MediaPlayer.Play(kickSound);
        //                 score += 600;
        //             }
        //         }
        //         bool nextIsHead = false;
        //         foreach (Segment s in centipede.segmentList){
        //             if(nextIsHead){
        //                 s.isHead = true;
        //                 nextIsHead = false;
        //             }
        //             if(isPathClear(s.body, l.body)){
        //                 continue;
        //             }else{
        //                 deadSegments.Add(s);
        //                 deadShots.Add(l);
        //                 MediaPlayer.Play(kickSound);
        //                 nextIsHead = true;
        //                 if(s.isHead){ 
        //                     score += 100; 
        //                 }else{ 
        //                     score += 10;
        //                 }
        //             }
        //         }
        //     }

        //     foreach (Lazer l in deadShots){
        //         m_player.shots.Remove(l);
        //     }
        //     foreach (Flea f in deadFlea){
        //         fleaHelper.fleaList.Remove(f);
        //     }
        //     foreach (Scorpion s in deadScorpion){
        //         scorpionHelper.scorpionList.Remove(s);
        //     }
        //     foreach (Spider s in deadSpider){
        //         spiderHelper.spiderList.Remove(s);
        //     }
        //     foreach (Segment s in deadSegments){
        //         centipede.segmentList.Remove(s);
        //         grid.addMushroomToMushroomList(s.body.X, s.body.Y);
        //     }
        // }

        // public void drawLives(){
        //     Vector2 stringSize1 = m_font.MeasureString(score.ToString());
        //     Printer p = new Printer();
        //     Vector2 stringSize = new Vector2(200, 350);
        //     p.printWithOutline("Lives: ", m_graphics, m_spriteBatch, stringSize, m_font, Color.Green, Color.White);
        //     for(int i = 0; i < lives; i++){
        //         m_spriteBatch.Draw( m_spriteSheet, new Rectangle(325 + i*30, 355, 40, 40), new Rectangle(0, 2, 15, 14), Color.White);
        //     }
        // }
        // public void drawScore(){
        //     Vector2 stringSize1 = m_font.MeasureString(score.ToString());
        //     Printer p = new Printer();
        //     Vector2 stringSize = new Vector2(200, 400);
        //     p.printWithOutline("Score: " + score.ToString(), m_graphics, m_spriteBatch, stringSize, m_font, Color.Green, Color.White);
        // }

        // public void drawFlea(GameTime gameTime){
        //     foreach(Flea f in fleaHelper.fleaList){
        //             m_spriteBatch.Draw( m_spriteSheet, f.body, new Rectangle(3+(f.pixelWidth*animatedFleaHelper), 63, 10, 8 ), Color.White);
        //     }
        //     animatedFleaHelper2 += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        //     if (animatedFleaHelper2 % 100 == 0){
        //         animatedFleaHelper2 = 0;
        //         animatedFleaHelper++;
        //         if(animatedFleaHelper%4 == 0){
        //             animatedFleaHelper = 0;
        //         }
        //     }
        // }
        // public void drawScorpion(GameTime gameTime){
        //     foreach(Scorpion s in scorpionHelper.scorpionList){
        //             m_spriteBatch.Draw( m_spriteSheet, s.body, new Rectangle(0+(s.pixelWidth*animatedScorpionHelper), 72, 16, 8 ), Color.White);
        //     }
        //     animatedScorpionHelper2 += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        //     if (animatedScorpionHelper2 % 30 == 0){
        //         animatedScorpionHelper2 = 0;
        //         animatedScorpionHelper++;
        //         if(animatedScorpionHelper%4 == 0){
        //             animatedScorpionHelper = 0;
        //         }
        //     }
        // }
        // public void drawSpider(GameTime gameTime){
        //     foreach(Spider s in spiderHelper.spiderList){
        //             m_spriteBatch.Draw( m_spriteSheet, s.body, new Rectangle(0+(s.pixelWidth*animatedSpiderHelper), 54, 16, 8 ), Color.White);
        //     }
        //     animatedSpiderHelper2 += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        //     if (animatedSpiderHelper2 % 50 == 0){
        //         animatedSpiderHelper2 = 0;
        //         animatedSpiderHelper++;
        //         if(animatedSpiderHelper%8 == 0){
        //             animatedSpiderHelper = 0;
        //         }
        //     }
        // }
        // public void drawCentipede(GameTime gameTime){
        //     foreach(Segment s in centipede.segmentList){
        //         if(s.isHead){
        //             if (s.direction == 3){
        //                 m_spriteBatch.Draw( m_spriteSheet, s.body, new Rectangle(3+(s.pixelWidth*animatedCentipedeHelper)+8, 18, -8, 8 ), Color.White);
        //             }else if (s.direction == 9){
        //                 m_spriteBatch.Draw( m_spriteSheet, s.body, new Rectangle(3+(s.pixelWidth*animatedCentipedeHelper), 18, 8, 8 ), Color.White);
        //             }else if (s.direction == 6){
        //                 m_spriteBatch.Draw( m_spriteSheet, s.body, new Rectangle(3+(s.pixelWidth*(2+(int)animatedCentipedeHelper/2)), 18+9, 9, 8 ), Color.White);
        //             }
        //         }else{
        //             if (s.direction == 3){
        //                 m_spriteBatch.Draw( m_spriteSheet, s.body, new Rectangle(3+(s.pixelWidth*animatedCentipedeHelper)+8, 36, -8, 8 ), Color.White);
        //             }else if (s.direction == 9){
        //                 m_spriteBatch.Draw( m_spriteSheet, s.body, new Rectangle(3+(s.pixelWidth*animatedCentipedeHelper), 36, 8, 8 ), Color.White);
        //             }else if (s.direction == 6){
        //                 m_spriteBatch.Draw( m_spriteSheet, s.body, new Rectangle(3+(s.pixelWidth*(2+(int)animatedCentipedeHelper/2)), 36+9, 9, 8 ), Color.White);
        //             }
        //         }
        //     }
        //     animatedCentipedeHelper2 += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        //     if (animatedCentipedeHelper2 % 40 == 0){
        //         animatedCentipedeHelper2 = 0;
        //         animatedCentipedeHelper++;
        //         if(animatedCentipedeHelper % 8 == 0){
        //             animatedCentipedeHelper = 0;
        //         }
        //     }
        // }
        // public void drawMushrooms(){
        //     foreach(Mushroom m in grid.m_mushroomList){
        //         if(m.health == 4){
        //             m_spriteBatch.Draw( m_spriteSheet, m.body, new Rectangle(68 + (m.isPoisoned ? poisonMushroomRenderOffset : 0), 81, 8, 8 ), Color.White);
        //         }
        //         if(m.health == 3){
        //             m_spriteBatch.Draw( m_spriteSheet, m.body, new Rectangle(77 + (m.isPoisoned ? poisonMushroomRenderOffset : 0), 81, 8, 8 ), Color.White);
        //         }
        //         if(m.health == 2){
        //             m_spriteBatch.Draw( m_spriteSheet, m.body, new Rectangle(86 + (m.isPoisoned ? poisonMushroomRenderOffset : 0), 81, 8, 8 ), Color.White);
        //         }
        //         if(m.health == 1){
        //             m_spriteBatch.Draw( m_spriteSheet, m.body, new Rectangle(95 + (m.isPoisoned ? poisonMushroomRenderOffset : 0), 81, 8, 8 ), Color.White);
        //         }
        //     }
        // }
        // public void drawLazers(){
        //     foreach (Lazer shot in m_player.shots){
        //         m_spriteBatch.Draw( m_spriteSheet, new Rectangle(shot.body.X-2 , shot.body.Y - m_player.size, shot.body.Width, shot.body.Height), new Rectangle(18, 0, 8, 8 ), Color.White);
        //     }
        // }
        // public void drawPlayer(){
        //     m_spriteBatch.Draw( m_spriteSheet, m_player.body, new Rectangle(0, 2, 15, 14), Color.White);
        //     //  m_spriteSheet, m_player.body, null, Color.White, 0, new Vector2(m_spaceship.Width / 2, m_spaceship.Height / 2), SpriteEffects.None, 0
        // }

        public void updateControls (){
            m_inputKeyboard.removeCommand(m_inUseControls[0]);
            m_inputKeyboard.removeCommand(m_inUseControls[1]);
            m_inputKeyboard.removeCommand(m_inUseControls[2]);
            m_inputKeyboard.removeCommand(m_inUseControls[3]);
            m_inputKeyboard.removeCommand(m_inUseControls[4]);
            m_inputKeyboard.registerCommand(m_controls[0], false, new InputDeviceHelper.CommandDelegate(onMoveUp));
            m_inputKeyboard.registerCommand(m_controls[1], false, new InputDeviceHelper.CommandDelegate(onMoveDown));
            m_inputKeyboard.registerCommand(m_controls[2], false, new InputDeviceHelper.CommandDelegate(onMoveLeft));
            m_inputKeyboard.registerCommand(m_controls[3], false, new InputDeviceHelper.CommandDelegate(onMoveRight));
            m_inputKeyboard.registerCommand(m_controls[4], false, new InputDeviceHelper.CommandDelegate(onShoot));
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
        public void onShoot(GameTime gameTime, float scale){
        //     shootTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        //     if (shootTimer > 100){
        //         m_player.shoot(m_player.body.X + m_player.body.Width/2, m_player.body.Y + m_player.body.Height);
        //         // lazerSound.Play();
        //         MediaPlayer.Play(lazerSound);
        //         shootTimer = shootTimer % 100;
        //     }
        }
        // private void saveHighScores()
        // {
        //     lock (this)
        //     {
        //         if (!this.saving)
        //         {
        //             this.saving = true;
        //             //
        //             // Create something to save
        //             // GameState myState = new GameState(100000, 20);
                    
        //             m_highScores.Add(score);
        //             m_highScores.Sort();
        //             m_highScores.Reverse();
        //             finalizeSaveAsync(m_highScores);
        //         }
        //     }
        // }
        // private async void finalizeSaveAsync(List<int> m_highScores)
        // {
        //     await Task.Run(() =>
        //     {
        //         using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
        //         {
        //             try
        //             {
        //                 using (IsolatedStorageFileStream fs = storage.OpenFile("HighScores.xml", FileMode.OpenOrCreate))
        //                 {
        //                     if (fs != null)
        //                     {
        //                         XmlSerializer mySerializer = new XmlSerializer(typeof(List<int>));
        //                         mySerializer.Serialize(fs, m_highScores);
        //                     }
        //                 }
        //             }
        //             catch (IsolatedStorageException)
        //             {
        //                 // Ideally show something to the user, but this is demo code :)
        //             }
        //         }

        //         this.saving = false;
        //     });
        // }
    }
}
