using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
        // private Player m_player;
        private Texture2D m_texture;
        private ContentManager m_contentManager;
        public List<Keys> m_inUseControls = new List<Keys> {Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space};
        public Grid grid;
        public bool gameOver = false;
        public int gridXOffset = 510; // number of pixels from the left side of the screen to where the grid starts
        public Random rnd = new Random();
        public int millisecondsToWait = 100;
        public int moveTimer = 0;
        // private bool saving = false;
        public int currentLevel = 1;
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
            grid = new Grid(currentLevel);
            m_font = contentManager.Load<SpriteFont>("Fonts/gameFont");
            m_texture = new Texture2D(m_graphics.GraphicsDevice, 1, 1);
            m_texture.SetData(new Color[] { Color.White});
            Renderer.loadSprites(contentManager);

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
            wipeRules();        //reset the rules
            findRules();       // finds the rules
            


        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();


            grid.renderLevel(m_graphics, m_spriteBatch, m_font);


            m_spriteBatch.End();
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
            if (grid.m_grid[t.X-1][t.Y].things.Count > 0 && grid.m_grid[t.X+1][t.Y].things.Count > 0){
                if (text.Contains(grid.m_grid[t.X-1][t.Y].things[0].m_name) && text.Contains(grid.m_grid[t.X+1][t.Y].things[0].m_name)) {
                    Char obj = grid.m_grid[t.X-1][t.Y].things[0].m_name;
                    Char txt = grid.m_grid[t.X+1][t.Y].things[0].m_name;
                    Console.WriteLine("this happens");
                    if (txt == 'Y') performRules(you, obj);
                    if (txt == 'X') performRules(win, obj);
                    if (txt == 'P') performRules(push, obj);
                    if (txt == 'S') performRules(stop, obj);
                    if (txt == 'N') performRules(sink, obj);
                    if (txt == 'K') performRules(kill, obj);
                        // reorganizes lists according to the rules
                }
            }
            if (grid.m_grid[t.X][t.Y-1].things.Count > 0 && grid.m_grid[t.X][t.Y+1].things.Count > 0){
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
            moveTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(moveTimer > millisecondsToWait){
                foreach(Thing y in you){
                    bool canMove = true;
                    foreach(Thing s in stop){
                        if (s.X == y.X && s.Y == y.Y-1) canMove = false; 
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
                    foreach(Thing p in push){
                        if (p.X == y.X && p.Y == y.Y+1) {
                            canMove = canBePushed(p, y, 6);
                        }
                    }
                    if (canMove){
                        grid.m_grid[y.X][y.Y].things.Remove(y);
                        grid.m_grid[y.X][y.Y+1].things.Add(y);
                        y.Y = y.Y+1;
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
                        if (s.X == y.X-1&& s.Y == y.Y) canMove = false; 
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
                    foreach(Thing p in push){
                        if (p.X == y.X+1 && p.Y == y.Y) {
                            canMove = canBePushed(p, y, 3);
                        }
                    }
                    if (canMove){
                        grid.m_grid[y.X][y.Y].things.Remove(y);
                        grid.m_grid[y.X+1][y.Y].things.Add(y);
                        y.X = y.X+1;
                    }
                }
                moveTimer = moveTimer % millisecondsToWait;
            }
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
