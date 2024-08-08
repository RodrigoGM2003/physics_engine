using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Numerics;


namespace PhysicsEngine
{   
    /**
     * Class for all circle colliders in the simulation
     */ 
    public class CircleCollider : Collider
    {
        public float Radius { get; protected set; } // The radius of the circle

        /**
        * Base constructor for CircleCollider class
        * @param position The position of the object in the scene
        * @param radius The radius of the object
        * @param rotation The rotation of the object
        * @param elasticity The elasticity of the object
        * @param friction The friction of the object
        */
        public CircleCollider(Vector2f position, float radius, float? rotation = 0, float? elasticity = null, float? friction = null)
        : base(
            position: position, 
            elasticity: elasticity, 
            friction: friction, 
            rotation: rotation
        )
        {
            Radius = radius;

            BoundingBox = new FloatRect(Position.X - Radius, Position.Y - Radius, 2 * Radius, 2 * Radius);        
        }

        /**
        * Update the position of the object
        * @param position The new position of the object
        */
        public override void UpdatePosition(Vector2f position, float rotation){
            LastPosition = Position;
            Position = position;

            LastRotation = Rotation;
            Rotation = rotation;         

            BoundingBox = new FloatRect(Position.X - Radius, Position.Y - Radius, 2 * Radius, 2 * Radius);        
        }


        public override bool Intersects(in CircleCollider other)
        {
            Vector2f collisionNormal = Position - other.Position;
            float distance = collisionNormal.Length();

            return distance <= Radius + other.Radius;
        }
        public override bool Intersects(in PolygonCollider other)
        {
            return other.Intersects(this);
        }
    }
}