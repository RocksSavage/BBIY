using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;
using CS5410.Input;
using System;

namespace CS5410
{
    public class GamePlayView : GameStateView
    {
        private KeyboardState kBS;
        private KeyboardState oldKBS;
        private SpriteFont m_font;
        private Texture2D m_texture;
        private ContentManager m_contentManager;
        public List<Keys> m_inUseControls = new List<Keys> {Keys.Up, Keys.Down, Keys.Left, Keys.Right};
        public Grid grid;
        public bool YouWin = false;
        public int gridXOffset = 510; // number of pixels from the left side of the screen to where the grid starts
        public Random rnd = new Random();
        public int millisecondsToWait = 100;
        public int moveTimer = 0;
        public bool youWin = false;
        public int currentLevel = 1;
        public int gameStep = 0;
        private SoundEffect m_moveSound;
        private bool canPlayMoveSound;
        private SoundEffect winChangedSound;
        private Song m_music;
        private bool musicIsPlaying = false;
        private Stack<Grid> gridStack;
        private bool reload = true;
        // private ParticleSystem particleSystem;
        private Texture2D fire;
        private Char lastYou = ' ';
        private Char lastWin = ' ';
        // private bool sparkleYou = false;
        private bool moved = false;
        private int screenSizeOffset;
        private Renderer renderer = new Renderer();

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
            gameStep = 0;
            grid = new Grid(currentLevel, gameStep, m_graphics);
            gridStack = new Stack<Grid>();
            gridStack.Push(grid.getDeepClone()); // always remember original state
            m_font = contentManager.Load<SpriteFont>("Fonts/gameFont");
            m_texture = new Texture2D(m_graphics.GraphicsDevice, 1, 1);
            m_texture.SetData(new Color[] { Color.White});
            renderer.loadContent(contentManager, m_graphics);
            screenSizeOffset = m_graphics.PreferredBackBufferHeight/24;
            
            fire = contentManager.Load<Texture2D>("Images/fire");
            ParticleSystem particleSystem = new ParticleSystem();
            
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
            if (kBS.IsKeyUp(Keys.Z) && oldKBS.IsKeyDown(Keys.Z)) // undo
            {
                oldKBS = kBS;
                // On 'z' press, restore previous game state (not the current state, which is stored on the top of the stack at all times
                if (gridStack.Count > 1) // always keep the first state
                    gridStack.Pop();
                grid = gridStack.Peek().getDeepClone();
                update(gameTime); // force call to update to re-hydrate all the rule-related lists
                Debug.Write($"Popped Grid Stack r:{gridStack.Count}. Order:");
                Debug.Write("Order: ");
                foreach (Grid grid in gridStack)
                {
                    Debug.Write(grid.gameStep.ToString() + ", ");
                }
                Debug.Write("\n");

                ParticleSystem.endParticle();
            }

            m_inputKeyboard.Update(gameTime);
            
            if (youWin){
                reload = true;
                youWin = false;
                musicIsPlaying = false;
                loadContent(m_contentManager);
                return GameStateEnum.YouWin;
            }
            canPlayMoveSound = true;
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
            if (moved) {
                gameStep++; // sprite animations rely on this
                this.grid.gameStep = gameStep;
                gridStack.Push(grid.getDeepClone());
                moved = false;
            }
            checkWin();
            ParticleSystem.update(gameTime);
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            grid.renderLevel(m_graphics, m_spriteBatch, m_font, gameStep);
            ParticleSystem.drawParticles(m_spriteBatch);


