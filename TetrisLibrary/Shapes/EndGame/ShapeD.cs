using System;
using System.Collections.Generic;
using TetrisLibrary.Misc;

namespace TetrisLibrary.Shapes
{
    internal class ShapeD : Shape
    {
        public ShapeD()
        {
            Type = ShapeEnum.D;
            Components.Add(new ShapeComponent(0, 0, 1, this));
            Components.Add(new ShapeComponent(0, 1, 2, this));
            Components.Add(new ShapeComponent(0, 2, 3, this));
            Components.Add(new ShapeComponent(0, 3, 4, this));
            Components.Add(new ShapeComponent(0, 4, 5, this));
            Components.Add(new ShapeComponent(1, 0, 6, this));
            Components.Add(new ShapeComponent(2, 0, 7, this));
            Components.Add(new ShapeComponent(3, 1, 8, this));
            Components.Add(new ShapeComponent(3, 2, 9, this));
            Components.Add(new ShapeComponent(3, 3, 10, this));
            Components.Add(new ShapeComponent(2, 4, 11, this));
            Components.Add(new ShapeComponent(1, 4, 12, this));
        }

        public override List<IPoint> AfterRotateRightPoss()
        {
            return new List<IPoint>();
        }
    }
}
