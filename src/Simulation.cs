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
            var sim = new Simulator("scenes/simple.scn");



            sim.Run();

        }
    }
}
