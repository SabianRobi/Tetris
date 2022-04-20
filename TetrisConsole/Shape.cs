using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace TetrisConsole
{
    abstract class Shape
    {
        //private Color color
        protected List<ShapeComponent> components;
        protected ShapeEnum type;
        protected Direction facing;

        public Shape()
        {
            components = new List<ShapeComponent>();
            facing = Direction.UP;
        }

        public abstract void RotateRight();

        public void MoveShape(Direction d)
        {
            foreach (ShapeComponent c in components)
            {
                c.Move(d);
            }
        }

        protected ShapeComponent GetComponentById(int id)
        {
            return components.Find(elem => elem.id == id);
        }
        protected ShapeComponent GCBI(int id)
        {
            return GetComponentById(id);
        }
        
    }
}
