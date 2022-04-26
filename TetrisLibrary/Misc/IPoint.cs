using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisLibrary.Misc
{
    public class IPoint
    {
        public Point p { get; set; }
        public int id { get; set; }

        public IPoint(int id, Point p)
        {
            this.id = id;
            this.p = p;
        }

        public IPoint(int id, int x, int y)
        {
            this.id = id;
            this.p = new Point(x, y);
        }
    }
}
