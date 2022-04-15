﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;
using CS5410.Input;
using System;
using CS5410.Particles;

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
        public List<Keys> m_inUseControls = new List<Keys> {Keys.Up, Keys.Down, Keys.Left, Keys.Right};
        public Grid grid;
        // public Grid grid2;
        public bool YouWin = false;
        public int gridXOffset = 510; // number of pixels from the left side of the screen to where the grid starts
        public Random rnd = new Random();
        public int millisecondsToWait = 100;
        public int moveTimer = 0;
        // private bool saving = false;
        public bool youWin = false;
        public int currentLevel = 1;
        public int gameStep = 0;
        private SoundEffect m_moveSound;
        private SoundEffect winChangedSound;
        private Song m_music;
        private bool musicIsPlaying = false;
        private Stack<Grid> gridStack;
        private bool reload = true;
        private List<ParticleEmitterLine> particles;
        private List<ParticleEmitterFromCenter> particles2;
        private ParticleEmitterLine m_emitter1;
        private ParticleEmitterLine m_emitter2;
        private ParticleEmitterFromCenter m_emitter3;
        private Texture2D fire;
        private Char lastYou = ' ';
        private Char lastWin = ' ';
        // private bool sparkleYou = false;

        public List<Char> objects = new List<Char>(){'w', 'r', 'f', 'b', 'l', 'g', 'a', 'v', 'h'};
        public List<Char> text = new List<Char>(){'W', 'R', 'F', 'B', 'I', 'S', 'P', 'V', 'A', 'Y', 'X', 'N', 'K'};
        
        public List<Char> isYou = new List<Char>(); // Rule of what thing is you
        public List<Char> isWin = new List<Char>();
        public List<Char> isPush = new List<Char>();
        public List<Char> isStop = new List<Char>();
        public List<Char> isSink = new List<Char>();
        public List<Char> isKill = new List<Char>();
        //--------------------------------------------------------
        public List<Thing> you = new List<Thing>(); // things that are you
        public List<Thing> win = new List<Thing>();
        public List<Thing> push = new List<Thing>();
        public List<Thing> stop = new List<Thing>();
        public List<Thing> sink = new List<Thing>();
        public List<Thing> kill = new List<Thing>();



        public override void loadContent(ContentManager contentManager)
        {
            m_contentManager = contentManager;
            currentLevel = m_level[0];
            grid = new Grid(currentLevel);
            gridStack = new Stack<Grid>();
            gridStack.Push(grid.getDeepClone());
            m_font = contentManager.Load<SpriteFont>("Fonts/gameFont");
            m_texture = new Texture2D(m_graphics.GraphicsDevice, 1, 1);
            m_texture.SetData(new Color[] { Color.White});
            Renderer.loadSprites(contentManager);
            
            fire = contentManager.Load<Texture2D>("Images/fire");
            particles = new List<ParticleEmitterLine>();
            particles2 = new List<ParticleEmitterFromCenter>();
            
            // Audio
            winChangedSound = contentManager.Load<SoundEffect>("Audio/zelda-chest");
            m_moveSound = contentManager.Load<SoundEffect>("Audio/Step");
            m_music = contentManager.Load<Song>("Audio/Sara'sSong");
            musicIsPlaying = false; 


            updateControls();
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (reload) {
                loadContent(m_contentManager);
                reload = false;
            }
            if(m_inUseControls[0] != m_controls[0] || m_inUseControls[1] != m_controls[1] || m_inUseControls[2] != m_controls[2] || m_inUseControls[3] != m_controls[3]) {
                updateControls();
            }
        
            kBS = Keyboard.GetState();

            if (kBS.IsKeyUp(Keys.Escape) && oldKBS.IsKeyDown(Keys.Escape))
            {
                reload = true;
                oldKBS = kBS;
                loadContent(m_contentManager);
                return GameStateEnum.MainMenu;
            }
            if (kBS.IsKeyUp(Keys.R) && oldKBS.IsKeyDown(Keys.R))
            {
                oldKBS = kBS;
                loadContent(m_contentManager);
            }
            if (kBS.IsKeyUp(Keys.Z) && oldKBS.IsKeyDown(Keys.Z))
            {
                oldKBS = kBS;
                Grid bob = new Grid(currentLevel);
                if (gridStack.TryPop(out grid))
                {
                    Console.WriteLine("Popped the Grid Stack");

                }
            }

            m_inputKeyboard.Update(gameTime);
            if (youWin){
                reload = true;
                youWin = false;
                musicIsPlaying = false;
                loadContent(m_contentManager);
                return GameStateEnum.YouWin;
            }

            oldKBS = kBS;
            return GameStateEnum.GamePlay;
        }
        public override void update(GameTime gameTime)
        {
            if (!musicIsPlaying){
               MediaPlayer.Play(m_music);
                musicIsPlaying = true; 
            }
            wipeRules();        //reset the rules
            findRules();       // finds the rules
            checkKillAndSink();       // checks for things like you touching kill or falling in water or rocks falling in water
            checkWin();
            updateParticles(gameTime);
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            grid.renderLevel(m_graphics, m_spriteBatch, m_font, gameStep);
            drawParticles(m_spriteBatch);
            // grid2.renderLevel(m_graphics, m_spriteBatch, m_font, gameStep);


            m_spriteBatch.End();
        }
        public void emitSparklesYou(){
            foreach(Thing y in you){
                makePartilesAroundThing(y.X, y.Y);
            }
        }
        public void emitSparklesWin(){
            foreach(Thing w in win){
                makePartilesAroundThing(w.X, w.Y);
            }
        }
        public void drawParticles(SpriteBatch spriteBatch){
            foreach(ParticleEmitterLine pe in particles){ 
                pe.draw(m_spriteBatch);
            }
            foreach(ParticleEmitterFromCenter pe in particles2){ 
                pe.draw(m_spriteBatch);
            }

        }
        public void updateParticles(GameTime gameTime){
            foreach(ParticleEmitterLine pe in particles){ // foreach loop going through list of emmiters and updating them then I need to draw the particles with a render funciton
                pe.update(gameTime);
            }
            foreach(ParticleEmitterFromCenter pe in particles2){
                pe.update(gameTime);
            }
        }
        public void checkWin(){
            foreach(Thing y in you){
                foreach(Thing w in win){
                    if (w.X == y.X && w.Y == y.Y){
                        youWin = true; 
                        Console.WriteLine("You Win!");
                    }
                }
            }
        }

        public void checkKillAndSink(){
            List<Thing> thingsToDelete = new List<Thing>();
            foreach(Thing p in push){
                foreach(Thing s in sink){
                    if (p.X == s.X && p.Y == s.Y) {
                        thingsToDelete.Add(p);
                        thingsToDelete.Add(s);
                    }

                }
            }
            foreach(Thing y in you){
                foreach(Thing s in sink){
                    if (y.X == s.X && y.Y == s.Y) {
                        thingsToDelete.Add(s);
                        thingsToDelete.Add(y);
                    }

                }
            }
            foreach(Thing k in kill){
                foreach(Thing y in you){
                    if (y.X == k.X && y.Y == k.Y) {
                        thingsToDelete.Add(y);
                    }
                }
            }
            foreach(Thing t in thingsToDelete){
                makeDeathPartiles(t.X, t.Y);
                grid.m_grid[t.X][t.Y].things.Remove(t);
                push.Remove(t);
                you.Remove(t);
                sink.Remove(t);
            }

        }
                
        public void makeDeathPartiles(int x, int y){
            Console.WriteLine("Death!");
            m_emitter3 = new ParticleEmitterFromCenter(
                fire,
                new TimeSpan(0, 0, 0, 0, 5),
                (x+2)*30+15, (y+2)*30+15,
                15,
                1,
                new TimeSpan(0, 0, 0, 0, 150));
            particles2.Add(m_emitter3);
        }

        public void findRules(){
            foreach(List<Cell> col in grid.m_grid){
                foreach(Cell c in col){
                    foreach(Thing t in c.things){
                        if(t.m_name == 'I'){
                            checkForRules(t);
                        }
                    }
                }
            }
            // Console.WriteLine("check rules");
            // find all instances of is
            // check neighboring cells to see if rules exist
        }
        public void checkForRules(Thing t){
            if (grid.m_grid[t.X-1][t.Y].things.Count > 0 && grid.m_grid[t.X+1][t.Y].things.Count > 0){ // checks for horizontal rules
                // Console.WriteLine("thing1" + grid.m_grid[t.X-1][t.Y].things[0].m_name);
                // Console.WriteLine("thing2" + grid.m_grid[t.X+1][t.Y].things[0].m_name);
                if (text.Contains(grid.m_grid[t.X-1][t.Y].things[0].m_name) && text.Contains(grid.m_grid[t.X+1][t.Y].things[0].m_name)) {
                    Char obj = grid.m_grid[t.X-1][t.Y].things[0].m_name;
                    Char txt = grid.m_grid[t.X+1][t.Y].things[0].m_name;
                    if (txt == 'Y') performRules(you, Char.ToLower(obj));
                    if (txt == 'X') performRules(win, Char.ToLower(obj));
                    if (txt == 'P') performRules(push, Char.ToLower(obj));
                    if (txt == 'S') performRules(stop, Char.ToLower(obj));
                    if (txt == 'N') performRules(sink, Char.ToLower(obj));
                    if (txt == 'K') performRules(kill, Char.ToLower(obj));
                        // reorganizes lists according to the rules
                }
            }
            if (grid.m_grid[t.X][t.Y-1].things.Count > 0 && grid.m_grid[t.X][t.Y+1].things.Count > 0){ // checks for vertical rules
                // Console.WriteLine("thing1" + grid.m_grid[t.X][t.Y-1].things[0].m_name);
                // Console.WriteLine("thing2" + grid.m_grid[t.X][t.Y+1].things[0].m_name);
                if (text.Contains(grid.m_grid[t.X][t.Y-1].things[0].m_name) && text.Contains(grid.m_grid[t.X][t.Y+1].things[0].m_name)) {
                    Char obj = grid.m_grid[t.X][t.Y-1].things[0].m_name;
                    Char txt = grid.m_grid[t.X][t.Y+1].things[0].m_name;
                    if (txt == 'Y') performRules(you, Char.ToLower(obj));
                    if (txt == 'X') performRules(win, Char.ToLower(obj));
                    if (txt == 'P') performRules(push, Char.ToLower(obj));
                    if (txt == 'S') performRules(stop, Char.ToLower(obj));
                    if (txt == 'N') performRules(sink, Char.ToLower(obj));
                    if (txt == 'K') performRules(kill, Char.ToLower(obj));
                }
            }
            performRules(stop, 'h'); // hedge is stop
            foreach (Char c in text){
                performRules(push, c); // all text blocks are pushable
            }

            if(you.Count > 0){
                if(lastYou == ' '){
                    lastYou = you[0].m_name;
                
                }
                if (you[0].m_name != lastYou){
                    // Console.WriteLine("this is the place");
                    emitSparklesYou();
                    lastYou = you[0].m_name;
                }
            }

            if(win.Count > 0){
                if(lastWin == ' '){
                    lastWin = win[0].m_name;
                
                }
                if (win[0].m_name != lastWin){
                    // Console.WriteLine("this is the place");
                    winChangedSound.Play();
                    emitSparklesWin();
                    lastWin = win[0].m_name;
                }
            }

        }
        public void wipeRules(){
            you = new List<Thing>();
            win = new List<Thing>();
            push = new List<Thing>();
            stop = new List<Thing>();
            sink = new List<Thing>();
            kill = new List<Thing>();
        }
        public void performRules(List<Thing> rule, Char obj){
            foreach(List<Cell> col in grid.m_grid){           // adds obj to list
                foreach(Cell c in col){
                    foreach(Thing t in c.things){
                        if (t.m_name == obj){
                            rule.Add(t);
                        }
                    }
                }
            }
        }

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
            // grid.printGrid();
            moveTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(moveTimer > millisecondsToWait){
                foreach(Thing y in you){
                    bool canMove = true;
                    foreach(Thing s in stop){
                        if (s.X == y.X && s.Y == y.Y-1) canMove = false; 
                    }
                    foreach(Thing w in win){
                        if (w.X == y.X && w.Y == y.Y-1){
                            youWin = true; 
                            Console.WriteLine("You Win!");
                            // makeFireworks(w.X, w.Y);
                        }
                    }
                    foreach(Thing p in push){
                        if (p.X == y.X && p.Y == y.Y-1) {
                            canMove = canBePushed(p, y, 12);
                        }
                    }
                    if (canMove){
                        grid.m_grid[y.X][y.Y].things.Remove(y);
                        grid.m_grid[y.X][y.Y-1].things.Add(y);
                        y.Y = y.Y-1;
                        generalMove();
                        gridStack.Push(grid.getDeepClone());
                        // Console.WriteLine(gridStack.Count);
                    }
                }
                moveTimer = moveTimer % millisecondsToWait;
            }
        }

        public void onMoveDown(GameTime gameTime, float scale){
            moveTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(moveTimer > millisecondsToWait){
                foreach(Thing y in you){
                    bool canMove = true;
                    foreach(Thing s in stop){
                        if (s.X == y.X && s.Y == y.Y+1) canMove = false; 
                    }
                    foreach(Thing w in win){
                        if (w.X == y.X && w.Y == y.Y+1){
                            youWin = true; 
                            Console.WriteLine("You Win!");
                            // makeFireworks(w.X, w.Y);
                        }
                    }
                    foreach(Thing p in push){
                        if (p.X == y.X && p.Y == y.Y+1) {
                            canMove = canBePushed(p, y, 6);
                        }
                    }
                    if (canMove){
                        grid.m_grid[y.X][y.Y].things.Remove(y);
                        grid.m_grid[y.X][y.Y+1].things.Add(y);
                        y.Y = y.Y+1;
                        generalMove();
                        gridStack.Push(grid.getDeepClone());
                        // Console.WriteLine(gridStack.Count);
                    }
                }
                moveTimer = moveTimer % millisecondsToWait;
            }
        }

        public void onMoveLeft(GameTime gameTime, float scale){
            moveTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(moveTimer > millisecondsToWait){
                foreach(Thing y in you){
                    bool canMove = true;
                    foreach(Thing s in stop){
                        if (s.X == y.X-1 && s.Y == y.Y) canMove = false; 
                    }
                    foreach(Thing w in win){
                        if (w.X == y.X-1 && w.Y == y.Y){
                            youWin = true; 
                            Console.WriteLine("You Win!");
                            // makeFireworks(w.X, w.Y);
                        }
                    }
                    foreach(Thing p in push){
                        if (p.X == y.X-1 && p.Y == y.Y) {
                            canMove = canBePushed(p, y, 9);
                        }
                    }
                    if (canMove){
                        grid.m_grid[y.X][y.Y].things.Remove(y);
                        grid.m_grid[y.X-1][y.Y].things.Add(y);
                        y.X = y.X-1;
                        generalMove();
                        gridStack.Push(grid.getDeepClone());
                        // Console.WriteLine(gridStack.Count);
                    }
                }
                moveTimer = moveTimer % millisecondsToWait;
            }
        }

        public void onMoveRight(GameTime gameTime, float scale){
            moveTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(moveTimer > millisecondsToWait){
                foreach(Thing y in you){
                    bool canMove = true;
                    foreach(Thing s in stop){
                        if (s.X == y.X+1 && s.Y == y.Y) canMove = false; 
                    }
                    foreach(Thing w in win){
                        if (w.X == y.X+1 && w.Y == y.Y){
                            youWin = true; 
                            Console.WriteLine("You Win!");
                            // makeFireworks(w.X, w.Y);
                        }
                    }
                    foreach(Thing p in push){
                        if (p.X == y.X+1 && p.Y == y.Y) {
                            canMove = canBePushed(p, y, 3);
                        }
                    }
                    if (canMove){
                        grid.m_grid[y.X][y.Y].things.Remove(y);
                        grid.m_grid[y.X+1][y.Y].things.Add(y);
                        y.X = y.X+1;
                        generalMove();
                        gridStack.Push(grid.getDeepClone());
                        // Console.WriteLine(gridStack.Count);
                    }
                }
                moveTimer = moveTimer % millisecondsToWait;
            }
        }
        
        public void makePartilesAroundThing(int x, int y){
            Console.WriteLine("x, y: " + x + " " + y);
            m_emitter1 = new ParticleEmitterLine(
                fire,
                new TimeSpan(0, 0, 0, 0, 50),
                (x+2)*30, (y+2)*30+15,
                10,
                2,
                new TimeSpan(0, 0, 0, 0, 100));
            particles.Add(m_emitter1);
            m_emitter2 = new ParticleEmitterLine(
                fire,
                new TimeSpan(0, 0, 0, 0, 50),
                (x+3)*30, (y+2)*30+15,
                10,
                1,
                new TimeSpan(0, 0, 0, 0, 100));
            particles.Add(m_emitter2);
        }

        private void generalMove()
        {
            gameStep++; // sprite animations rely on this
            m_moveSound.Play();
            Console.WriteLine("when is this called?");
            particles.Clear();
            particles2.Clear();

        }
        public bool canBePushed(Thing pushed, Thing pusher, int direction){
            // if direction is out of bounds return false
            if (pushed == null) return true;
            if (stop.Contains(pushed)){
                return false;
            }
            if (!push.Contains(pushed)){
                return false;
            }
            Thing result = null;
            if (direction == 12) {
                foreach(Thing t in grid.m_grid[pushed.X][pushed.Y-1].things){
                    if (push.Contains(t)){
                        result = t;
                    }
                    if (stop.Contains(t)){
                        return false;
                    }
                }
                if (grid.m_grid[pushed.X][pushed.Y-1].things.Count == 0 || canBePushed(result, pushed, 12)){
                    grid.m_grid[pushed.X][pushed.Y].things.Remove(pushed);
                    grid.m_grid[pushed.X][pushed.Y-1].things.Add(pushed);
                    pushed.Y = pushed.Y-1;
                    return true;
                }
            }
            if (direction == 6) {
                foreach(Thing t in grid.m_grid[pushed.X][pushed.Y+1].things){
                    if (push.Contains(t)){
                        result = t;
                    }
                    if (stop.Contains(t)){
                        return false;
                    }
                }
                if (grid.m_grid[pushed.X][pushed.Y+1].things.Count == 0 || canBePushed(result, pushed, 6)){
                    grid.m_grid[pushed.X][pushed.Y].things.Remove(pushed);
                    grid.m_grid[pushed.X][pushed.Y+1].things.Add(pushed);
                    pushed.Y = pushed.Y+1;
                    return true;
                }
            }
            if (direction == 9) {
                foreach(Thing t in grid.m_grid[pushed.X-1][pushed.Y].things){
                    if (push.Contains(t)){
                        result = t;
                    }
                    if (stop.Contains(t)){
                        return false;
                    }
                }
                if (grid.m_grid[pushed.X-1][pushed.Y].things.Count == 0 || canBePushed(result, pushed, 9)){
                    grid.m_grid[pushed.X][pushed.Y].things.Remove(pushed);
                    grid.m_grid[pushed.X-1][pushed.Y].things.Add(pushed);
                    pushed.X = pushed.X-1;
                    return true;
                }
            }
            if (direction == 3) {
                foreach(Thing t in grid.m_grid[pushed.X+1][pushed.Y].things){
                    if (push.Contains(t)){
                        result = t;
                    }
                    if (stop.Contains(t)){
                        return false;
                    }
                }
                if (grid.m_grid[pushed.X+1][pushed.Y].things.Count == 0 || canBePushed(result, pushed, 3)){
                    grid.m_grid[pushed.X][pushed.Y].things.Remove(pushed);
                    grid.m_grid[pushed.X+1][pushed.Y].things.Add(pushed);
                    pushed.X = pushed.X+1;
                    return true;
                }
            }
            return false;
        }
    }
    public class Wait {
        public static void wait(){
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true) {
                if (stopwatch.ElapsedMilliseconds >= 200)
                {
                    break;
                }
            }
        }
    }
}
