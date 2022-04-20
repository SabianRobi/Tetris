using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsole {
    internal class ShapeComponent {
        private int x;
        private int y;

        public ShapeComponent(int x, int y) {
            this.x = x;
            this.y = y;
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
                    this.x += 1;
                    break;
                case Direction.RIGHT:
                    this.x -= 1;
                    break;
            }
        }

        public int GetX() { return x; }
        public int GetY() { return y; }
    }
}
