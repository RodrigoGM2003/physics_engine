using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    /**
     * Abstract class for all collisionable objects in the simulation
     */
    public abstract class Body
    {
        public Vector2f Position { get; protected set; } // Position in m
        public float Rotation { get; protected set; } // Rotation in grads


        /**
         * Start method for the object
         * @param position The position of the object in the scene at start
         */
        public abstract void Start(Vector2f position); // Called when the object is created in the scene only once

        /**
         * Update the state of the object
         * @param dt The change in time since the last frame
         */
        public abstract void Update(in float deltaTime);

        /**
         * Draw the object to the screen
         */
        public abstract void Draw();
    }
}