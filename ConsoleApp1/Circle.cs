using System;
using System.Collections.Generic;
using System.Text;

namespace CirclePuzzle
{
    public class Circle
    {
        public Point centerCoord { get; private set; }
        public float radius { get; private set; }

        public Circle(Point coord, float rad)
        {
            centerCoord = coord;
            radius = rad;
        }

        public override string ToString()
        {
            return "Center: " + centerCoord + "\t Radius: " + radius;
        }
    }
}
