using System;
using System.Collections.Generic;
using TetrisConsole.Misc;
using System.Linq;

namespace TetrisConsole
{
    abstract class Shape
    {
        //private Color color
        public List<ShapeComponent> Components { get; }
        protected ShapeEnum Type;
        protected Direction Facing;
        private bool IsArrived { get; }

        public Shape()
        {
            Components = new List<ShapeComponent>();
            Facing = Direction.UP;
            IsArrived = false;
        }

        public abstract void RotateRight();

        public Point GetTopPos()
        {
            List<int> xs = new List<int>();
            int maxY = -1;
            foreach (ShapeComponent component in Components)
            {
                if(component.Y > maxY)
                {
                    maxY = component.Y;
                }
                xs.Add(component.Y);
            }
            Console.WriteLine("TopPos of + " + this + ": " + maxY);
            return new Point(xs.Sum() / xs.Count, maxY-1);
        }

        public void MoveShape(Direction d, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                foreach (ShapeComponent c in Components)
                {
                    c.Move(d);
                }
            }
        }
        public void MoveShape(Direction d)
        {
            MoveShape(d, 1);
        }

        public void MoveShapeTopTo(Point mapCenter)
        {
            Point ShapeTopPos = GetTopPos();
            int moveX = mapCenter.X - ShapeTopPos.X;
            int moveY = mapCenter.Y - ShapeTopPos.Y;

            if(ShapeTopPos.X < mapCenter.X)
            {
                MoveShape(Direction.RIGHT, moveX-1);
                Console.Write("Mozgatás jobbra " + (moveX-1) + " mezővel");
            } else if (ShapeTopPos.X > mapCenter.X)
            {
                MoveShape(Direction.LEFT, Math.Abs(moveX-1));
                Console.Write("Mozgatás Balra " + Math.Abs(moveX-1) + " mezővel");
            }


            if (ShapeTopPos.Y < mapCenter.Y)
            {
                MoveShape(Direction.UP, moveY-1);
                Console.Write("Mozgatás fel " + (moveY-1) + " mezővel");
            } else if (ShapeTopPos.Y > mapCenter.Y)
            {
                MoveShape(Direction.DOWN, Math.Abs(moveY-1));
                Console.Write("Mozgatás Le " + Math.Abs(moveY-1) + " mezővel");
            }
        }

        protected ShapeComponent GetComponentById(int id)
        {
            return Components.Find(elem => elem.id == id);
        }
        protected ShapeComponent GCBI(int id)
        {
            return GetComponentById(id);
        }

        public void drawToConsole()
        {
            bool[,] matrix = new bool[4,4];
            foreach (ShapeComponent c in Components)
            {
                matrix[c.X, c.Y] = true;
            }


            
            for (int y = 3; y >= 0; y--)
            {
                for (int x = 0; x <= 3; x++)
                {
                    if(matrix[x, y] == false)
                    {
                        Console.Write("-");
                    } else
                    {
                        Console.Write("X");
                    }
                }
                Console.WriteLine();
            }
            
        }
        
    }
}
