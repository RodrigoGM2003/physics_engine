using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Numerics;
using System.Formats.Asn1;
using System.Diagnostics;
using System.Security.AccessControl;

namespace PhysicsEngine
{
    public class Simulator
    {
        public static readonly int MaxSubsteps = 128; // The maximum number of substeps to take per frame
        public static readonly int MinSubsteps = 1; // The minimum number of substeps to take per frame
        public required RigidBody[] Bodies { get; set; } = new RigidBody[1000]; // The array of bodies in the scene
        public int numBodies { get; set; } = 0; // The number of bodies in the scene
        // public required LogicObject[] Objects { get; set; } = new LogicObject[1000]; // The array of objects in the scene
        private string scene { get; set; } = ""; // The scene of the scene
        public required RenderWindow window { get; set; } // The window to draw to
        public required CollisionManager collisionManager { get; set; } // The collision manager for the scene
        public required CollisionResolver collisionResolver { get; set; } // The collision resolver for the scene
        public required Clock clock { get; set; } // The clock to keep track of time
        public float speedFactor { get; set; } = 1f; // The speed factor of the simulation
        public int substeps { get; set; } = 8; // The number of substeps to take per frame
        private float accumulatedTime { get; set; } = 0; // The time that has accumulated since the last frame
        // private static float AspectRatio = ComputingConstants.AspectRatio;  // Desired aspect ratio (e.g., 4:3)
        // private static readonly float FrameTime = 1.0f / ComputingConstants.FrameRate;   // Frame time for 60 FPS
        // private static RenderWindow window = null!; // The window to draw to
        // private static Clock clock = null!; // The clock to keep track of time
        // private static float accumulatedTime = 0.0f; // The time that has accumulated since the last frame
        // private static CollisionManager collisionManager;
        // private static CollisionResolver collisionResolver;
        // private static float speedFactor = 1f; // The speed factor of the simulation
        
        // private static int frames = 0;
        // public static int substeps = 8;
        // private static float trueFPS = 0;

        // private static int collisions = 0;

        public Simulator(RenderWindow window, CollisionManager collisionManager, CollisionResolver collisionResolver, 
                        float speedFactor = 1, int substeps = ComputingConstants.DefaultSubsteps)
        {
            this.window = window;
            window.Closed += RenderWindowManager.OnWindowClosed;
            window.Resized += RenderWindowManager.OnWindowResized;

            this.collisionManager = collisionManager;
            this.collisionResolver = collisionResolver;

            this.speedFactor = speedFactor;
            this.substeps = substeps;
        }

        public Simulator(in string sceneName)
        {
            throw new NotImplementedException();
        }


        public void Run()
        {
            Start();

            // Create the clock
            clock = new Clock();

            float frameTime = 1 / ComputingConstants.FrameRate;

            float substepTime = 1 / (substeps * ComputingConstants.FrameRate);
            // Main loop
            while (window.IsOpen)
            {
                // Update the time
                float deltaTime = clock.Restart().AsSeconds();
                accumulatedTime += deltaTime;


                // Update the scene
                while (accumulatedTime >= frameTime )
                {
                    window.DispatchEvents();

                    for (int i = 0; i < substeps; i++)
                        Update(substepTime * speedFactor);

                    accumulatedTime -= frameTime;
                }

                // Clear the window
                window.Clear(Color.Black);

                // Draw the scene
                Draw();
                window.Display();

                // Sleep if the frame time is too fast
                float elapsed = clock.Restart().AsSeconds();

                while (elapsed < frameTime)
                    elapsed = clock.ElapsedTime.AsSeconds();
            }

        }

        /**
         * Start all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private void Start()
        {
            throw new NotImplementedException();
        }

        /**
         * Update all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private void Update(float deltaTime)
        {
            for(int i = 0; i < numBodies; i++)
                Bodies[i].Update(deltaTime);

            collisionManager.UpdateBVH(Bodies);

            var potentialCollisions = collisionManager.GetPotentialCollisions();

            foreach (var (bodyA, bodyB) in potentialCollisions){

                if(bodyA.IsStatic && bodyB.IsStatic)
                    continue;

                Vector2f normal;
                float depth;
                if (bodyA.Collider.Intersects(bodyB.Collider, out normal, out depth))
                {
                    if(collisionResolver.HandleOverlap(bodyA, bodyB, normal, depth))
                        continue;
                        
                    collisionResolver.ResolveCollision(bodyA, bodyB, normal, depth);
                }
                
            }
        }

        /**
         * Draw all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private void Draw()
        {
            for(int i = 0; i < numBodies; i++)
                Bodies[i].Draw();
            


            // collisionManager.root.Draw();
        }
    }
}
