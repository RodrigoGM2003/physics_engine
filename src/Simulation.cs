using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace PhysicsEngine
{
    class Simulation
    {
        /**
         * Main method for the simulation
         * @param args The command line arguments
         */
        static void Main(string[] args)
        {
            var sim = new Simulator("scenes/verlet_simple.scn");


            var body = new RectangleRigidBody(
                window: sim.window,
                size: new Vector2f(30, 4),
                color: Color.Red,
                mass: 1.0f,
                rotation: 1.0f,
                elasticity: 0.5f,
                friction: 0.5f,
                solid: true,
                isStatic: true
            );

            sim.AddBody(body, new Vector2f(sim.window.Size.X / (2 * PhysicsConstants.PixelsPerMeter), 
                                    sim.window.Size.Y / (2 * PhysicsConstants.PixelsPerMeter) + 10));

            sim.Run();

        }
    }
}
