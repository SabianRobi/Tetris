using TetrisConsole.Misc;

namespace TetrisConsole {
    internal class ShapeComponent {
        public int X { get; set; }
        public int Y { get; set; }
        public Shape Shape { get; set; }
        public int id;

        public ShapeComponent(int X, int Y, int id, Shape Shape) {
            this.X = X;
            this.Y = Y;
            this.id = id;
            this.Shape = Shape;
        }

        public void Move(Direction d) {
            switch (d) {
                case Direction.UP:
                    Y += 1;
                    break;
                case Direction.DOWN:
                    Y -= 1;
                    break;
                case Direction.LEFT:
                    X -= 1;
                    break;
                case Direction.RIGHT:
                    X += 1;
                    break;
            }
        }

        public void MoveCoord(int x, int y)
        {
            this.X += x;
            this.Y += y;
        }

        public Point GetCoord()
        {
            return new Point(X, Y);
        }
    }
}
