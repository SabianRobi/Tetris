using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace TetrisConsole {
    internal class Shape {
        
        private int width;
        private int height;
        //private Color color
        private List<ShapeComponent> components;

        public Shape(int maxWidth, int maxHeight) {
            this.width = GetRandomNumber(1, maxWidth);
            this.height = GetRandomNumber(1, maxHeight);
            components = new List<ShapeComponent>();
            GenerateShape();
        }

        private void GenerateShape() {
            Console.WriteLine("width: " + width + ", height: " + height);
        }

        public void Move(Direction d) {
            foreach (ShapeComponent c in components) {
                c.Move(d);
            }
        }

        private int GetRandomNumber(int min, int max) {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] randNum = new byte[4];
            rng.GetBytes(randNum);
            return Math.Abs(BitConverter.ToInt32(randNum, 0)) % max-min+1 + min;
        }

    }
}
