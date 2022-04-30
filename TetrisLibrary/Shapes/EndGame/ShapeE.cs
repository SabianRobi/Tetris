using System;
using System.Collections.Generic;
using TetrisLibrary.Misc;

namespace TetrisLibrary.Shapes
{
    internal class ShapeE : Shape
    {
        public ShapeE()
        {
            Type = ShapeEnum.E;
            Components.Add(new ShapeComponent(0, 0, 1, this));
            Components.Add(new ShapeComponent(1, 0, 2, this));
            Components.Add(new ShapeComponent(2, 0, 3, this));
            Components.Add(new ShapeComponent(3, 0, 4, this));
            Components.Add(new ShapeComponent(0, 1, 5, this));
            Components.Add(new ShapeComponent(0, 2, 6, this));
            Components.Add(new ShapeComponent(1, 2, 7, this));
            Components.Add(new ShapeComponent(2, 2, 8, this));
            Components.Add(new ShapeComponent(3, 2, 9, this));
            Components.Add(new ShapeComponent(0, 3, 10, this));
            Components.Add(new ShapeComponent(0, 4, 11, this));
            Components.Add(new ShapeComponent(1, 4, 12, this));
            Components.Add(new ShapeComponent(2, 4, 13, this));
            Components.Add(new ShapeComponent(3, 4, 14, this));
        }

        public override List<IPoint> AfterRotateRightPoss()
        {
            return new List<IPoint>();
        }
    }
}
