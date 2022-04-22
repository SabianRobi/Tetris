using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsole.Shapes
{
    internal class ShapeT : Shape
    {
        public ShapeT()
        {
            Type = ShapeEnum.T;
            Components.Add(new ShapeComponent(1, 2, 1, this));
            Components.Add(new ShapeComponent(0, 1, 2, this));
            Components.Add(new ShapeComponent(1, 1, 3, this));
            Components.Add(new ShapeComponent(2, 1, 4, this));
        }

        public override void RotateRight()
        {
            switch (Facing)
            {
                case Direction.UP:
                    Facing = Direction.RIGHT;
                    GCBI(1).MoveCoord(1, -1);
                    GCBI(2).MoveCoord(1, 1);
                    GCBI(4).MoveCoord(-1, -1);
                    break;

                case Direction.RIGHT:
                    Facing = Direction.DOWN;
                    GCBI(1).MoveCoord(-1, -1);
                    GCBI(2).MoveCoord(1, -1);
                    GCBI(4).MoveCoord(-1, 1);
                    break;

                case Direction.DOWN:
                    Facing = Direction.LEFT;
                    GCBI(1).MoveCoord(-1, 1);
                    GCBI(2).MoveCoord(-1, -1);
                    GCBI(4).MoveCoord(1, 1);
                    break;

                case Direction.LEFT:
                    Facing = Direction.UP;
                    GCBI(1).MoveCoord(1, 1);
                    GCBI(2).MoveCoord(-1, 1);
                    GCBI(4).MoveCoord(1, -1);
                    break;
            }
        }
    }
}
