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
        // public string[] levels = System.IO.File.ReadAllLines("../../../Levels/levelSource/levels-all.bbiy");

        // use these five file lines if working in VSCode--------------------------------------------------------------------------------------
        public string[] levels = System.IO.File.ReadAllLines("./Levels/levelSource/levels-all.bbiy");


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
                    Cell cell = new Cell(i, j);
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