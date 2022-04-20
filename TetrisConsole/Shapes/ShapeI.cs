﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsole.Shapes
{
    internal class ShapeI : Shape
    {
        public ShapeI()
        {
            type = ShapeEnum.I;
            components.Add(new ShapeComponent(1, 3, 1));
            components.Add(new ShapeComponent(1, 2, 2));
            components.Add(new ShapeComponent(1, 1, 3));
            components.Add(new ShapeComponent(1, 0, 4));
        }

        public override void RotateRight()
        {
            switch (facing)
            {
                case Direction.UP:
                    facing = Direction.RIGHT;
                    GCBI(1).MoveCoord(2, -1);
                    GCBI(2).MoveCoord(1, 0);
                    GCBI(3).MoveCoord(0, 1);
                    GCBI(4).MoveCoord(-1, 2);
                    break;

                case Direction.RIGHT:
                    facing = Direction.DOWN;
                    GCBI(1).MoveCoord(-1, -2);
                    GCBI(2).MoveCoord(0, -1);
                    GCBI(3).MoveCoord(1, 0);
                    GCBI(4).MoveCoord(2, 1);
                    break;

                case Direction.DOWN:
                    facing = Direction.LEFT;
                    GCBI(1).MoveCoord(-2, 1);
                    GCBI(2).MoveCoord(-1, 0);
                    GCBI(3).MoveCoord(1, 1);
                    GCBI(4).MoveCoord(1, -2);
                    break;

                case Direction.LEFT:
                    facing = Direction.UP;
                    GCBI(1).MoveCoord(1, 2);
                    GCBI(2).MoveCoord(1, 1);
                    GCBI(3).MoveCoord(-1, 0);
                    GCBI(4).MoveCoord(-2, -1);
                    break;

                default:
                    break;
            }
        }
    }
}