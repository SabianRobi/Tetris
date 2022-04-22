namespace TetrisConsole.Misc
{
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point()
        {
            X = 0;
            Y = 0;
        }
        public Point(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public void Set(Point p)
        {
            this.X = p.X;
            this.Y = p.Y;
        }
    }
}
