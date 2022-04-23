using CS5410.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CS5410
{
    public class Grid {
        public int gameStep=99;
        public int m_size { get; set; }
        public int gridSizeAdjust;
        public int gridXOffset = 510;
        public int m_X;
        public int m_Y;
        public List<List<Cell>> m_grid;
        public Random rnd = new Random();
        public int m_currentLevel;
        public Renderer m_renderer;
        // use this file if working in visual studio--------------------------------------------------------------------------------------
            public string[] levels = System.IO.File.ReadAllLines("../../../Levels/levelSource/levels-all.bbiy");

        // use this file if working in VSCode--------------------------------------------------------------------------------------
        //public string[] levels = System.IO.File.ReadAllLines("./Levels/levelSource/levels-all.bbiy");


        public Grid(int currentLevel, int gameStep, GraphicsDeviceManager m_graphics){
            this.gameStep = gameStep;
            m_grid = new List<List<Cell>>();
            m_currentLevel = currentLevel;
            makeLevel(m_currentLevel);
            gridSizeAdjust = m_graphics.PreferredBackBufferHeight/24;

        }
        public Grid(int gameStep,
                    int m_size,
                    int gridSizeAdjust,
                    int gridXOffset,
                    int m_X,
                    int m_Y,
                    List<List<Cell>> m_grid,
                    int m_currentLevel,
                    Renderer m_renderer)
        {
            this.gameStep = gameStep;
            this.m_size = m_size;
            this.gridSizeAdjust = gridSizeAdjust;
            this.gridXOffset = gridXOffset;
            this.m_X = m_X;
            this.m_Y = m_Y;
            this.m_grid = m_grid;
            this.m_currentLevel = m_currentLevel;
            this.m_renderer = m_renderer;
        }

        public Grid getDeepClone(){
            return new Grid(this.gameStep,
                    this.m_size,
                    this.gridSizeAdjust,
                    this.gridXOffset,
                    this.m_X,
                    this.m_Y,
                    this.m_grid.Select(list => list.Select(cell => cell.getDeepCopy()).ToList()).ToList(), // needs to deep copy cells in the list!!!
                    this.m_currentLevel,
                    this.m_renderer); // keep object (as if it was a singleton)
        }

        public void makeLevel(int currentLevel){
            m_currentLevel = currentLevel;
            string[] level = levels;
            int levelOffset = 42;
            level = level.Skip(levelOffset * (currentLevel-1)).Take(levelOffset).ToArray();
            string[] size = level[1].Split(' ');
            m_X = int.Parse(size[0]);
            m_Y = int.Parse(size[2]);
            makeGrid(level);
        }
        public void makeGrid(string[] level){
            for (int i = 2; i < m_X+2; i++){
                List<Cell> col = new List<Cell>();
                for (int j = 0; j < m_Y; j++){
                    Cell cell = new Cell(j, i);
                    if(level[i][j] != ' '){                // adds background letters like shrubs grass
                        cell.things.Add(new Thing(level[i][j], i-2, j)); 
                    }
                    if(level[i+20][j] != ' '){             // adds foreground letters like bb is you rock skull ice flag
                        cell.things.Add(new Thing(level[i+20][j], i-2, j));
                    }
                    col.Add(cell);
                }
                m_grid.Add(col);
            }
        }
        public void printGrid(){
            foreach (List<Cell> col in m_grid){
                foreach(Cell c in col){
                    Console.Write((c.things.Count() > 0) ? c.things[0].m_name.ToString() : " ");
                }
                Console.WriteLine("");
            }
        }
        public void renderLevel(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont font, int gameStep){

            foreach (List<Cell> col in m_grid){
                foreach(Cell c in col){
                    if(c.things.Count > 0){
                        foreach(Thing t in c.things){
                            Renderer.PrintThing(t, c, gameStep, spriteBatch, font);
                        }
                    }
                }
            }
        }
    }
    public class Cell{

        public List<Thing> things = new List<Thing>();

        public Point coord;
        public int X
        {
            get => coord.X;
            set => coord.X = value;
        }
        public int Y
        {
            get => coord.Y;
            set => coord.Y = value;
        }
        public Cell(int x, int y){
            coord = new Point(x, y); // this is kind of horrible, but it *might* ensure a deep copy is made?
        }
        
        public override string ToString(){
            // return "x:"+ X+ " y:"+ Y + "  " + ((things.Count() > 0) ? things[0].ToString() : "") + "  ";
            return ((things.Count() > 0) ? things[0].ToString() + "    " : "     ");
        }
        public Cell getDeepCopy()
        {
            var dcpy = new Cell(this.X, this.Y);
            dcpy.things = this.things.ToList();
            return dcpy;
        }
    }
    public class Thing{

        public Char m_name;
        public int X { get; set; }
        public int Y { get; set; }
        // public Rectangle body;

        public Thing(Char name, int x, int y){
            m_name = name;
            X = x;
            Y = y;
        }
        // public Thing(int x, int y, int h){
        //     body = new Rectangle(x, y, mushroomSize, mushroomSize);
        //     // health = h;

        // }
        
        public override string ToString(){
            return "X" + X + " Y" + Y + " " + m_name.ToString();
        }
    }
}