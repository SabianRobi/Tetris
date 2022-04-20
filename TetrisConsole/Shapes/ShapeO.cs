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
            type = ShapeEnum.O;
            components.Add(new ShapeComponent(0, 0, 1));
            components.Add(new ShapeComponent(0, 1, 2));
            components.Add(new ShapeComponent(1, 0, 3));
            components.Add(new ShapeComponent(1, 1, 4));
        }

        public override void RotateRight() { }
    }
}
