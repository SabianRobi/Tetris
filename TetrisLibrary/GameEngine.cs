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
        public Shape currentShape { get; private set; }
        public Shape nextShape { get; private set; }
        private bool isConsole;
        private Timer timer;
        private bool isRotating;
        private bool isMoving;
        public int score { get; private set; }
        public int lines { get; private set; }
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
            currentShape = null;
            nextShape = null;
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
                        MoveShape(currentShape, Direction.LEFT);
                    } else if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                    {
                        MoveShape(currentShape, Direction.RIGHT);
                    } else if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                    {
                        RotateRight();
                    } else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                    {
                        SimulateTimerTick();
                    }
                }
                Console.ReadKey();
            }
        }

        private bool CreateShape(object o = null)
        {
            ShapeEnum shapeEnum;
            if (o != null)
            {
                shapeEnum = (ShapeEnum)o;
            } else
            {
                shapeEnum = (ShapeEnum)GetRandomNumber(0, Enum.GetValues(typeof(ShapeEnum)).Length - 5);
            }
            Shape newShape = null;

            switch (shapeEnum)
            {
                case ShapeEnum.I:
                    newShape = new ShapeI();
                    break;
                case ShapeEnum.O:
                    newShape = new ShapeO();
                    break;
                case ShapeEnum.T:
                    newShape = new ShapeT();
                    break;
                case ShapeEnum.S:
                    newShape = new ShapeS();
                    break;
                case ShapeEnum.Z:
                    newShape = new ShapeZ();
                    break;
                case ShapeEnum.J:
                    newShape = new ShapeJ();
                    break;
                case ShapeEnum.L:
                    newShape = new ShapeL();
                    break;
            }
            
            currentShape = nextShape;
            nextShape = newShape;
            if(currentShape == null)
            {
                CreateShape();
            }
            else
            {
                currentShape.MoveShapeTopTo(MAPCENTER);
                if (currentShape.Components.Exists(comp =>
                {
                    return fields[comp.X, comp.Y] != null;
                }))
                {
                    return false;
                }
                UpdateShapePos(currentShape);
                shapes.Add(currentShape);
            }
            
            return true;
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
//INDEXOUTOFBOUNDEXCEPTION néhanapján
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

        //It rotates if it can
        public void RotateRight()
        {
            isRotating = true;
            bool canRotate = true;

            Shape testShape = currentShape.GetCopy();
            testShape.RotateRight();
            AlignShapeBackToMap(testShape);

            foreach (ShapeComponent component in testShape.Components)
            {
                Log("Komponens vizsgálat:"+ component.X + ", "+component.Y);
                if(fields[component.X, component.Y] != null)
                {
                    Log("Van ott valami");
                    if (fields[component.X, component.Y].Shape != currentShape)
                    {
                        Log("Nem én vagyok ott");
                        canRotate = false;
                    }
                }
            }

            if(canRotate)
            {
                currentShape.RotateRight();
                AlignShapeBackToMap(currentShape);
                UpdateShapePos(currentShape);
                DrawMapToConsole();
            } else
            {
                Log("Cannot rotate");
            }
            isRotating = false;
        }

        private void AlignShapeBackToMap(Shape shape)
        {
            if(!shape.Components.Exists(comp => comp.X < 0 || comp.X > MAPWIDTH-1 || comp.Y < 0 || comp.Y > MAPHEIGHT-1))
            {
                Log("Alignment not needed");
                return;
            }
            Log("Alignment needed");
            shape.Components.ForEach(comp =>
            {
                if(comp.X < 0)
                {
                    Log("Moving right " + Math.Abs(comp.X));
                    shape.Move(Direction.RIGHT, Math.Abs(comp.X));
                }
                else if (comp.X > MAPWIDTH-1)
                {
                    Log("Moving left " + (comp.X - (MAPWIDTH - 1)));
                    shape.Move(Direction.LEFT, comp.X-(MAPWIDTH-1));
                }
                if(comp.Y < 0)
                {
                    Log("Moving up" + Math.Abs(comp.Y));
                    shape.Move(Direction.UP, Math.Abs(comp.Y));
                }
                else if(comp.Y > MAPHEIGHT-1)
                {
                    Log("Moving down " + (comp.Y - (MAPHEIGHT - 1)));
                    shape.Move(Direction.DOWN, comp.Y - (MAPHEIGHT - 1));
                }
            });
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
                        if(fields[x, y].Shape == currentShape)
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

        public bool SimulateTimerTick()
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
                        moveShapesDown = ExamineRows(things);
                        Log("Got the value back: " + moveShapesDown);
                        score += 5;
                    }
                }
            }

            foreach (Shape shape in things.toRemoveShapes)
            {
                shapes.Remove(shape);
            }

            if (moveShapesDown)
            {
                things.rowsDeleted.Sort();
                things.rowsDeleted.Reverse();
                for (int i = 0; i < things.rowsDeleted.Count; i++)
                {
                    MoveShapesDown(things.rowsDeleted[i]);
                }
            }

            if (createNewShape)
            {
                if(!CreateShape())
                {
                    Log("Couldn't create new shape. Game over!");
                    End();
                    return true;
                }
            }

            DrawMapToConsole();
            return false;
        }

        private bool ExamineRows(ExamineThings things)
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
                }
            }
            int rows = things.rowsDeleted.Count;
            lines += rows;
            score += (int)Math.Pow(rows, 2)*100;
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

        private void End()
        {
            if(isConsole)
            {
                timer.Stop();
            }
            ShowEndShapes();
            DrawMapToConsole();
        }

        public int GetLines()
        {
            return lines;
        }

        public int GetScore()
        {
            return score;
        }

        public Shape GetNextShape()
        {
            return nextShape;
        }

        private void ShowEndShapes()
        {
            nextShape = new ShapeSmiley();

            shapes.Clear();
            for (int x = 0; x < MAPWIDTH; x++)
            {
                for (int y = 0; y < MAPHEIGHT; y++)
                {
                    fields[x, y] = null;
                }
            }

            Shape E = new ShapeE();
            E.Move(Direction.RIGHT, 3);
            E.Move(Direction.UP, 14);
            shapes.Add(E);
            UpdateShapePos(E);

            Shape N = new ShapeN();
            N.Move(Direction.RIGHT, 3);
            N.Move(Direction.UP, 8);
            shapes.Add(N);
            UpdateShapePos(N);

            Shape D = new ShapeD();
            D.Move(Direction.RIGHT, 3);
            D.Move(Direction.UP, 2);
            shapes.Add(D);
            UpdateShapePos(D);
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
