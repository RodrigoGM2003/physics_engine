using SFML.Graphics;
using SFML.System;


namespace PhysicsEngine
{
    public class Simulator
    {
        public static readonly int MaxSubsteps = 128; // The maximum number of substeps to take per frame
        public static readonly int MinSubsteps = 1; // The minimum number of substeps to take per frame

        public required RigidBody[] Bodies { get; set; } = new RigidBody[1000]; // The array of bodies in the scene
        public int numBodies { get; set; } = 0; // The number of bodies in the scene
        // public required LogicObject[] Objects { get; set; } = new LogicObject[1000]; // The array of objects in the scene

        public required RenderWindow window { get; set; } // The window to draw to

        public required CollisionManager collisionManager { get; set; } // The collision manager for the scene
        public required CollisionResolver collisionResolver { get; set; } // The collision resolver for the scene

        public float speedFactor { get; set; } = 1f; // The speed factor of the simulation
        public int substeps { get; set; } = 8; // The number of substeps to take per frame
        private double accumulatedTime { get; set; } = 0; // The time that has accumulated since the last frame

        /**
        * Base constructor for the Simulator class
        * @param window The window to draw to
        * @param collisionManager The collision manager for the scene
        * @param collisionResolver The collision resolver for the scene
        * @param speedFactor The speed factor of the simulation
        * @param substeps The number of substeps to take per frame
        */
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
            var sceneReader = new SceneReader(sceneName);
            sceneReader.ReadScene();
        }


        public void Run()
        {
            Start();

            // Create the clock
            var clock = new Clock();
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

                //If framerate is too low, add a delay to keep the framerate constant
                double elapsed = clock.ElapsedTime.AsSeconds();
                if (elapsed > frameTime)
                    accumulatedTime += frameTime;
                else
                    accumulatedTime += elapsed;
                
                clock.Restart();

                // Sleep if the frame time is too fast
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

            collisionManager.UpdateBVH(Bodies, usedBodiesCount: numBodies);

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
