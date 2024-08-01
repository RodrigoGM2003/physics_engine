using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Numerics;

namespace PhysicsEngine
{
    class Simulation
    {
        // private static readonly float AspectRatio = 16.0f / 9.0f;  // Desired aspect ratio (e.g., 4:3)
        private static float AspectRatio = 1.0f / 1.0f;  // Desired aspect ratio (e.g., 4:3)
        private static readonly float FrameTime = 1.0f / 144.0f;   // Frame time for 60 FPS
        private static RenderWindow window = null!; // The window to draw to
        private static Clock clock = null!; // The clock to keep track of time
        private static float accumulatedTime = 0.0f; // The time that has accumulated since the last frame

        /**
         * Main method for the simulation
         * @param args The command line arguments
         */
        static void Main(string[] args)
        {
            // Create the window
            window = new RenderWindow(new VideoMode(1000, 750), "SFML.NET Window");
            window.Closed += OnWindowClosed;
            window.Resized += OnWindowResized;

            Shape shape = new CircleShape(10);
            Drawer drawer = new CircleDrawer(window, radius: 10, color: Color.White);

            // Create the bodies
            Body[] Bodies = new Body[5];
            
            Bodies[0] = new RigidBody(collider: new CircleCollider(new Vector2f(100, 100), 10), 
                                      drawer: drawer, velocity: new Vector2f(1, -10));
            Bodies[1] = new RigidBody(collider: new CircleCollider(new Vector2f(100, 100), 10),
                                      drawer: drawer, velocity: new Vector2f(2, -10));
            Bodies[2] = new RigidBody(collider: new CircleCollider(new Vector2f(100, 100), 10), 
                                      drawer: drawer, velocity: new Vector2f(3, -10));
            Bodies[3] = new RigidBody(collider: new CircleCollider(new Vector2f(100, 100), 10), 
                                      drawer: drawer, velocity: new Vector2f(4, -10)); 
            Bodies[4] = new RigidBody(collider: new CircleCollider(new Vector2f(100, 100), 10),
                                      drawer: drawer, velocity: new Vector2f(5, -10));


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
            foreach (Body b in Bodies)
                b.Start(new Vector2f(0, 6f));
        }

        /**
         * Update all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private static void Update(ref Body[] Bodies, float deltaTime)
        {
            foreach (Body b in Bodies)
                b.Update(deltaTime);
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
