// using System;
// using SFML.Graphics;
// using SFML.Window;
// using SFML.System;
// using System.Data;


// namespace PhysicsEngine
// {   
//     /**
//      * Class for all circle colliders in the simulation
//      */ 
//     public class RectangleCollider : Collider
//     {
//         public Vector2f Size { get; protected set; } // The size of the rectangle
//         private float maxDimension;

//         /**
//         * Base constructor for RectangleCollider class
//         * @param position The position of the object in the scene
//         * @param size The size of the rectangle
//         * @param rotation The rotation of the object
//         * @param elasticity The elasticity of the object
//         * @param friction The friction of the object
//         */
//         public RectangleCollider(Vector2f position, Vector2f size, float? rotation = 0, float? elasticity = null, float? friction = null)
//         : base(
//             position: position, 
//             elasticity: elasticity, 
//             friction: friction, 
//             rotation: rotation
//         )
//         {
//             Size = size;
//             maxDimension = Math.Max(Size.X, Size.Y);
//             BoundingBox = new FloatRect(Position.X - Size.X / 2, Position.Y - Size.Y / 2, Size.X, Size.Y);
//         }

//         /**
//         * Update the position of the object
//         * @param position The new position of the object
//         * @param rotation The new rotation of the object
//         */
//         public override void UpdatePosition(Vector2f position, float rotation){
//             LastPosition = Position;
//             Position = position;

//             LastRotation = Rotation;
//             Rotation = rotation;

//             BoundingBox = new FloatRect(Position.X - maxDimension / 2, Position.Y - maxDimension / 2, maxDimension, maxDimension);
//         }

//         public new void UpdateSweptAABB(){
//             float minX = Math.Min(Position.X - maxDimension / 2, LastPosition.X - maxDimension / 2);
//             float minY = Math.Min(Position.Y - maxDimension / 2, LastPosition.Y - maxDimension / 2);
//             float maxX = Math.Max(Position.X + maxDimension / 2, LastPosition.X + maxDimension / 2);
//             float maxY = Math.Max(Position.Y + maxDimension / 2, LastPosition.Y + maxDimension / 2);

//             SweptAABB = new FloatRect(minX, minY, maxX - minX, maxY - minY);
//         }


//         /**
//         * Resolve the collision between the object and another object
//         * @param other The collider to resolve the collision with
//         */
//         public override void ResolveCollision(in CircleCollider other){

//         }
//         public override void ResolveCollision(in RectangleCollider other){

//         }
//         public override void ResolveCollision(in PolygonCollider other){

//         }

//     }
// }