using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    /**
    * Class for resolving collisions between rigid bodies
    */
    public class CollisionResolver
    {
        /**
        * Method to resolve a collision between two circle rigid bodies
        * @param circleA The first circle rigid body
        * @param circleB The second circle rigid body
        * @param toi The time of impact
        * @param deltaTime The change in time since the last frame
        */
        public static void ResolveCollision(CircleRigidBody circleA, CircleRigidBody circleB, float toi, float deltaTime)
        {
            circleA.Velocity = Vector2fExtensions.Hadmard(circleA.Velocity, new Vector2f(-1, 1));
            circleB.Velocity = Vector2fExtensions.Hadmard(circleB.Velocity, new Vector2f(-1, 1));
            // // Move the bodies to the point of collision
            // Vector2f newPosA = circleA.Position + circleA.Velocity * toi * deltaTime;
            // Vector2f newPosB = circleB.Position + circleB.Velocity * toi * deltaTime;

            // circleA.RBCollider.UpdatePosition(newPosA, circleA.Rotation);
            // circleB.RBCollider.UpdatePosition(newPosB, circleB.Rotation);

            // // Calculate collision normal
            // Vector2f collisionNormal = newPosB - newPosA;
            // collisionNormal = collisionNormal / collisionNormal.Length();

            // // Calculate relative velocity
            // Vector2f relativeVelocity = circleA.Velocity - circleB.Velocity;

            // // Calculate relative velocity in terms of the normal direction
            // float velocityAlongNormal = relativeVelocity.Dot(collisionNormal);

            // // Do not resolve if velocities are separating
            // if (velocityAlongNormal > 0)
            //     return;

            // // Calculate restitution
            // float restitution = MathF.Min(circleA.RBCollider.Elasticity, circleB.RBCollider.Elasticity);

            // // Calculate impulse scalar
            // float impulseScalar = -(1 + restitution) * velocityAlongNormal;
            // impulseScalar /= (1 / circleA.Mass + 1 / circleB.Mass);

            // // Apply impulse
            // Vector2f impulse = impulseScalar * collisionNormal;

            // circleA.Velocity -= impulse / circleA.Mass;
            // circleB.Velocity += impulse / circleB.Mass;

            // // Angular velocity adjustments
            // Vector2f contactPointA = newPosA + collisionNormal * circleA.Radius;
            // Vector2f contactPointB = newPosB - collisionNormal * circleB.Radius;

            // float angularImpulseA = (contactPointA - newPosA).Cross(impulse);
            // float angularImpulseB = (contactPointB - newPosB).Cross(impulse);

            // circleA.AngularVelocity -= angularImpulseA / circleA.Mass;
            // circleB.AngularVelocity += angularImpulseB / circleB.Mass;
        }
    }
}