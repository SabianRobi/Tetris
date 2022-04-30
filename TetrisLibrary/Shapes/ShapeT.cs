using System.Collections.Generic;
using TetrisLibrary.Misc;

namespace TetrisLibrary.Shapes
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

        public override List<IPoint> AfterRotateRightPoss()
        {
            List<IPoint> poss = new List<IPoint>();
            switch (Facing)
            {
                case Direction.UP:
                    poss.Add(new IPoint(1, 1, -1));
                    poss.Add(new IPoint(2, 1, 1));
                    poss.Add(new IPoint(4, -1, -1));
                    break;

                case Direction.RIGHT:
                    poss.Add(new IPoint(1, -1, -1));
                    poss.Add(new IPoint(2, 1, -1));
                    poss.Add(new IPoint(4, -1, 1));
                    break;

                case Direction.DOWN:
                    poss.Add(new IPoint(1, -1, 1));
                    poss.Add(new IPoint(2, -1, -1));
                    poss.Add(new IPoint(4, 1, 1));
                    break;

                case Direction.LEFT:
                    poss.Add(new IPoint(1, 1, 1));
                    poss.Add(new IPoint(2, -1, 1));
                    poss.Add(new IPoint(4, 1, -1));
                    break;
            }
            return poss;
        }
    }
}
