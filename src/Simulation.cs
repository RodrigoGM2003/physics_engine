using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Numerics;

namespace PhysicsEngine
{
    public static class RenderWindowManager
    {
        public static RenderWindow Window { get; set; } = null!;
    }
    class Simulation
    {
        // private static readonly float AspectRatio = 16.0f / 9.0f;  // Desired aspect ratio (e.g., 4:3)
        private static float AspectRatio = ComputingConstants.AspectRatio;  // Desired aspect ratio (e.g., 4:3)
        private static readonly float FrameTime = 1.0f / ComputingConstants.FrameRate;   // Frame time for 60 FPS
        private static RenderWindow window = null!; // The window to draw to
        private static Clock clock = null!; // The clock to keep track of time
        private static float accumulatedTime = 0.0f; // The time that has accumulated since the last frame
        private static CollisionManager collisionManager;


        /**
         * Main method for the simulation
         * @param args The command line arguments
         */
        static void Main(string[] args)
        {
            // Create the window
            window = new RenderWindow(new VideoMode(1000, 750), "SFML.NET Window");
            RenderWindowManager.Window = window;
            window.Closed += OnWindowClosed;
            window.Resized += OnWindowResized;


            // Create the bodies
            Body[] Bodies = new Body[2];
            
            Bodies[0] = new CircleRigidBody(radius: 1f, window: window, color: Color.White, 
                                            mass: 1, velocity: new Vector2f(10, -20));

            Bodies[1] = new CircleRigidBody(radius: 1f, window: window, color: Color.White, 
                                mass: 1, velocity: new Vector2f(-10, -20));

            // Bodies[1] = new RectangleRigidBody(size: new Vector2f(4, 2), window: window, color: Color.Red, 
            //                                 mass: 1, velocity: new Vector2f(20, -20));

            // Bodies[2] = new PolygonRigidBody(vertices: new Vector2f[] { new Vector2f(0, 0), new Vector2f(2, 0), new Vector2f(1, 2) }, 
            //                                 window: window, color: Color.Yellow, mass: 1, velocity: new Vector2f(30, -20));

            // Bodies[3] = new CircleRigidBody(radius: 30, window: window, color: Color.Blue, 
            //                                 mass: 10, velocity: new Vector2f(4, -10));

            // Bodies[4] = new RectangleRigidBody(size: new Vector2f(80, 20), window: window, color: Color.Green, 
            //                                 mass: 1000, velocity: new Vector2f(5, -10));


            collisionManager = new CollisionManager(Bodies.Cast<RigidBody>().ToArray());
            // Start the bodies
            Start(ref Bodies);

            // Create the clock
            clock = new Clock();

            // Main loop
            while (window.IsOpen)
            {
                // Update the time
                float deltaTime = clock.Restart().AsSeconds();
                accumulatedTime += deltaTime;

                // Update the scene
                while (accumulatedTime >= FrameTime)
                {
                    window.DispatchEvents();
                    Update(ref Bodies, FrameTime);
                    accumulatedTime -= FrameTime;
                }

                // Clear the window
                window.Clear(Color.Black);

                // Draw the scene
                Draw(ref Bodies);
                window.Display();

                // Sleep if the frame time is too fast
                float elapsed = clock.Restart().AsSeconds();
                if (elapsed < FrameTime)
                    System.Threading.Thread.Sleep((int)((FrameTime - elapsed) * 1000));
            }
        }

        /**
         * Event handler for when the window is closed
         * @param sender The object that sent the event
         * @param e The event arguments
         */
        private static void OnWindowClosed(object? sender, EventArgs e)
        {
            window.Close();
        }

        /**
         * Event handler for when the window is resized
         * @param sender The object that sent the event
         * @param e The event arguments
         */
        private static void OnWindowResized(object? sender, SizeEventArgs e)
        {
            // Maintain aspect ratio
            float windowAspect = e.Width / (float)e.Height;
            float viewWidth = window.Size.X;
            float viewHeight = window.Size.Y;
            View view = window.GetView();


            // Window is too wide
            if (windowAspect > AspectRatio)
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

        /**
         * Start all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private static void Start(ref Body[] Bodies)
        {
            Bodies[0].Start(new Vector2f(0, 60f));
            Bodies[1].Start(new Vector2f(60f, 60f));
            // foreach (Body b in Bodies)
            //     b.Start(new Vector2f(0, 60f));
        }

        /**
         * Update all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private static void Update(ref Body[] Bodies, float deltaTime)
        {
            foreach (Body b in Bodies)
                b.Update(deltaTime);

            collisionManager.UpdateBVH(Bodies.Cast<RigidBody>().ToArray());

            var potentialCollisions = collisionManager.GetPotentialCollisions();

            if (potentialCollisions.Count > 0)
                Console.WriteLine("Collision count: " + potentialCollisions.Count);

            if (potentialCollisions.Count == 0)
                Console.WriteLine("No");
            
        }

        /**
         * Draw all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private static void Draw(ref Body[] Bodies)
        {
            foreach (Body b in Bodies)
                b.Draw();
        }
    }
}