            m_spriteBatch.End();
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
                if (!text.Contains(p.m_name)) {
                    foreach(Thing s in sink){
                        if (p.X == s.X && p.Y == s.Y) {
                            thingsToDelete.Add(p);
                            thingsToDelete.Add(s);
                        }
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
                ParticleSystem.makeDeathPartiles((t.Y+2)*screenSizeOffset+(screenSizeOffset/2), (t.X+2)*screenSizeOffset+(screenSizeOffset/2), fire);
                grid.m_grid[t.X][t.Y].things.Remove(t);
                push.Remove(t);
                you.Remove(t);
                sink.Remove(t);
            }

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
                    // "if there is a horizontal rule / sentence"
                    char obj = grid.m_grid[t.X-1][t.Y].things[0].m_name;
                    char txt = grid.m_grid[t.X+1][t.Y].things[0].m_name;
                    executeRules(obj, txt);
                        // reorganizes lists according to the rules
                }
            }
            if (grid.m_grid[t.X][t.Y-1].things.Count > 0 && grid.m_grid[t.X][t.Y+1].things.Count > 0){ // checks for vertical rules
                // Console.WriteLine("thing1" + grid.m_grid[t.X][t.Y-1].things[0].m_name);
                // Console.WriteLine("thing2" + grid.m_grid[t.X][t.Y+1].things[0].m_name);
                if (text.Contains(grid.m_grid[t.X][t.Y-1].things[0].m_name) && text.Contains(grid.m_grid[t.X][t.Y+1].things[0].m_name)) {
                    char obj = grid.m_grid[t.X][t.Y-1].things[0].m_name;
                    char txt = grid.m_grid[t.X][t.Y+1].things[0].m_name;
                    executeRules(obj, txt);
                }
            }
            performRules(stop, 'h'); // hedge is stop
            foreach (Char c in text){
                performRules(push, c); // all text blocks are pushable
            }
            bool lavaNotKill = false;
            bool waterNotKill = false;
            foreach (Thing y in you) {
                if (y.m_name == 'v'){
                    lavaNotKill = true;
                }
                if ( y.m_name == 'a'){
                    waterNotKill = true;
                }
            }
            foreach (Thing w in win) {
                if (w.m_name == 'v'){
                    lavaNotKill = true;
                }
                if (w.m_name == 'a'){
                    waterNotKill = true;
                }
            }
            foreach (Thing p in push) {
                if (p.m_name == 'v'){
                    lavaNotKill = true;
                }
                if (p.m_name == 'a'){
                    waterNotKill = true;
                }
            }
            foreach (Thing s in stop) {
                if (s.m_name == 'v'){
                    lavaNotKill = true;
                }
                if (s.m_name == 'a'){
                    waterNotKill = true;
                }
            }
            if (!lavaNotKill) {
                performRules(kill, 'v'); // lava is kill
            }
            if (!waterNotKill) {
                performRules(kill, 'a'); // lava is kill
            }


            if(you.Count > 0){
                if(lastYou == ' '){
                    lastYou = you[0].m_name;
                
                }
                if (you[0].m_name != lastYou){
                    foreach(Thing y in you){
                        ParticleSystem.makePartilesAroundThing(y.Y, y.X, m_graphics.PreferredBackBufferHeight/24, fire);
                    }
                    lastYou = you[0].m_name;
                }
            }

