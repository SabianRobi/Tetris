using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TetrisConsole.Misc;
using TetrisConsole.Shapes;

namespace TetrisConsole
{
    internal class GameEngine
    {
        private const int MAPHEIGHT = 20;
        private const int MAPWIDTH = 10;
        private Point MAPCENTER = new Point(MAPWIDTH / 2, MAPHEIGHT - 1);
        private List<Shape> shapes;
        private ShapeComponent[,] fields;
        RandomNumberGenerator rng;
        byte[] randNum;

        public GameEngine() {
            InitGame();
            startGame();
            /*
            shapes.Add(new ShapeI());
            shapes[0].drawToConsole();


            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(i + 1 + ".forgatás után:");
                shapes[0].RotateRight();
                shapes[0].drawToConsole();
            }
            
            Console.WriteLine("Nyomj entert a bezáráshoz!");
            Console.ReadLine();
            */
        }

        private void InitGame()
        {
            shapes = new List<Shape>();
            fields = new ShapeComponent[MAPWIDTH, MAPHEIGHT];
            for (int x = 0; x < MAPWIDTH; x++)
            {
                for (int y = 0; y < MAPHEIGHT; y++)
                {
                    fields[x, y] = null;
                }
            }
            rng = RandomNumberGenerator.Create();
            randNum = new byte[4];
        }

        private void startGame()
        {
            Console.WriteLine("Kezdjük!");
            _ = CreateShape();
            Console.WriteLine();
            DrawMap();
            Console.ReadLine();
        }

        public Shape CreateShape()
        {
            return CreateShape(null);
        }
        public Shape CreateShape(object o)
        {
            ShapeEnum shapeEnum;
            if (o != null)
            {
                shapeEnum = (ShapeEnum)o;
            } else
            {
                shapeEnum = (ShapeEnum)GetRandomNumber(0, Enum.GetValues(typeof(ShapeEnum)).Length-1);
            }
            Shape shape = null;

            switch (shapeEnum)
            {
                case ShapeEnum.I:
                    shape = new ShapeI();
                    break;
                case ShapeEnum.O:
                    shape = new ShapeO();
                    break;
                case ShapeEnum.T:
                    shape = new ShapeT();
                    break;
                case ShapeEnum.S:
                    shape = new ShapeS();
                    break;
                case ShapeEnum.Z:
                    shape = new ShapeZ();
                    break;
                case ShapeEnum.J:
                    shape = new ShapeJ();
                    break;
                case ShapeEnum.L:
                    shape = new ShapeL();
                    break;
            }
            shape.MoveShapeTopTo(MAPCENTER);
            UpdateShapePos(shape);
            shapes.Add(shape);
            return shape;
        }

        void UpdateShapePos(Shape shape)
        {
            //Cycle goes from top to down
            for (int y = 0; y < MAPHEIGHT; y++)
            {
                for (int x = 0; x < MAPWIDTH; x++)
                {

                    //Delete all componenets from fields
                    if(fields[x, y] != null && fields[x, y].Shape.Equals(shape))
                    {
                        fields[x, y] = null;
                    }
                }
            }

            //Add the moved components to fields
            foreach (ShapeComponent component in shape.Components)
            {
                fields[component.X, component.Y] = component;
            }
        }

        /*
        void RotateShape(Shape shape, Direction direction)
        {
            shape.RotateRight();
            AlignShapeBackToMap(shape);
        }

        void AlignShapeBackToMap(Shape shape)
        {
            //TODO: Align the shape back to map if the rotation would rotate the shape out of the map
        }
        */

        /*
        void AlignShapeToCenter(Shape shape)
        {
            Point mapCenter = new Point(MAPWIDTH/2, MAPHEIGHT-1);
            Point shapeCenter = shape.GetCenter();
            shape.MoveShapeTopTo(mapCenter);
        }
        */

        void DrawMap()
        {
            for (int y = MAPHEIGHT-1; y >= 0; y--)
            {
                for (int x = 0; x < MAPWIDTH; x++)
                {
                    if (fields[x, y] != null)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write("-");
                    }
                }
                Console.WriteLine();
            }
        }

        private int GetRandomNumber(int min, int max)
        {
            rng.GetBytes(randNum);
            return Math.Abs(BitConverter.ToInt32(randNum, 0)) % max - min + 1 + min;
        }

    }
}
