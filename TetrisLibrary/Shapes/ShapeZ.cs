using System.Collections.Generic;
using TetrisLibrary.Misc;

namespace TetrisLibrary.Shapes
{
    internal class ShapeZ : Shape
    {
        public ShapeZ()
        {
            Type = ShapeEnum.Z;
            Components.Add(new ShapeComponent(0, 2, 1, this));
            Components.Add(new ShapeComponent(1, 2, 2, this));
            Components.Add(new ShapeComponent(1, 1, 3, this));
            Components.Add(new ShapeComponent(2, 1, 4, this));
        }

        public override List<IPoint> AfterRotateRightPoss()
        {
            List<IPoint> poss = new List<IPoint>();
            switch (Facing)
            { 
                case Direction.UP:
                    poss.Add(new IPoint(1, 2, 0));
                    poss.Add(new IPoint(2, 1, -1));
                    poss.Add(new IPoint(4, -1, -1));
                    break;

                case Direction.RIGHT:
                    poss.Add(new IPoint(1, 0, -2));
                    poss.Add(new IPoint(2, -1, -1));
                    poss.Add(new IPoint(4, -1, 1));
                    break;

                case Direction.DOWN:
                    poss.Add(new IPoint(1, -2, 0));
                    poss.Add(new IPoint(2, -1, 1));
                    poss.Add(new IPoint(4, 1, 1));
                    break;

                case Direction.LEFT:
                    poss.Add(new IPoint(1, 0, 2));
                    poss.Add(new IPoint(2, 1, 1));
                    poss.Add(new IPoint(4, 1, -1));
                    break;
            }
            return poss;
        }
    }
}
