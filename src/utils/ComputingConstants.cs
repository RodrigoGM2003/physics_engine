using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Globalization;

namespace PhysicsEngine
{
    public static class ComputingConstants
    {
        public const float FrameRate = 60.0f; // The frame rate of the simulation
        public const float AspectRatio = 4.0f/3.0f; // The aspect ratio of the window
        public const int DefaultSubsteps = 2; // The default number of substeps
        
        public const uint CirclePointCount = 30;

        public static readonly CultureInfo CultureInfo = CultureInfo.InvariantCulture;    
    }
}