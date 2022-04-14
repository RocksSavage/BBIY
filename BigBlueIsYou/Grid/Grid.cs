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
        public int m_size { get; set; }
        public int gridSizeAdjust = 30;
        public int gridXOffset = 510;
        public int m_X;
        public int m_Y;
        public List<List<Cell>> m_grid;
        public Random rnd = new Random();
        public int m_currentLevel;
        public Renderer m_renderer;
        // use these five file lines if working in visual studio--------------------------------------------------------------------------------------
        //  public string[] level1 = System.IO.File.ReadAllLines("../../../Levels/levelSource/level-1.bbiy");
        // public string[] level2 = System.IO.File.ReadAllLines("../../../Levels/levelSource/level-2.bbiy");
        // public string[] level3 = System.IO.File.ReadAllLines("../../../Levels/levelSource/level-3.bbiy");
        // public string[] level4 = System.IO.File.ReadAllLines("../../../Levels/levelSource/level-4.bbiy");
        // public string[] level5 = System.IO.File.ReadAllLines("../../../Levels/levelSource/level-5.bbiy");
         public string[] levels = System.IO.File.ReadAllLines("../../../Levels/levelSource/levels-all.bbiy");

        // use these five file lines if working in VSCode--------------------------------------------------------------------------------------
        // public string[] level1 = System.IO.File.ReadAllLines("./Levels/levelSource/level-1.bbiy");
        // public string[] level2 = System.IO.File.ReadAllLines("./Levels/levelSource/level-2.bbiy");
        // public string[] level3 = System.IO.File.ReadAllLines("./Levels/levelSource/level-3.bbiy");
        // public string[] level4 = System.IO.File.ReadAllLines("./Levels/levelSource/level-4.bbiy");
        // public string[] level5 = System.IO.File.ReadAllLines("./Levels/levelSource/level-5.bbiy");
        //public string[] levels = System.IO.File.ReadAllLines("./Levels/levelSource/levels-all.bbiy");


        public Grid(int currentLevel){
            m_grid = new List<List<Cell>>();
            m_currentLevel = currentLevel;
            makeLevel(m_currentLevel);

        }
        public Grid(Grid grid){
            m_grid = grid.m_grid;

        }

        public Grid Clone(){
            return (Grid)this.MemberwiseClone();
        }

        public void makeLevel(int currentLevel){
            Console.WriteLine("currentLevel" + currentLevel);
            m_currentLevel = currentLevel;
            string[] level = levels;
            int levelOffset = 42;
            level = level.Skip(levelOffset * (currentLevel-1)).Take(levelOffset).ToArray();
            // if (m_currentLevel == 2) level = level2;
            // if (m_currentLevel == 3) level = level3;
            // if (m_currentLevel == 4) level = level4;
            // if (m_currentLevel == 5) level = level5;
            string[] size = level[1].Split(' ');
            // Console.WriteLine("size " + size);
            // Console.WriteLine("size [0]" + size[0]);
            // Console.WriteLine("size [2]" + size[2]);
            m_X = int.Parse(size[0]);
            m_Y = int.Parse(size[2]);
            makeGrid(level);
        }
        public void makeGrid(string[] level){
            for (int i = 2; i < m_X+2; i++){
                List<Cell> col = new List<Cell>();
                for (int j = 0; j < m_Y; j++){
                    Cell cell = new Cell(i, j);
                    if(level[i][j] != ' '){                // adds background letters like shrubs grass
                        // Console.WriteLine("i* 1 " + i*m_currentLevel + m_currentLevel);
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
        public int X { get; set; }
        public int Y { get; set; }
        public Cell(int x, int y){
            coord.X = x;
            coord.Y = y;
            X = x;
            Y = y;
        }
        
        public override string ToString(){
            // return "x:"+ X+ " y:"+ Y + "  " + ((things.Count() > 0) ? things[0].ToString() : "") + "  ";
            return ((things.Count() > 0) ? things[0].ToString() + "    " : "     ");
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