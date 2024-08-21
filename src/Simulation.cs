using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Runtime.CompilerServices;

namespace PhysicsEngine
{
    public static class RenderWindowManager
    {
        public static RenderWindow window { get; set; } = null!;
                /**
         * Event handler for when the window is closed
         * @param sender The object that sent the event
         * @param e The event arguments
         */
        public static void OnWindowClosed(object? sender, EventArgs e)
        {
            window.Close();
        }

        /**
         * Event handler for when the window is resized
         * @param sender The object that sent the event
         * @param e The event arguments
         */
        public static void OnWindowResized(object? sender, SizeEventArgs e)
        {
            // Maintain aspect ratio
            float windowAspect = e.Width / (float)e.Height;
            float viewWidth = window.Size.X;
            float viewHeight = window.Size.Y;
            View view = window.GetView();


            // Window is too wide
            if (windowAspect > ComputingConstants.AspectRatio)
            {
                float newWidth = viewHeight * windowAspect;
                view.Viewport = new FloatRect((1.0f - newWidth / viewWidth) / 2.0f, 0.0f, newWidth / viewWidth, 1.0f);
            }
            // Window is too tall
            else
            {
                float newHeight = viewWidth / windowAspect;
                view.Viewport = new FloatRect(0.0f, (1.0f - newHeight / viewHeight) / 2.0f, 1.0f, newHeight / viewHeight);
            }

            window.SetView(view);
        }
    }
    class Simulation
    {
        private static float AspectRatio = ComputingConstants.AspectRatio;  // Desired aspect ratio (e.g., 4:3)
        private static readonly float FrameTime = 1.0f / ComputingConstants.FrameRate;   // Frame time for 60 FPS
        private static RenderWindow window = null!; // The window to draw to
        private static Clock clock = null!; // The clock to keep track of time
        private static double accumulatedTime = 0.0f; // The time that has accumulated since the last frame
        private static CollisionManager collisionManager;
        private static CollisionResolver collisionResolver;
        private static float speedFactor = 1f; // The speed factor of the simulation
        
        private static int frames = 0;
        public static int substeps = 8;
        private static float trueFPS = 0;

        private static int collisions = 0;
        public static bool check = false;
        public static int usedBodies = 0;


