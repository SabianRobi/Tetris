using System;
using System.Collections.Generic;
using TetrisLibrary.Misc;
using System.Linq;
using TetrisLibrary.Shapes;

namespace TetrisLibrary
{
    public abstract class Shape
    {
        //private Color color
        public List<ShapeComponent> Components { get; }
        public ShapeEnum Type;
        public Direction Facing;

        public Shape()
        {
            Components = new List<ShapeComponent>();
            Facing = Direction.UP;
        }
        public bool IsAtBottom { get; set; }

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
            return new Point(xs.Sum() / xs.Count, maxY-1);
        }

        public int GetMaxPos(Direction d)
        {
            List<int> xs = new List<int>();
            List<int> ys = new List<int>();
            foreach (ShapeComponent component in Components)
            {
                xs.Add(component.X);
                ys.Add(component.Y);
            }
            int num = 0;
            switch (d)
            {
                case Direction.UP:
                    num = ys.Max();
                    break;
                case Direction.DOWN:
                    num = ys.Min();
                    break;
                case Direction.LEFT:
                    num = xs.Min();
                    break;
                case Direction.RIGHT:
                    num = xs.Max();
                    break;
            }
            return num;
        }

        public void Move(Direction d, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                foreach (ShapeComponent c in Components)
                {
                    c.Move(d);
                }
            }
        }

        public void MoveShapeTopTo(Point mapCenter)
        {
            Point ShapeTopPos = GetTopPos();
            int moveX = mapCenter.X - ShapeTopPos.X;
            int moveY = mapCenter.Y - ShapeTopPos.Y;

            if(ShapeTopPos.X < mapCenter.X)
            {
                Move(Direction.RIGHT, moveX-1);
                //Console.Write("Mozgatás jobbra " + (moveX-1) + " mezővel");
            } else if (ShapeTopPos.X > mapCenter.X)
            {
                Move(Direction.LEFT, Math.Abs(moveX-1));
                //Console.Write("Mozgatás Balra " + Math.Abs(moveX-1) + " mezővel");
            }


            if (ShapeTopPos.Y < mapCenter.Y)
            {
                Move(Direction.UP, moveY-1);
                //Console.Write("Mozgatás fel " + (moveY-1) + " mezővel");
            } else if (ShapeTopPos.Y > mapCenter.Y)
            {
                Move(Direction.DOWN, Math.Abs(moveY-1));
                //Console.Write("Mozgatás Le " + Math.Abs(moveY-1) + " mezővel");
            }
        }

        public ShapeComponent GetComponentById(int id)
        {
            return Components.Find(elem => elem.id == id);
        }
        public ShapeComponent GCBI(int id)
        {
            return GetComponentById(id);
        }

        public void RemoveComponent(ShapeComponent component)
        {
            Components.Remove(component);
        }

        public abstract List<IPoint> AfterRotateRightPoss();

        public void RotateRight()
        {
            List<IPoint> points = new List<IPoint>();
            points.AddRange(AfterRotateRightPoss());
            foreach (IPoint p in points)
            {
                GCBI(p.id).MoveCoord(p.p.X, p.p.Y);
            }
            switch (Facing)
            {
                case Direction.UP:
                    Facing = Direction.RIGHT;
                    break;
                case Direction.RIGHT:
                    Facing = Direction.DOWN;
                    break;
                case Direction.DOWN:
                    Facing = Direction.LEFT;
                    break;
                case Direction.LEFT:
                    Facing = Direction.UP;
                    break;
            }
        }
        
        public Shape GetCopy()
        {
            Shape copy = null;
            switch (Type)
            {
                case ShapeEnum.I:
                    copy = new ShapeI();
                    break;
                case ShapeEnum.O:
                    copy = new ShapeO();
                    break;
                case ShapeEnum.T:
                    copy = new ShapeT();
                    break;
                case ShapeEnum.S:
                    copy = new ShapeS();
                    break;
                case ShapeEnum.Z:
                    copy = new ShapeZ();
                    break;
                case ShapeEnum.J:
                    copy = new ShapeJ();
                    break;
                case ShapeEnum.L:
                    copy = new ShapeL();
                    break;
            }
            copy.Facing = Facing;
            foreach (ShapeComponent component in Components)
            {
                copy.Components.Add(component.GetCopy(copy));
            }
            return copy;
        }
        
    }
}
