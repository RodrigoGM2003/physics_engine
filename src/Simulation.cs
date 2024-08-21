using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Runtime.CompilerServices;

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
            var sim = new Simulator("scenes/simple_scene.scn");

            var body = new RectangleRigidBody(
                window: sim.window,
                size: new Vector2f(40.0f, 5.0f),
                color: Color.Red,
                mass: 1.0f,
                elasticity: 0.5f,
                friction: 0.5f,
                solid: true,
                isStatic: true
            );

            sim.AddBody(body, new Vector2f(sim.window.Size.X / 20, sim.window.Size.Y / 20 + 20));

            sim.Run();

        }
    }
}
