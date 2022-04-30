using System.Collections.Generic;
using TetrisLibrary.Misc;

namespace TetrisLibrary.Shapes
{
    internal class ShapeSmiley : Shape
    {
        public ShapeSmiley()
        {
            Type = ShapeEnum.Smiley;
            Components.Add(new ShapeComponent(0, 0, 1, this));
            Components.Add(new ShapeComponent(1, 0, 2, this));
            Components.Add(new ShapeComponent(2, 0, 3, this));
            Components.Add(new ShapeComponent(0, 2, 4, this));
            Components.Add(new ShapeComponent(2, 2, 5, this));
        }

        public override List<IPoint> AfterRotateRightPoss()
        {
            return new List<IPoint>();
        }
    }
}
