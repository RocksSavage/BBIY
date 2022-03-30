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
        public List<List<Cell>> m_grid = new List<List<Cell>>();
        public List<Mushroom> m_mushroomList = new List<Mushroom>();
        public Random rnd = new Random();

        public Grid(int size){
            m_size = size;
            makeGrid();
            placeInitialMushrooms();
            makeMushroomList();

        }
        public void makeGrid(){
            for (int i = 0; i < m_size; i++){
                List<Cell> row = new List<Cell>();
                for (int j = 0; j < m_size; j++){
                    Cell cell = new Cell(i, j);
                    row.Add(cell);
                }
                m_grid.Add(row);
            } 
        }
        public void placeInitialMushrooms(){
            foreach (List<Cell> r in m_grid){
                foreach(Cell c in r.Skip(3)){
                    if(rnd.Next(100) < 2){
                        c.mushroom = new Mushroom(c.X * gridSizeAdjust + gridXOffset, c.Y * gridSizeAdjust, 4);
                    }
                }
            }
        }
        public void checkMushrooms(){
            List<Mushroom> deadMushrooms = new List<Mushroom>();
            foreach (Mushroom m in m_mushroomList){
                if (m.health == 0){
                    deadMushrooms.Add(m);
                }
            }
            foreach (Mushroom m in deadMushrooms){
                m_mushroomList.Remove(m);
            }
        }
        public void makeMushroomList(){
            for(int r = 0; r < m_size; r++){
                for(int c = 0; c < m_size; c++){
                    if (m_grid[r][c].mushroom != null){
                        if(m_grid[r][c].mushroom.health > 0){
                            m_mushroomList.Add(m_grid[r][c].mushroom);
                        }
                    }
                }
            }
        }
        public void addMushroomToMushroomList(int x, int y){
            m_mushroomList.Add(new Mushroom(x, y));
        }
    }
    public class Cell{

        public Mushroom mushroom { get; set; }

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