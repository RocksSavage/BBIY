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
        public List<Mushroom> m_mushroomList = new List<Mushroom>();
        public Random rnd = new Random();
        public int m_currentLevel;
        public string[] level1 = System.IO.File.ReadAllLines("C:/Programming/CS Homework/CS5410 Game Development/HW 4 Big Blue is You/BBIY/BigBlueIsYou/Levels/levelSource/level-1.bbiy");
        public string[] level2 = System.IO.File.ReadAllLines("C:/Programming/CS Homework/CS5410 Game Development/HW 4 Big Blue is You/BBIY/BigBlueIsYou/Levels/levelSource/level-2.bbiy");
        public string[] level3 = System.IO.File.ReadAllLines("C:/Programming/CS Homework/CS5410 Game Development/HW 4 Big Blue is You/BBIY/BigBlueIsYou/Levels/levelSource/level-3.bbiy");
        public string[] level4 = System.IO.File.ReadAllLines("C:/Programming/CS Homework/CS5410 Game Development/HW 4 Big Blue is You/BBIY/BigBlueIsYou/Levels/levelSource/level-4.bbiy");
        public string[] level5 = System.IO.File.ReadAllLines("C:/Programming/CS Homework/CS5410 Game Development/HW 4 Big Blue is You/BBIY/BigBlueIsYou/Levels/levelSource/level-5.bbiy");


        public Grid(int currentLevel){
            m_grid = new List<List<Cell>>();
            m_currentLevel = currentLevel;
            makeLevel(m_currentLevel);

        }

        public void makeLevel(int m_currentLevel){
            string[] size = level1[1].Split(' ');
            m_X = int.Parse(size[0]);
            m_Y = int.Parse(size[2]);
            makeGrid();
        }
        public void makeGrid(){
            for (int i = 2; i < m_X+2; i++){
                List<Cell> row = new List<Cell>();
                for (int j = 0; j < m_Y; j++){
                    Cell cell = new Cell(i, j);
                    if(level1[i][j] != ' '){                // adds background letters like shrubs grass
                        cell.things.Add(level1[i][j]); 
                    }
                    if(level1[i+20][j] != ' '){             // adds foreground letters like bb is you rock skull ice flag
                        cell.things.Add(level1[i+20][j]);
                    }
                    row.Add(cell);
                }
                m_grid.Add(row);
            } 
        }
        public void renderLevel(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont font){

            foreach (List<Cell> r in m_grid){
                foreach(Cell c in r){
                    if(c.things.Count > 0){
                        foreach(Char ch in c.things){
                            String s = Char.ToString(ch);
                            Vector2 stringSize1 = font.MeasureString(s);
                            // Vector2 stringSize = new Vector2(graphics.PreferredBackBufferWidth / 2 - stringSize1.X / 2, graphics.PreferredBackBufferHeight/8);
                            Vector2 stringSize = new Vector2(c.Y*30, c.X*30);
                            Printer.PrintWithOutline(s, graphics, spriteBatch, stringSize, font, Color.Green, Color.White);
                        }
                    }
                }
            }
        }
        // public void checkMushrooms(){
        //     List<Mushroom> deadMushrooms = new List<Mushroom>();
        //     foreach (Mushroom m in m_mushroomList){
        //         if (m.health == 0){
        //             deadMushrooms.Add(m);
        //         }
        //     }
        //     foreach (Mushroom m in deadMushrooms){
        //         m_mushroomList.Remove(m);
        //     }
        // }
        // public void makeMushroomList(){
        //     for(int r = 0; r < m_size; r++){
        //         for(int c = 0; c < m_size; c++){
        //             if (m_grid[r][c].mushroom != null){
        //                 if(m_grid[r][c].mushroom.health > 0){
        //                     m_mushroomList.Add(m_grid[r][c].mushroom);
        //                 }
        //             }
        //         }
        //     }
        // }
        // public void addMushroomToMushroomList(int x, int y){
        //     m_mushroomList.Add(new Mushroom(x, y));
        // }
    }
    public class Cell{

        public Mushroom mushroom { get; set; }
        public List<char> things = new List<char>();

        public int X { get; set; }
        public int Y { get; set; }
        public Cell(int x, int y){
            X = x;
            Y = y;
        }
        
        public override string ToString(){
            return "x:"+ X+ "y:"+ Y;
        }
    }
    public class Mushroom{

        public int health = 4;
        public int mushroomSize = 30;
        public Rectangle body;
        public bool isPoisoned = false;

        public Mushroom(int x, int y){
            body = new Rectangle(x, y, mushroomSize, mushroomSize);

        }
        public Mushroom(int x, int y, int h){
            body = new Rectangle(x, y, mushroomSize, mushroomSize);
            health = h;

        }
        
        public override string ToString(){
            return "x:"+ body.X+ "y:"+ body.Y;
        }
    }
}