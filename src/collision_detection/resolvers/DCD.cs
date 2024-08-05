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
    public class DCD : CollisionResolver
    {
        public override bool Discrete => true; // Discrete collision detection is enabled
        /**
        * Method to resolve a collision between two circle rigid bodies
        * @param circleA The first circle rigid body
        * @param circleB The second circle rigid body
        * @param toi The time of impact
        * @param deltaTime The change in time since the last frame
        */
        public override void ResolveCollision(CircleRigidBody circleA, CircleRigidBody circleB)
        {
            Console.WriteLine("Position A: " + circleA.Position + " Position B: " + circleB.Position);
            Vector2f collisionNormal = circleA.Position - circleB.Position;
            float distance = collisionNormal.Length();
            collisionNormal /= distance; // Normalize the vector
            Console.WriteLine("Collision Normal: " + collisionNormal);

            // Calculate relative velocity
            Vector2f relativeVelocity = circleA.Velocity - circleB.Velocity;
            Console.WriteLine("Relative velocity: " + relativeVelocity);

            // Calculate relative velocity in terms of the normal direction
            float velocityAlongNormal = relativeVelocity.Dot(collisionNormal);
            Console.WriteLine("Velocity along normal: " + velocityAlongNormal);

            // Do not resolve if velocities are separating
            if (velocityAlongNormal > 0)
                return;

            // Calculate restitution
            float restitution = MathF.Min(circleA.RBCollider.Elasticity, circleB.RBCollider.Elasticity);

            // Calculate impulse scalar
            float impulseScalar = -(1 + restitution) * velocityAlongNormal;
            impulseScalar /= (1 / circleA.Mass + 1 / circleB.Mass);

            // Apply impulse
            Vector2f impulse = impulseScalar * collisionNormal;

            Console.WriteLine("A's Velocity before: " + circleA.Velocity + " B's Velocity before: " + circleB.Velocity);
            circleA.ApplyImpulse(impulse);
            circleB.ApplyImpulse(-impulse);
            Console.WriteLine("A's Velocity after: " + circleA.Velocity + " B's Velocity after: " + circleB.Velocity);
            Console.WriteLine("Velocity added to A: " + impulse / circleA.Mass + " Velocity added to B: " + impulse / circleB.Mass);

            // // Angular velocity adjustments
            Vector2f contactPointA = circleA.Position + collisionNormal * circleA.Radius;
            Vector2f contactPointB = circleB.Position - collisionNormal * circleB.Radius;

            float inertiaA = circleA.Mass * circleA.Radius * circleA.Radius / 2;
            float inertiaB = circleB.Mass * circleB.Radius * circleB.Radius / 2;

            float angularImpulseA = (contactPointA - circleA.Position).Cross(impulse) / inertiaA;
            float angularImpulseB = (contactPointB - circleB.Position).Cross(impulse) / inertiaB;

            circleA.AngularVelocity -= angularImpulseA;
            circleB.AngularVelocity += angularImpulseB;
        }

    }
}