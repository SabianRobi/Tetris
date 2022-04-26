using System.Collections.Generic;
using TetrisLibrary.Misc;

namespace TetrisLibrary.Shapes
{
    internal class ShapeS : Shape
    {
        public ShapeS()
        {
            Type = ShapeEnum.S;
            Components.Add(new ShapeComponent(0, 1, 1, this));
            Components.Add(new ShapeComponent(1, 1, 2, this));
            Components.Add(new ShapeComponent(1, 2, 3, this));
            Components.Add(new ShapeComponent(2, 2, 4, this));
        }

        public override List<IPoint> AfterRotateRightPoss()
        {
            List<IPoint> poss = new List<IPoint>();
            switch (Facing)
            {
                case Direction.UP:
                    poss.Add(new IPoint(1, 1, 1));
                    poss.Add(new IPoint(3, 1, -1));
                    poss.Add(new IPoint(4, 0, -2));
                    break;

                case Direction.RIGHT:
                    poss.Add(new IPoint(1, 1, -1));
                    poss.Add(new IPoint(3, -1, -1));
                    poss.Add(new IPoint(4, -2, 0));
                    break;

                case Direction.DOWN:
                    poss.Add(new IPoint(1, -1, -1));
                    poss.Add(new IPoint(3, -1, 1));
                    poss.Add(new IPoint(4, 0, 2));
                    break;

                case Direction.LEFT:
                    poss.Add(new IPoint(1, -1, 1));
                    poss.Add(new IPoint(3, 1, 1));
                    poss.Add(new IPoint(4, 2, 0));
                    break;
            }
            return poss;
        }
    }
}
