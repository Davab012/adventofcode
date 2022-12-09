using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9_rope_movements
{
    internal class Coord
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coord(Coord c)
        {
            X = c.X;
            Y = c.Y;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }

        // Get hashcode
        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }

        // Calculate the longest one dimensional distance between two coordinates.
        public int Distance1d(Coord c)
        {
            return Math.Max(Math.Abs(X - c.X), Math.Abs(Y - c.Y));
        }
        
        public Coord CalculateDiff(Coord moveTo)
        {
            var dx = moveTo.X - X;
            var dy = moveTo.Y - Y;
            return new Coord(dx, dy);
        }

        public void Move(int x, int y)
        {
            X += x;
            Y += y;
        }

        public void Move(Coord c, int limit = 1)
        {
            X += c.X != 0 ? c.X / Math.Abs(c.X) : 0;
            Y += c.Y != 0 ? c.Y / Math.Abs(c.Y) : 0;
        }
    }
}
