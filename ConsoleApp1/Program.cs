using System;

namespace CirclePuzzle
{
    internal class Program
    {
        private static float Tangent1(float xVal)
        {
            //y = a * x + b, in example, a = 1/3f, x = xVal, and b = 0;
            return 1 / 3f * xVal;
        }

        private static float Tangent2(float xVal)
        {
            return -1 / 3f * xVal;
        }

        public static void Main(string[] args)
        {
            //give the points
            const float pointX = 5f;
            const float pointY = 0f;

            Point point = new Point(pointX, pointY);
            Circle foundCircle = FindCircle(Tangent1, Tangent2, point);

            if (foundCircle == null)
            {
                //Special case should be specified here and should be exception cases!!
                throw new InvalidOperationException("Circle not found(special case or parallel lines");
            }
            else
            {
                Console.Write(foundCircle.ToString());
            }
            Console.ReadLine();
        }

        public static Circle FindCircle(Func<float, float> firstTangent, Func<float, float> secondTangent, Point coord)
        {
            if (firstTangent == null || secondTangent == null || coord == null)
                return null;

            LinearFunctionCoefficients firstTangentCoefficients = FindLinearFunctionCoefficients(firstTangent);
            LinearFunctionCoefficients secondTangentCoefficients = FindLinearFunctionCoefficients(secondTangent);

            Point tangentsCrossPoint = FindLinearFunctionsCrossPoint(firstTangentCoefficients, secondTangentCoefficients);
            if (tangentsCrossPoint == null)
            {
                Console.Write("Your tangents are parallel"); //error handling should be added
                return null;
            }

            LinearFunctionCoefficients bisectorLinearCoefficients = FindLinearFunctionsBisector(firstTangentCoefficients,
                secondTangentCoefficients, tangentsCrossPoint);
            //can be use a different function
            float bisectorASquare = bisectorLinearCoefficients.a * bisectorLinearCoefficients.a;

            // comes from formula of radius center to tangent, ax+b
            float divideVal = firstTangentCoefficients.a * firstTangentCoefficients.a + 1f;

            // coefficients of square equation: a* x^2 + b * x + c = 0

            //ar, br, cr - right coefficients of equation
            float ar = bisectorASquare + 1;
            float br = 2 * bisectorLinearCoefficients.a * bisectorLinearCoefficients.b - 2 * coord.x - 2 * coord.y * bisectorLinearCoefficients.a;
            float cr = coord.x * coord.x + bisectorLinearCoefficients.b * bisectorLinearCoefficients.b -
                      2 * coord.y * bisectorLinearCoefficients.b + coord.y * coord.y;
            //al, bl, cl - left coefficients of equation
            float al = (firstTangentCoefficients.a * firstTangentCoefficients.a + bisectorASquare
                        - 2 * firstTangentCoefficients.a * bisectorLinearCoefficients.a) / divideVal;
            float bl = 2 * (bisectorLinearCoefficients.a * bisectorLinearCoefficients.b - firstTangentCoefficients.a * bisectorLinearCoefficients.b +
                            firstTangentCoefficients.a * firstTangentCoefficients.b - firstTangentCoefficients.b * bisectorLinearCoefficients.a) / divideVal;
            float cl = (bisectorLinearCoefficients.b * bisectorLinearCoefficients.b + firstTangentCoefficients.b * firstTangentCoefficients.b -
                        2 * firstTangentCoefficients.b * bisectorLinearCoefficients.b) / divideVal;
            //subtruct left and right equation
            float a = ar - al;
            float b = br - bl;
            float c = cr - cl;

            // it's parabolic function, has two answers, here is i get only the highest
            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0f)
            {
                //should be add error handling
                throw new InvalidOperationException("It can not be less than zero");
            }

            float centerX = (-b + (float)Math.Sqrt(discriminant)) / (2f * a);
            float centerY = bisectorLinearCoefficients.a * centerX + bisectorLinearCoefficients.b;

            float length = Math.Abs(firstTangentCoefficients.a * centerX - centerY + firstTangentCoefficients.b);
            float sqrt = (float)Math.Sqrt(divideVal);
            //radius: distance from center of circle to point and to tangent line
            float radius = length / sqrt;

            return new Circle(new Point(centerX, centerY), radius);
        }

        private static LinearFunctionCoefficients FindLinearFunctionCoefficients(Func<float, float> linearFunction)
        {
            if (linearFunction == null)
                return null;

            float a = linearFunction.Invoke(1f) - linearFunction.Invoke(0f);
            float b = linearFunction.Invoke(0f);

            return new LinearFunctionCoefficients(a, b);
        }
        /// <summary>
        /// find coordinate crossing of two linear functions
        /// </summary>
        /// <param name="firstCoefficients"></param>
        /// <param name="secondCoefficients"></param>
        /// <returns></returns>
        private static Point FindLinearFunctionsCrossPoint(LinearFunctionCoefficients firstCoefficients, LinearFunctionCoefficients secondCoefficients)
        {
            if (firstCoefficients.a - secondCoefficients.a == 0f)
                return null;

            float x = (secondCoefficients.b - firstCoefficients.b) / (firstCoefficients.a - secondCoefficients.a);
            float y = firstCoefficients.a * x + firstCoefficients.b;
            return new Point(x, y);
        }

        /// <summary>
        /// find coordinate bisector of two linear functions
        /// </summary>
        /// <param name="firstCoefficients"></param>
        /// <param name="secondCoefficients"></param>
        /// <param name="crossPoint"></param>
        /// <returns></returns>
        private static LinearFunctionCoefficients FindLinearFunctionsBisector(LinearFunctionCoefficients firstCoefficients,
            LinearFunctionCoefficients secondCoefficients, Point crossPoint)
        {
            float tangentCoefficientsMultiple = firstCoefficients.a * secondCoefficients.a;
            if (Math.Abs(tangentCoefficientsMultiple - 1f) < 0.001f)
            {
                //Specific case : tg is undefined should use error handling
                throw new InvalidOperationException("Tangent is undefined");
            }

            //tan(a+ß) = tana +tanß /1-tana+tanß
            float sumTangentCoefficient = (firstCoefficients.a + secondCoefficients.a) / (1f - tangentCoefficientsMultiple);
            float sumAngle = (float)Math.Atan(sumTangentCoefficient);
            bool isAngleApproximateZero = Math.Equals(sumAngle, 0f);
            float bisectorTangentCoefficient = (float)((!isAngleApproximateZero) ? (1f - Math.Cos(sumAngle)) / Math.Sin(sumAngle) : 0f);


            float b = crossPoint.y - bisectorTangentCoefficient * crossPoint.x;

            return new LinearFunctionCoefficients(bisectorTangentCoefficient, b);
        }
    }
}
