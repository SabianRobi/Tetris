using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisConsole.Shapes
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

        public override void RotateRight() { }
    }
}
