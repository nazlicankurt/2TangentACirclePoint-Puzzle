using System;
using System.Collections.Generic;
using System.Text;

namespace CirclePuzzle
{
    public class Point
    {
        /// <summary>
        /// point coordinates
        /// </summary>
        public float x { get; private set; }
        public float y { get; private set; }

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "{ " + x + ", " + y + " }";
        }
    }
}
