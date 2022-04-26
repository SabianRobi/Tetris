using System.Collections.Generic;
using TetrisLibrary.Misc;

namespace TetrisLibrary.Shapes
{
    internal class ShapeO : Shape
    { 
        public ShapeO()
        {
            Type = ShapeEnum.O;
            Components.Add(new ShapeComponent(0, 0, 1, this));
            Components.Add(new ShapeComponent(0, 1, 2, this));
            Components.Add(new ShapeComponent(1, 0, 3, this));
            Components.Add(new ShapeComponent(1, 1, 4, this));
        }

        public override List<IPoint> AfterRotateRightPoss()
        {
            return new List<IPoint>();
        }
    }
}
