using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisConsole
{
    internal class GameEngine
    {
        private const int MAPHEIGHT = 10;
        private const int MAPWIDTH = 10;
        private const int SHAPEMAXHEIGHT = 4;
        private const int SHAPEMAXWIDTH = 4;
        private List<Shape> shapes;

        public GameEngine() {
            shapes = new List<Shape>();
            for (int i = 0; i < 20; i++) {
                makeNewShape();
            }
            Console.WriteLine("Nyomj entert a bezáráshoz!");
            Console.ReadLine();
        }

        public void makeNewShape()
        {
            shapes.Append(new Shape(SHAPEMAXWIDTH, SHAPEMAXHEIGHT));
        }

        public void moveShapes(Direction d)
        {
            foreach (Shape shape in shapes)
            {
                shape.Move(d);
            }
        }

        
    }
}
