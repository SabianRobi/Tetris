using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Timers;
using TetrisLibrary.Misc;
using TetrisLibrary.Shapes;

namespace TetrisLibrary
{
    public class GameEngine
    {
        private const int MAPHEIGHT = 20;
        private const int MAPWIDTH = 10;
        private Point MAPCENTER = new Point(MAPWIDTH / 2, MAPHEIGHT - 1);
        private List<Shape> shapes;
        private ShapeComponent[,] fields;
        private RandomNumberGenerator rng;
        private byte[] randNum;
        private int tick;
        public Shape current { get; set; }
        private bool isConsole;
        private Timer timer;
        private bool isRotating;
        private bool isMoving;
        public int score;
        private bool debug;

        public GameEngine(bool isConsole = false, bool debug = false) {
            Init(isConsole, debug);
        }

        private void Init(bool isConsole, bool debug)
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
            tick = 0;
            this.isConsole = isConsole;
            if (isConsole)
            {
                timer = new Timer(1000);
                timer.Elapsed += OnTimerTick;
                timer.AutoReset = true;
                if(debug)
                {
                    this.debug = true;
                }
            }
            isRotating = false;
            isMoving = false;
            score = 0;
        }

        public void Start()
        {
            CreateShape();
            DrawMapToConsole();
            if (isConsole)
            {
                timer.Start();
                ConsoleKeyInfo key;
                while (key.Key != ConsoleKey.Escape)
                {
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                    {
                        MoveShape(current, Direction.LEFT);
                    } else if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                    {
                        MoveShape(current, Direction.RIGHT);
                    } else if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                    {
                        RotateRight();
                    } else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                    {
                        SimulateTimerTick();
                    }
                }
            }
        }

