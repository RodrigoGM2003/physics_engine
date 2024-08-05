using SFML.System;

namespace PhysicsEngine
{
    /**
    * Utility extension methods for the Vector2f class
    */
    public static class Vector2fExtensions
    {
        /**
        * Element-wise multiplication of two vectors
        * @param v1 The first vector
        * @param v2 The second vector
        * @return The element-wise multiplication of the two vectors
        */
        public static Vector2f Hadmard(this Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.X * v2.X, v1.Y * v2.Y);
        }
        /**
        * Method to calculate the dot product of two vectors
        * @param v1 The first vector
        * @param v2 The second vector
        * @return The dot product of the two vectors
        */
        public static float Dot(this Vector2f v1, Vector2f v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        /**
        * Method to calculate the cross product of two vectors
        * @param v1 The first vector
        * @param v2 The second vector
        * @return The cross product of the two vectors
        */
        public static float Cross(this Vector2f v1, Vector2f v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        /**
        * Method to calculate the length of a vector
        * @param v The vector
        * @return The length of the vector
        */
        public static float Length(this Vector2f v)
        {
            return (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }
    }
}