using System;
using System.Numerics;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    public static class ComputingConstants
    {
        public const float FrameRate = 60.0f; // The frame rate of the simulation
        public const float AspectRatio = 2.0f / 1.0f; // The aspect ratio of the window
        public const int DefaultSubsteps = 8; // The default number of substeps
        


        public const uint CirclePointCount = 30;  
    }
}