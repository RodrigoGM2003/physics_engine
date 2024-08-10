using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Numerics;


namespace PhysicsEngine
{   
    /**
     * Abstract class for all colliders in the simulation
     */ 
    public abstract class Collider
    {
        public Vector2f Position { get; protected set; } // Position in m
        public Vector2f LastPosition { get; protected set; } // Last position in m (used to calculate swept AABB)
        public float Rotation { get; protected set; } // Rotation in rad
        public float LastRotation { get; protected set; } // Last rotation in rad (used to calculate swept AABB)
        public float Elasticity { get; protected set; } // The elasticity of the object
        public float Friction { get; protected set; } // The friction of the object
        public FloatRect BoundingBox { get; protected set; } // The bounding box of the object (used to increase performance in collision detection)
        public FloatRect SweptAABB { get; protected set; } // The swept AABB of the object (used to calculate collision response)

        /**
         * Base constructor for the Collider class
         * @param position The position of the object in the scene
         * @param elasticity The elasticity of the object
         * @param friction The friction of the object
         */
        protected Collider(Vector2f position, float? rotation = 0, float? elasticity = null, float? friction = null)
        {
            Position = position;
            LastPosition = position;

            Rotation = rotation ?? 0;
            LastRotation = rotation ?? 0;
            
            Elasticity = elasticity ?? PhysicsConstants.DefaultElasticity;
            Friction = friction ?? PhysicsConstants.DefaultFriction;
        }

        /**
         * Update the position of the object
         * @param position The new position of the object
         */
        public abstract void UpdatePosition(Vector2f position, float rotation);
        

        /**
         * Update the swept AABB of the object
         */
        public void UpdateSweptAABB(){

            float minX = Math.Min(Position.X - BoundingBox.Width / 2, LastPosition.X - BoundingBox.Width / 2);
            float minY = Math.Min(Position.Y - BoundingBox.Height / 2, LastPosition.Y - BoundingBox.Height / 2);
            float maxX = Math.Max(Position.X + BoundingBox.Width / 2, LastPosition.X + BoundingBox.Width / 2);
            float maxY = Math.Max(Position.Y + BoundingBox.Height / 2, LastPosition.Y + BoundingBox.Height / 2);

            SweptAABB = new FloatRect(minX, minY, maxX - minX, maxY - minY);
        }


        public static float CalculateToI(in CircleCollider a, in CircleCollider b, Vector2f vA, Vector2f vB){
            //Calculate the relative velocity and position
            Vector2f relativeVelocity = vA - vB;
            Vector2f relativePosition = a.LastPosition - b.LastPosition;

            //Calculate the combined radius
            float combinedRadius = a.Radius + b.Radius;

            //Solve the quadratic equation
            //v1.X * v2.X + v1.Y * v2.Y;
            float aCoeff = relativeVelocity.Dot(relativeVelocity);
            float bCoeff = 2 * relativePosition.Dot(relativeVelocity);
            float cCoeff =  relativePosition.Dot(relativePosition) - combinedRadius * combinedRadius;
            float discriminant = bCoeff * bCoeff - 4 * aCoeff * cCoeff;

            //If the discriminant is negative, there is no collision
            if (discriminant < 0)
                return float.MaxValue;

            //Calculate the time of impact
            float sqrtDiscriminant = MathF.Sqrt(discriminant);
            float t1 = (-bCoeff - sqrtDiscriminant) / (2 * aCoeff);
            float t2 = (-bCoeff + sqrtDiscriminant) / (2 * aCoeff);

            // Return the smallest positive time of impact
            if (t1 >= 0 && t1 <= 1){
                Console.WriteLine("Ladys and gentlemen, we got him");
                return t1;
            }

            else if (t2 >= 0 && t2 <= 1){
                Console.WriteLine("Gadys and lentlemen, we got him");
                return t2;
            }

            //If there is no collision, return infinity
            return float.MaxValue;
        }

        /**
        * Check for real collision between two colliders
        * @param other The other collider
        */
        public bool Intersects(in Collider other, out Vector2f normal, out float depth){
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is CircleCollider){
                return this.Intersects(other as CircleCollider, out normal, out depth);
            }
            else if (other is PolygonCollider)
                return this.Intersects(other as PolygonCollider, out normal, out depth);
            else
                throw new NotImplementedException();
        }
        public abstract bool Intersects(in CircleCollider other, out Vector2f normal, out float depth);
        public abstract bool Intersects(in PolygonCollider other, out Vector2f normal, out float depth);



    }
}