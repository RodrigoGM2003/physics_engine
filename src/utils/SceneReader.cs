using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace PhysicsEngine
{
    public class SceneReader
    {
        public string scene { get; set; } = ""; // The name of the scene

        public SceneReader(in string scene)
        {
            this.scene = scene;
        }

        public void ReadScene()
        {
            // Read the scene from the file
            // Parse the scene
            // Create the bodies
            // Create the objects
            // Create the collision manager
            // Create the collision resolver
            // Create the simulator
            // Run the simulator
        }
    }
}