        /**
         * Main method for the simulation
         * @param args The command line arguments
         */
        static void Main(string[] args)
        {
            var sim = new Simulator("scenes/scene1.scn");
            sim.Run();
            // uint width = 750;

            // // Create the window
            // // window = new RenderWindow(new VideoMode(width, (uint)(width / ComputingConstants.AspectRatio)), "SFML.NET Window");
            // window = new RenderWindow(new VideoMode((uint)(width * ComputingConstants.AspectRatio), width), "SFML.NET Window");
            // // window.SetFramerateLimit(60);
            // RenderWindowManager.window = window;
            // window.Closed += RenderWindowManager.OnWindowClosed;
            // window.Resized += RenderWindowManager.OnWindowResized;



            // // Create the bodies
            // RigidBody[] Bodies = new RigidBody[2500];
            // // RigidBody[] Bodies = new RigidBody[2000];

            // Bodies[0] = new RectangleRigidBody(
            //     size: new Vector2f(100, 20),
            //     window: window, 
            //     color: Color.Black, 
            //     mass: 1f, 
            //     velocity: new Vector2f(0, 0),
            //     angularVelocity: 0f,
            //     elasticity: 1f,
            //     friction: 0f,
            //     rotation: 0,
            //     solid: false,
            //     isStatic: true
            // );
            // Bodies[1] = new RectangleRigidBody(
            //     size: new Vector2f(100, 20),
            //     window: window, 
            //     color: Color.Black, 
            //     mass: 1f, 
            //     velocity: new Vector2f(0, 0),
            //     angularVelocity: 0f,
            //     elasticity: 1f,
            //     friction: 0f,
            //     rotation: 0,
            //     solid: false,
            //     isStatic: true
            // );
            // Bodies[2] = new RectangleRigidBody(
            //     size: new Vector2f(20, 75),
            //     window: window, 
            //     color: Color.Black, 
            //     mass: 1f, 
            //     velocity: new Vector2f(0, 0),
            //     angularVelocity: 0f,
            //     elasticity: 1f,
            //     friction: 0f,
            //     rotation: 0,
            //     solid: false,
            //     isStatic: true
            // );
            // Bodies[3] = new RectangleRigidBody(
            //     size: new Vector2f(20, 75),
            //     window: window, 
            //     color: Color.Black, 
            //     mass: 1f, 
            //     velocity: new Vector2f(0, 0),
            //     angularVelocity: 0f,
            //     elasticity: 1f,
            //     friction: 0f,
            //     rotation: 0,
            //     solid: false,
            //     isStatic: true
            // );
            // // Bodies[4] = new RectangleRigidBody(
            // //     size: new Vector2f(5, 40),
            // //     window: window,
            // //     color: Color.Red, 
            // //     mass: 1f, 
            // //     velocity: new Vector2f(0, 0),
            // //     angularVelocity: MathF.PI,
            // //     elasticity: 1f,
            // //     friction: 0f,
            // //     rotation: 1,
            // //     solid: false,
            // //     isStatic: true
            // // );
            
            // usedBodies = 4;
            // collisionResolver = new DCD();
            // collisionManager = new CollisionManager(Bodies, usedBodies, discrete: collisionResolver.Discrete);

            // // Start the bodies
            // Start(ref Bodies);

            // // Create the clock
            // clock = new Clock();

            // float substepTime = FrameTime / substeps;
            // // Main loop
            // while (window.IsOpen)
            // {
            //     // Update the time
            //     double deltaTime = clock.Restart().AsSeconds();
            //     accumulatedTime += deltaTime;

            //     //--------------------------------------------------------------------------------
            //     //Every .5 seconds add a new body
            //     if(frames % 15 == 0 && usedBodies < 200){
            //         Bodies[usedBodies] = new CircleRigidBody(
            //             // float random radius between 1 and 4
            //             radius: 2f,
            //             window: window,
            //             // Random color
            //             color: new Color((byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255)),
            //             mass: 1,
            //             // Random velocity between -20 and 20
            //             velocity: new Vector2f(-20, -20),
            //             // velocity: new Vector2f((float)new Random().Next(10, 30), (float)new Random().Next(10, 30)),
            //             angularVelocity: 0,
            //             elasticity: .7f,
            //             friction: 0.1f
            //         );
            //         Bodies[usedBodies].Start(new Vector2f(85f, 40f));
            //         usedBodies++;
            //     }
            //     //--------------------------------------------------------------------------------

            //     // Update the scene
            //     while (accumulatedTime >= FrameTime )
            //     {   
            //         window.DispatchEvents();

            //         for (int i = 0; i < substeps; i++)
            //             // Update(ref Bodies, substepTime * speedFactor);
            //             Update(ref Bodies, (float)substepTime * speedFactor);

            //         accumulatedTime -= FrameTime;
            //     }

            //     // Clear the window
            //     window.Clear(Color.Black);

            //     // Draw the scene
            //     Draw(ref Bodies);
            //     frames++;
            //     window.Display();

            //     // Sleep if the frame time is too fast
            //     double elapsed = clock.ElapsedTime.AsSeconds();
            //     if (elapsed > FrameTime)
            //         accumulatedTime += FrameTime;
            //     else
            //         accumulatedTime += elapsed;
                
            //     clock.Restart();

            //     // if(frames % ComputingConstants.FrameRate == 0){
            //     //     Console.WriteLine("----------------------------------------------");
            //     //     Console.WriteLine("Frames: " + frames);
            //     //     Console.WriteLine("Sim FPS: " + 1.0f / elapsed);
            //     //     Console.WriteLine("DeltaTime: " + deltaTime);
            //     //     Console.WriteLine("Elapsed: " + elapsed * 1000 + "ms");
            //     //     Console.WriteLine("Spare Time: " + (FrameTime - elapsed) * 1000 + "ms");

            //     // }
            //     while (elapsed < FrameTime)
            //         elapsed = clock.ElapsedTime.AsSeconds();


            //     if(frames % ComputingConstants.FrameRate == 0)
            //     {
            //     //     Console.WriteLine("Time elapsed after sleep: " + elapsed * 1000 + "ms");
            //     //     Console.WriteLine("Number of bodies: " + usedBodies);
            //         Console.WriteLine("True FPS: " + 1.0f / elapsed);
            //     //     Console.WriteLine("----------------------------------------------");
            //     }

            //     // if(1.0f / elapsed < ComputingConstants.FrameRate - 1){
            //     //     speedFactor = ComputingConstants.FrameRate / (float)(1.0f / elapsed);
            //     //     // Console.WriteLine("FPS is too low");
            //     // }
            //     // else
            //     //     speedFactor = 1f;
            //}

        }

        /**
         * Start all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private static void Start(ref RigidBody[] Bodies)
        {
            Bodies[0].Start(new Vector2f(50, 0f));
            Bodies[1].Start(new Vector2f(50f, 75f));
            Bodies[2].Start(new Vector2f(0f, 75/2));
            Bodies[3].Start(new Vector2f(100f, 75/2));
            // Bodies[4].Start(new Vector2f(50f, 75/2f + 15));
        }

        /**
         * Update all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private static void Update(ref RigidBody[] Bodies, float deltaTime)
        {
            for(int i = 0; i < usedBodies; i++)
                Bodies[i].Update(deltaTime);


            collisionManager.UpdateBVH(Bodies, usedBodies);

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
        private static void Draw(ref RigidBody[] Bodies)
        {
            for(int i = 0; i < usedBodies; i++)
                Bodies[i].Draw();
            


            // collisionManager.root.Draw();
        }
    }
}
