using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsole.Shapes
{
    internal class ShapeL : Shape
    {
        public ShapeL()
        {
            Type = ShapeEnum.L;
            Components.Add(new ShapeComponent(0, 2, 1, this));
            Components.Add(new ShapeComponent(0, 1, 2, this));
            Components.Add(new ShapeComponent(0, 0, 3, this));
            Components.Add(new ShapeComponent(1, 0, 4, this));
        }

        public override void RotateRight()
        {
            switch (Facing)
            {
                case Direction.UP:
                    Facing = Direction.RIGHT;
                    GCBI(1).MoveCoord(2, -1);
                    GCBI(2).MoveCoord(1, 0);
                    GCBI(3).MoveCoord(0, 1);
                    GCBI(4).MoveCoord(-1, 0);
                    break;

                case Direction.RIGHT:
                    Facing = Direction.DOWN;
                    GCBI(1).MoveCoord(-1, -1);
                    GCBI(3).MoveCoord(1, 1);
                    GCBI(4).MoveCoord(0, 2);
                    break;

                case Direction.DOWN:
                    Facing = Direction.LEFT;
                    GCBI(1).MoveCoord(-1, 0);
                    GCBI(2).MoveCoord(0, -1);
                    GCBI(3).MoveCoord(1, -2);
                    GCBI(4).MoveCoord(2, -1);
                    break;

                case Direction.LEFT:
                    Facing = Direction.UP;
                    GCBI(1).MoveCoord(0, 2);
                    GCBI(2).MoveCoord(-1, 1);
                    GCBI(3).MoveCoord(-2, 0);
                    GCBI(4).MoveCoord(-1, -1);
                    break;

                default:
                    break;
            }
        }
    }
}