            if(win.Count > 0){
                if(lastWin == ' '){
                    lastWin = win[0].m_name;
                
                }
                if (win[0].m_name != lastWin){
                    winChangedSound.Play();
                    foreach(Thing w in win){
                        ParticleSystem.makePartilesAroundThing( w.Y, w.X, m_graphics.PreferredBackBufferHeight/24, fire);
                    }
                    lastWin = win[0].m_name;
                }
            }

        }
        void executeRules(char obj, char txt)
        {
            // NOUN IS NOUN case
            if (objects.Contains(Char.ToLower(txt)))
            {
                foreach (List<Cell> col in grid.m_grid)
                {
                    foreach (Cell c in col)
                    {
                        foreach (Thing thang in c.things)
                        {
                            if (thang.m_name == Char.ToLower(obj))
                            {
                                thang.m_name = Char.ToLower(txt);
                            }
                        }
                    }
                }
            }
            // NOUN IS VERB case
            if (txt == 'Y') performRules(you, Char.ToLower(obj));
            if (txt == 'X') performRules(win, Char.ToLower(obj));
            if (txt == 'P') performRules(push, Char.ToLower(obj));
            if (txt == 'S') performRules(stop, Char.ToLower(obj));
            if (txt == 'N') performRules(sink, Char.ToLower(obj));
            if (txt == 'K') performRules(kill, Char.ToLower(obj));
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
            foreach (List<Cell> col in grid.m_grid)
            {           // adds obj to list
                foreach (Cell c in col)
                {
                    foreach (Thing t in c.things)
                    {
                        if (t.m_name == obj && !rule.Contains(t))
                        {
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

        public void onMoveLeft(GameTime gameTime, float scale){
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
                            Debug.WriteLine("You Win!");
                        }
                    }
                    foreach(Thing p in push){
                        if (p.X == y.X && p.Y == y.Y-1) {
                            canMove = canBePushed(p, y, 12);
                        }
                    }
                    if (canMove){
                        grid.m_grid[y.X][y.Y].things.Remove(y);
                        grid.m_grid[y.X][y.Y-1].things.Add(new Thing(y.m_name,y.X,y.Y-1));
                        generalMove();
                        moved = true;
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
                        if (s.X == y.X && s.Y == y.Y+1) canMove = false; 
                    }
                    foreach(Thing w in win){
                        if (w.X == y.X && w.Y == y.Y+1){
                            youWin = true; 
                            Console.WriteLine("You Win!");
                        }
                    }
                    foreach(Thing p in push){
                        if (p.X == y.X && p.Y == y.Y+1) {
                            canMove = canBePushed(p, y, 6);
                        }
                    }
                    if (canMove){
                        grid.m_grid[y.X][y.Y].things.Remove(y);
                        grid.m_grid[y.X][y.Y+1].things.Add(new Thing(y.m_name, y.X,y.Y+1));
                        generalMove();
                        moved = true;
                    }
                }
                moveTimer = moveTimer % millisecondsToWait;
            }
        }

        public void onMoveUp(GameTime gameTime, float scale){
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
                            Debug.WriteLine("You Win!");
                        }
                    }
                    foreach(Thing p in push){
                        if (p.X == y.X-1 && p.Y == y.Y) {
                            canMove = canBePushed(p, y, 9);
                        }
                    }
                    if (canMove){
                        grid.m_grid[y.X][y.Y].things.Remove(y);
                        grid.m_grid[y.X-1][y.Y].things.Add(new Thing(y.m_name, y.X-1, y.Y));
                        generalMove();
                        moved = true;
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
                        if (s.X == y.X+1 && s.Y == y.Y) canMove = false; 
                    }
                    foreach(Thing w in win){
                        if (w.X == y.X+1 && w.Y == y.Y){
                            youWin = true; 
                            Debug.WriteLine("You Win!");
                        }
                    }
                    foreach(Thing p in push){
                        if (p.X == y.X+1 && p.Y == y.Y) {
                            canMove = canBePushed(p, y, 3);
                        }
                    }
                    if (canMove){
                        grid.m_grid[y.X][y.Y].things.Remove(y);
                        grid.m_grid[y.X+1][y.Y].things.Add(new Thing(y.m_name, y.X+1, y.Y));
                        generalMove();
                        moved = true;
                    }
                }
                moveTimer = moveTimer % millisecondsToWait;
            }
        }

        private void generalMove()
        {

            if (canPlayMoveSound){
                m_moveSound.Play();
                if (you.Count > 1){
                    canPlayMoveSound = false;
                }
            }
            ParticleSystem.endParticle();

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
                    if (grid.m_grid[pushed.X][pushed.Y].things.Remove(pushed))
                    {
                        grid.m_grid[pushed.X][pushed.Y - 1].things.Add(new Thing(pushed.m_name, pushed.X, pushed.Y - 1));
                    }
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
                    if (grid.m_grid[pushed.X][pushed.Y].things.Remove(pushed))
                    {
                        grid.m_grid[pushed.X][pushed.Y + 1].things.Add(new Thing(pushed.m_name, pushed.X, pushed.Y + 1));
                    }
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
                    if (grid.m_grid[pushed.X][pushed.Y].things.Remove(pushed))
                    {
                        grid.m_grid[pushed.X - 1][pushed.Y].things.Add(new Thing(pushed.m_name, pushed.X - 1, pushed.Y));
                    }
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
                    if (grid.m_grid[pushed.X][pushed.Y].things.Remove(pushed))
                    {
                        grid.m_grid[pushed.X + 1][pushed.Y].things.Add(new Thing(pushed.m_name, pushed.X + 1, pushed.Y));
                    }
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
