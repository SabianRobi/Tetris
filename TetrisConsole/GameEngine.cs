using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TetrisConsole.Shapes;

namespace TetrisConsole
{
    internal class GameEngine
    {
        private const int MAPHEIGHT = 10;
        private const int MAPWIDTH = 10;
        private List<Shape> shapes;
        RandomNumberGenerator rng;
        byte[] randNum;

        public GameEngine() {
            shapes = new List<Shape>();
            rng = RandomNumberGenerator.Create();
            randNum = new byte[4];


            shapes.Add(new ShapeI());
            shapes.Add(new ShapeO());
            shapes.Add(new ShapeT());
            shapes.Add(new ShapeS());
            shapes.Add(new ShapeZ());
            shapes.Add(new ShapeJ());
            shapes.Add(new ShapeL());
            Console.WriteLine("Nyomj entert a bezáráshoz!");
            Console.ReadLine();
        }

        public void CreateShape()
        {
            ShapeEnum shapeEnum = (ShapeEnum)GetRandomNumber(1, Enum.GetValues(typeof(ShapeEnum)).Length);

            switch (shapeEnum)
            {
                case ShapeEnum.I:
                    shapes.Add(new ShapeI());
                    break;
                case ShapeEnum.O:
                    shapes.Add(new ShapeO());
                    break;
                case ShapeEnum.T:
                    shapes.Add(new ShapeT());
                    break;
                case ShapeEnum.S:
                    shapes.Add(new ShapeZ());
                    break;
                case ShapeEnum.Z:
                    shapes.Add(new ShapeZ());
                    break;
                case ShapeEnum.J:
                    shapes.Add(new ShapeJ());
                    break;
                case ShapeEnum.L:
                    shapes.Add(new ShapeL());
                    break;
            }
        }

        void RotateShape(Shape shape, Direction direction)
        {
            shape.RotateRight();
            AlignShape(shape);
        }

        void AlignShape(Shape shape)
        {
            //TODO: Align the shape back to map if the rotation would rotate the shape out of the map
        }

        public void moveShapes(Direction d)
        {
            foreach (Shape shape in shapes)
            {
                shape.MoveShape(d);
            }
        }

        private int GetRandomNumber(int min, int max)
        {
            rng.GetBytes(randNum);
            return Math.Abs(BitConverter.ToInt32(randNum, 0)) % max - min + 1 + min;
        }

    }
}
