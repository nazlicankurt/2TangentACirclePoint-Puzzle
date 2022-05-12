using System;
using System.Collections.Generic;
using System.Text;

namespace CirclePuzzle
{
    public class LinearFunctionCoefficients
    {
        /// <summary>
        ///  coefficients: y = a* x + b, a and b are coefficients
        /// </summary>
        public float a { get; }
        public float b { get; }

        public LinearFunctionCoefficients(float a, float b)
        {
            this.a = a;
            this.b = b;
        }
    }
}
