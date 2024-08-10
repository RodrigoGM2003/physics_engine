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
    public class CCD : CollisionResolver
    {
        public override bool Discrete => false; // Continuous collision detection is enabled
    }
}