        private Shape CreateShape(object o = null)
        {
            ShapeEnum shapeEnum;
            if (o != null)
            {
                shapeEnum = (ShapeEnum)o;
            } else
            {
                shapeEnum = (ShapeEnum)GetRandomNumber(0, Enum.GetValues(typeof(ShapeEnum)).Length - 1);
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
            current = shape;
            return shape;
        }

        //It moves if it can
        public int MoveShape(Shape shape, Direction d, int amount = 1, bool updatePos = true)
        {
            isMoving = true;
            int shapeMoved = 0;
            int pos = shape.GetMaxPos(d);
            switch (d)
            {
                case Direction.UP:
                    if (pos + amount <= MAPHEIGHT - 1)
                    {
                        shape.Move(d, amount);
                        shapeMoved = amount;
                    }
                    break;
                case Direction.DOWN:
                    for (int i = 1; i <= amount; i++)
                    {
                        bool canMoveDown = true;
                        foreach (ShapeComponent component in shape.Components)
                        {
                            if (component.Y == 0 || component.Y - i < 0 ||
                                (fields[component.X, component.Y - i] != null &&
                                !shape.Components.Contains(fields[component.X, component.Y - i])))
                            {
                                canMoveDown = false;
                            }
                        }
                        if (canMoveDown)
                        {
                            shape.Move(Direction.DOWN, 1);
                            shapeMoved++;
                        }
                    }
                    break;
                case Direction.LEFT:
                    for (int i = 1; i <= amount; i++)
                    {
                        bool canMove = true;
                        foreach (ShapeComponent component in shape.Components)
                        {

                            if(component.X - i >= MAPWIDTH)
                            {
                                canMove = true;
                            }
                            else if (component.X == 0 || component.X - i < 0 ||
                                (fields[component.X - i, component.Y] != null &&
                                !shape.Components.Contains(fields[component.X - i, component.Y])))
                            {
                                canMove = false;
                            }
                        }
                        if (canMove)
                        {
                            shape.Move(d, 1);
                            shapeMoved++;
                        }
                    }
                    break;
                case Direction.RIGHT:
                    if (pos + amount <= MAPWIDTH - 1)
                    {
                        for (int i = 1; i <= amount; i++)
                        {
                            bool canMove = true;
                            foreach (ShapeComponent component in shape.Components)
                            {
                                if (component.X + i < 0)
                                {
                                    canMove = true;
                                }
//INDEXOUTOFBOUNDEXCEPTION
                                else if (component.X == MAPWIDTH-1 || component.X + i >= MAPWIDTH ||
                                    (fields[component.X + i, component.Y] != null &&
                                    !shape.Components.Contains(fields[component.X + i, component.Y])))
                                {
                                    canMove = false;
                                }
                            }
                            if (canMove)
                            {
                                shape.Move(d, 1);
                                shapeMoved++;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            if (updatePos)
            {
                UpdateShapePos(shape);
            }
            isMoving = false;
            DrawMapToConsole();
            return shapeMoved;
        }

        private void UpdateShapePos(Shape shape)
        {
            int counter = 0;
            //Cycle goes from top to down
            for (int y = 0; y < MAPHEIGHT && counter < 4; y++)
            {
                for (int x = 0; x < MAPWIDTH && counter < 4; x++)
                {
                    //Delete all componenets from fields
                    if (fields[x, y] != null && fields[x, y].Shape.Equals(shape))
                    {
                        fields[x, y] = null;
                        counter++;
                    }
                }
            }

            //Add the moved components to fields
            foreach (ShapeComponent component in shape.Components)
            {
                fields[component.X, component.Y] = component;
            }
        }

        //TODO DOING: It rotates (when it can)
        public void RotateRight()
        {
            isRotating = true;
            bool canRotate = true;

            //Műxik, csak map szélén bugos, nem "tolja" vissza a pályára mielőtt tesztelni
            List<IPoint> relCoords = new List<IPoint>();
            relCoords.AddRange(current.AfterRotateRightPoss());
            for (int i = 0; i < 4; i++)
            {
                if(current.Components.Exists(p => p.id == i+1))
                {
                    Log("van current componens");
                    if(relCoords.Exists(p => p.id == i+1))
                    {
                        Log("a komponens forgatható");
                        if (fields[current.GCBI(i + 1).X + relCoords.Find(p => p.id == i+1).p.X, current.GCBI(i + 1).Y + relCoords.Find(p => p.id == i + 1).p.Y] != null)
                        {
                            Log("van valami after forgatás mezőjén");
                            if(fields[current.GCBI(i + 1).X + relCoords.Find(p => p.id == i + 1).p.X, current.GCBI(i + 1).Y + relCoords.Find(p => p.id == i + 1).p.Y].Shape != current)
                            {
                                Log("Ütközik");
                                canRotate = false;
                            }
                        }
                        
                    }
                }
            }








            /* v2
            Shape backupShape = current.GetCopy();
            current.RotateRight();
            AlignShapeBackToMap(current);
            foreach (ShapeComponent component in current.Components)
            {
                if( fields[component.X, component.Y] != null &&
                    fields[component.X, component.Y].Shape != current)
                {
                    canRotate = false;
                }
            }
            if (!canRotate)
            {
                current = backupShape;
            }
            */


            /* v1

            Shape testShape = current.GetCopy();
            testShape.RotateRight();
            AlignShapeBackToMap(testShape);

            foreach (ShapeComponent testComponent in testShape.Components)
            {
                if (testComponent.X < 0 || testComponent.X > MAPWIDTH - 1 ||
                        testComponent.Y < 0 || testComponent.Y > MAPHEIGHT - 1 ||
                        (fields[testComponent.X, testComponent.Y] != null &&
                        fields[testComponent.X, testComponent.Y].Shape != current))
                        //!current.Components.Exists(c => c.X == testComponent.X && c.Y == testComponent.Y)))
                {
                    canRotate = false;
                }
            }
            */
            if(canRotate)
            {
                current.RotateRight();
                AlignShapeBackToMap(current);
                UpdateShapePos(current);
                DrawMapToConsole();
            } else
            {
                Log("Cannot rotate");
            }
            isRotating = false;
        }

        private void AlignShapeBackToMap(Shape shape)
        {
            Log("Aligning back:");
            foreach (ShapeComponent component in shape.Components)
            {
                if (component.X < 0)
                {
                    int amount = Math.Abs(component.X);
                    Log("Moving right " + amount);
                    MoveShape(shape, Direction.RIGHT, amount, false);
                } else if (component.X > MAPWIDTH - 1)
                {
                    int amount = component.X - MAPWIDTH - 1/* + 1*/;
                    Log("Moving left " + amount);
                    MoveShape(shape, Direction.LEFT, amount, false);
                }

                if (component.Y < 0)
                {
                    int amount = Math.Abs(component.Y);
                    Log("Moving up " + amount);
                    MoveShape(shape, Direction.UP, amount, false);
                } else if (component.Y > MAPHEIGHT - 1)
                {
                    int amount = component.Y - MAPHEIGHT - 1;
                    Log("Moving down " + amount);
                    MoveShape(shape, Direction.DOWN, amount, false);
                }
            }
        }

        private void DrawMapToConsole()
        {
            if (!isConsole) return;
            if(!debug)
            {
                Console.Clear();
            } else
            {
                Console.WriteLine();
            }
            for (int y = MAPHEIGHT - 1; y >= 0; y--)
            {
                for (int x = 0; x < MAPWIDTH; x++)
                {
                    if (fields[x, y] != null)
                    {
                        if(fields[x, y].Shape == current)
                        {
                            Console.Write("X");
                        } else
                        {
                            Console.Write("#");
                        }
                    }
                    else
                    {
                        Console.Write("-");
                    }
                }
                Console.WriteLine();
            }
        }

        public List<Shape> GetShapes()
        {
            return shapes;
        }

        private int GetRandomNumber(int min, int max)
        {
            rng.GetBytes(randNum);
            return (Math.Abs(BitConverter.ToInt32(randNum, 0)) % (max - min + 1)) + min;
        }

        public void OnTimerTick(Object source, ElapsedEventArgs e)
        {
            SimulateTimerTick();
        }

        public void SimulateTimerTick()
        {
            while (isRotating || isMoving) { }
            tick++;
            bool createNewShape = false;
            bool moveShapesDown = false;
            ExamineThings things = new ExamineThings();
            foreach (Shape shape in shapes)
            {
                if (!shape.IsAtBottom)
                {
                    if(MoveShape(shape, Direction.DOWN, 1, true) == 0)
                    {
                        Log("Shape arrived down");
                        shape.IsAtBottom = true;
                        createNewShape = true;
                        moveShapesDown = ExamineRows(ref things);
                        Log("Got the value back: " + moveShapesDown);
                    }
                }
            }

            foreach (Shape shape in things.toRemoveShapes)
            {
                //shape.Die();
                shapes.Remove(shape);
            }

            if (moveShapesDown)
            {
                /*
                foreach (Shape shape in shapes)
                {
                    //MoveShape(shape, Direction.DOWN, 1, true);
                }
                */
                things.rowsDeleted.Sort();
                things.rowsDeleted.Reverse();
                for (int i = 0; i < things.rowsDeleted.Count; i++)
                {
                    MoveShapesDown(things.rowsDeleted[i]);
                }
            }

            if (createNewShape)
            {
                CreateShape();
            }

            DrawMapToConsole();
        }

        private bool ExamineRows(ref ExamineThings things)
        {
            Log("Examining rows");
            bool isFilled;
            for (int y = 0; y < MAPHEIGHT; y++)
            {
                isFilled = true;
                for (int x = 0; x < MAPWIDTH && isFilled; x++)
                {
                    if (fields[x, y] == null)
                    {
                        isFilled = false;
                    }
                }
                if (isFilled)
                {
                    Log("Found filled row: " + y);
                    things.rowsDeleted.Add(y);
                    for (int x = 0; x < MAPWIDTH; x++)
                    {
                        fields[x, y].Shape.RemoveComponent(fields[x, y]);
                        fields[x, y] = null;
                    }

                    foreach (Shape shape in shapes)
                    {
                        if (shape.Components.Count == 0)
                        {
                            things.toRemoveShapes.Add(shape);
                        }
                    }
                    /*
                    foreach (Shape shape in shapes)
                    {
                        if(!toRemoveShapes.Contains(shape))
                        {
                            MoveShape(shape, Direction.DOWN, 1, false);
                        }
                    }
                    */
                    score += MAPWIDTH * 10;
                }
            }
            return things.rowsDeleted.Count > 0;
        }

        private void MoveShapesDown(int y)
        {
            foreach (Shape shape in shapes)
            {
                foreach (ShapeComponent component in shape.Components)
                {
                    if(component.Y >= y)
                    {
                        component.Move(Direction.DOWN);
                    }
                }
                UpdateShapePos(shape);
            }
        }

        private class ExamineThings {
            public List<Shape> toRemoveShapes;
            public List<int> rowsDeleted;

            public ExamineThings()
            {
                toRemoveShapes = new List<Shape>();
                rowsDeleted = new List<int>();
            }
        }

        private void Log(String s)
        {
            if(isConsole && debug)
            {
                Console.WriteLine(s);
            }
        }

    }
}
