using System;
using System.Collections.Generic;
using TetrisLibrary.Misc;

namespace TetrisLibrary.Shapes
{
    internal class ShapeN : Shape
    {
        public ShapeN()
        {
            Type = ShapeEnum.N;
            Components.Add(new ShapeComponent(0, 0, 1, this));
            Components.Add(new ShapeComponent(0, 1, 2, this));
            Components.Add(new ShapeComponent(0, 2, 3, this));
            Components.Add(new ShapeComponent(0, 3, 4, this));
            Components.Add(new ShapeComponent(0, 4, 5, this));
            Components.Add(new ShapeComponent(1, 3, 6, this));
            Components.Add(new ShapeComponent(2, 2, 7, this));
            Components.Add(new ShapeComponent(2, 1, 8, this));
            Components.Add(new ShapeComponent(3, 0, 9, this));
            Components.Add(new ShapeComponent(4, 0, 10, this));
            Components.Add(new ShapeComponent(4, 1, 11, this));
            Components.Add(new ShapeComponent(4, 2, 12, this));
            Components.Add(new ShapeComponent(4, 3, 13, this));
            Components.Add(new ShapeComponent(4, 4, 14, this));
        }

        public override List<IPoint> AfterRotateRightPoss()
        {
            return new List<IPoint>();
        }
    }
}
