using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsole {
    internal class ShapeComponent {
        private int x;
        private int y;
        public int id;

        public ShapeComponent(int x, int y, int id) {
            this.x = x;
            this.y = y;
            this.id = id;
        }

        public void Move(Direction d) {
            switch (d) {
                case Direction.UP:
                    this.y += 1;
                    break;
                case Direction.DOWN:
                    this.y -= 1;
                    break;
                case Direction.LEFT:
                    this.x -= 1;
                    break;
                case Direction.RIGHT:
                    this.x += 1;
                    break;
            }
        }

        public int GetX() { return x; }
        public int GetY() { return y; }
        public void SetX(int x) { this.x = x; }
        public void SetY(int y) { this.y = y; }

        public void MoveCoord(int x, int y)
        {
            this.x += x;
            this.y += y;
        }
    }
}
