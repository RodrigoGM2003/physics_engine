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
            Vector2f collisionNormal = circleA.Position - circleB.Position;
            float distance = collisionNormal.Length();
            collisionNormal /= distance; // Normalize the vector

            if (distance > circleA.Radius + circleB.Radius)
                return;

            // Eliminate overlap
            float overlap = (circleA.Radius + circleB.Radius) - distance;
            circleA.UpdatePosition(circleA.Position + ( overlap / 2 * collisionNormal));
            circleB.UpdatePosition(circleB.Position - ( overlap / 2 * collisionNormal));

            // Calculate relative velocity
            Vector2f relativeVelocity = circleA.Velocity - circleB.Velocity;

            // Calculate relative tangential velocity
            Vector2f tangentialVelocityA = new Vector2f(circleA.AngularVelocity * circleA.Radius * collisionNormal.Y, 
                                                        -circleA.AngularVelocity * circleA.Radius * collisionNormal.X);
            Vector2f tangentialVelocityB = new Vector2f(-circleB.AngularVelocity * circleB.Radius * collisionNormal.Y, 
                                                        circleB.AngularVelocity * circleB.Radius * collisionNormal.X);
            Vector2f relativeTangentialVelocity = tangentialVelocityA - tangentialVelocityB;


            // Calculate relative velocity in terms of the normal direction
            float velocityAlongNormal = relativeVelocity.Dot(collisionNormal);

            // Do not resolve if velocities are separating
            if (velocityAlongNormal > 0)
                return;

            // Calculate restitution
            float restitution = MathF.Min(circleA.Collider.Elasticity, circleB.Collider.Elasticity);
            float friction = MathF.Min(circleA.Collider.Friction, circleB.Collider.Friction);

            // Calculate impulse scalar
            float impulseScalar = -(1 + restitution) * velocityAlongNormal;
            impulseScalar /= (1 / circleA.Mass + 1 / circleB.Mass);

            // Calculate and apply final impulses
            Vector2f normalImpulse = impulseScalar * collisionNormal;  
            Vector2f frictionImpulse = relativeTangentialVelocity * friction;
            circleA.ApplyImpulse(normalImpulse + frictionImpulse);
            circleB.ApplyImpulse(-(normalImpulse + frictionImpulse));

            // Apply frictional torque
            float inertiaA = circleA.Mass * circleA.Radius * circleA.Radius / 2;
            float inertiaB = circleB.Mass * circleB.Radius * circleB.Radius / 2;
            float frictionTorqueA = frictionImpulse.Length() * circleA.Radius / inertiaA;
            float frictionTorqueB = frictionImpulse.Length() * circleB.Radius / inertiaB;
            circleA.AngularVelocity -= frictionTorqueA;
            circleB.AngularVelocity += frictionTorqueB;
        }

    }
}