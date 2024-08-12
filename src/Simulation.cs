using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Numerics;
using System.Formats.Asn1;
using System.Diagnostics;

namespace PhysicsEngine
{
    public static class RenderWindowManager
    {
        public static RenderWindow Window { get; set; } = null!;
    }
    class Simulation
    {
        private static float AspectRatio = ComputingConstants.AspectRatio;  // Desired aspect ratio (e.g., 4:3)
        private static readonly float FrameTime = 1.0f / ComputingConstants.FrameRate;   // Frame time for 60 FPS
        private static RenderWindow window = null!; // The window to draw to
        private static Clock clock = null!; // The clock to keep track of time
        private static float accumulatedTime = 0.0f; // The time that has accumulated since the last frame
        private static CollisionManager collisionManager;
        private static CollisionResolver collisionResolver;
        private static float speedFactor = 1f; // The speed factor of the simulation
        
        private static int frames = 0;
        private static float trueFPS = 0;
        private static bool keepAdding = true;

        private static int collisions = 0;


        /**
         * Main method for the simulation
         * @param args The command line arguments
         */
        static void Main(string[] args)
        {
            // Create the window
            window = new RenderWindow(new VideoMode(1000, 750), "SFML.NET Window");
            // window.SetFramerateLimit(60);
            RenderWindowManager.Window = window;
            window.Closed += OnWindowClosed;
            window.Resized += OnWindowResized;


            // Create the bodies
            RigidBody[] Bodies = new RigidBody[4];

            Bodies[0] = new RectangleRigidBody(
                size: new Vector2f(100, 20),
                window: window, 
                color: Color.Black, 
                mass: 1f, 
                velocity: new Vector2f(0, 0),
                angularVelocity: 0f,
                elasticity: 1f,
                friction: 0f,
                rotation: 0,
                solid: false,
                isStatic: true
            );
            Bodies[1] = new RectangleRigidBody(
                size: new Vector2f(100, 20),
                window: window, 
                color: Color.Black, 
                mass: 1f, 
                velocity: new Vector2f(0, 0),
                angularVelocity: 0f,
                elasticity: 1f,
                friction: 0f,
                rotation: 0,
                solid: false,
                isStatic: true
            );
            Bodies[2] = new RectangleRigidBody(
                size: new Vector2f(20, 75),
                window: window, 
                color: Color.Black, 
                mass: 1f, 
                velocity: new Vector2f(0, 0),
                angularVelocity: 0f,
                elasticity: 1f,
                friction: 0f,
                rotation: 0,
                solid: false,
                isStatic: true
            );
            Bodies[3] = new RectangleRigidBody(
                size: new Vector2f(20, 75),
                window: window, 
                color: Color.Black, 
                mass: 1f, 
                velocity: new Vector2f(0, 0),
                angularVelocity: 0f,
                elasticity: 1f,
                friction: 0f,
                rotation: 0,
                solid: false,
                isStatic: true
            );

            // Bodies[4] = new RectangleRigidBody(
            //     size: new Vector2f(4, 4),
            //     window: window,
            //     // Random color
            //     color: Color.Blue,
            //     mass: 1,
            //     // Random velocity between -20 and 20
            //     velocity: new Vector2f(-30, 0),
            //     angularVelocity: 0,
            //     elasticity: 1f,
            //     friction: 0f
            // );

            // Bodies[5] = new RectangleRigidBody(
            //     size: new Vector2f(4, 4),
            //     window: window,
            //     // Random color
            //     color: Color.Red,
            //     mass: 1,
            //     // Random velocity between -20 and 20
            //     velocity: new Vector2f(0, 0),
            //     angularVelocity: 0,
            //     elasticity: 1f,
            //     friction: 0f
            // );
            // Bodies[6] = new RectangleRigidBody(
            //     size: new Vector2f(4, 4),
            //     window: window,
            //     // Random color
            //     color: Color.Yellow,
            //     mass: 1,
            //     // Random velocity between -20 and 20
            //     velocity: new Vector2f(0, 0),
            //     angularVelocity: 0,
            //     elasticity: 1f,
            //     friction: 0f
            // );

            

            collisionResolver = new DCD();
            collisionManager = new CollisionManager(Bodies, discrete: collisionResolver.Discrete);

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
                while (accumulatedTime >= FrameTime )
                {
                    window.DispatchEvents();
                    Update(ref Bodies, FrameTime * speedFactor);
                    accumulatedTime -= FrameTime;
                }

                // Clear the window

                window.Clear(Color.Black);
                Text framerateText = new Text($"FPS: {trueFPS:F2}", new Font("arial.ttf"), 20);
                framerateText.FillColor = Color.White;
                framerateText.Position = new Vector2f(10, 10);
                window.Draw(framerateText);

                // Draw the scene
                Draw(ref Bodies);
                //Draw the real framerate on the right top corner of the window


                window.Display();

                // Sleep if the frame time is too fast
                float elapsed = clock.Restart().AsSeconds();

                if(frames % ComputingConstants.FrameRate == 0){
                    Console.WriteLine("----------------------------------------------");
                    Console.WriteLine("Frames: " + frames);
                    Console.WriteLine("Sim FPS: " + 1.0f / elapsed);
                    Console.WriteLine("Elapsed: " + elapsed * 1000 + "ms");
                    Console.WriteLine("Spare Time: " + (FrameTime - elapsed) * 1000 + "ms");

                }
                while (elapsed < FrameTime)
                {
                    elapsed = clock.ElapsedTime.AsSeconds();
                }

                // if (elapsed < FrameTime)
                //     System.Threading.Thread.Sleep((int)((FrameTime - elapsed) * 1000));

                // Stop the stopwatch
                // stopwatch.Stop();

                if(frames % ComputingConstants.FrameRate == 0){
                    Console.WriteLine("Time elapsed after sleep: " + elapsed + "ms");
                    Console.WriteLine("True FPS: " + 1.0f / (elapsed));
                    Console.WriteLine("Number of bodies: " + Bodies.Length);
                    Console.WriteLine("----------------------------------------------");
                    trueFPS = 1.0f / (elapsed);
                }
            }

        }

        /**
         * Event handler for when the window is closed
         * @param sender The object that sent the event
         * @param e The event arguments
         */
        private static void OnWindowClosed(object? sender, EventArgs e)
        {
            Console.WriteLine("Collisions: " + collisions);
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
        private static void Start(ref RigidBody[] Bodies)
        {
            Bodies[0].Start(new Vector2f(50, 0f));
            Bodies[1].Start(new Vector2f(50f, 75f));
            Bodies[2].Start(new Vector2f(0f, 75/2));
            Bodies[3].Start(new Vector2f(100f, 75/2));
            // Bodies[4].Start(new Vector2f(50f, 63f));
            // Bodies[5].Start(new Vector2f(30f, 63f));
            // Bodies[6].Start(new Vector2f(34f, 63f));
        }

        /**
         * Update all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private static void Update(ref RigidBody[] Bodies, float deltaTime)
        {
            frames++;

            //Every .5 seconds add a new body
            if (frames % 1 == 0 && Bodies.Length < 5000)
            {
                // Console.WriteLine("Adding new body");
                RigidBody newBody = new CircleRigidBody(
                    radius: .1f,
                    window: window,
                    // Random color
                    color: new Color((byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255)),
                    mass: 2,
                    // Random velocity between -20 and 20
                    velocity: new Vector2f(50, 50),
                    // velocity: new Vector2f((float)new Random().Next(10, 30), (float)new Random().Next(10, 30)),
                    angularVelocity: 0,
                    elasticity: 1f,
                    friction: 0.1f
                );
                // Create a new array with a size larger by one
                RigidBody[] newBodies = new RigidBody[Bodies.Length + 1];
                // Copy the elements from the original array to the new array
                Array.Copy(Bodies, newBodies, Bodies.Length);
                // Add the new element to the end of the new array
                newBodies[Bodies.Length] = newBody;
                // Replace the original array with the new array
                Bodies = newBodies;
                // Start the new body
                Bodies[Bodies.Length - 1].Start(new Vector2f(20f, 20f));
            }
            if (frames % 1 == 0 && Bodies.Length < 5000)
            {
                // Console.WriteLine("Adding new body");
                RigidBody newBody = new CircleRigidBody(
                    radius: .1f,
                    window: window,
                    // Random color
                    color: new Color((byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255)),
                    mass: 2,
                    // Random velocity between -20 and 20
                    velocity: new Vector2f(-50, 50),
                    // velocity: new Vector2f((float)new Random().Next(10, 30), (float)new Random().Next(10, 30)),
                    angularVelocity: 0,
                    elasticity: 1f,
                    friction: 0.1f
                );
                // Create a new array with a size larger by one
                RigidBody[] newBodies = new RigidBody[Bodies.Length + 1];
                // Copy the elements from the original array to the new array
                Array.Copy(Bodies, newBodies, Bodies.Length);
                // Add the new element to the end of the new array
                newBodies[Bodies.Length] = newBody;
                // Replace the original array with the new array
                Bodies = newBodies;
                // Start the new body
                Bodies[Bodies.Length - 1].Start(new Vector2f(80f, 20f));
            }

            // else if(frames % (15*10) == 0 && Bodies.Length == 500){
            //     Console.WriteLine("Adding new body");
            //     RigidBody newBody = new CircleRigidBody(
            //         radius: 10f,
            //         window: window,
            //         // Random color
            //         color: new Color((byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255)),
            //         mass: 10000,
            //         // Random velocity between -20 and 20
            //         velocity: new Vector2f(30, 30),
            //         // velocity: new Vector2f((float)new Random().Next(10, 30), (float)new Random().Next(10, 30)),
            //         angularVelocity: 0,
            //         elasticity: .9f,
            //         friction: 0.1f
            //     );
            //     // Create a new array with a size larger by one
            //     RigidBody[] newBodies = new RigidBody[Bodies.Length + 1];
            //     // Copy the elements from the original array to the new array
            //     Array.Copy(Bodies, newBodies, Bodies.Length);
            //     // Add the new element to the end of the new array
            //     newBodies[Bodies.Length] = newBody;
            //     // Replace the original array with the new array
            //     Bodies = newBodies;
            //     // Start the new body
            //     Bodies[Bodies.Length - 1].Start(new Vector2f(20f, 20f));
            // }



            foreach (RigidBody b in Bodies)
                b.Update(deltaTime);

            collisionManager.UpdateBVH(Bodies.ToArray());

            var potentialCollisions = collisionManager.GetPotentialCollisions();

            foreach (var (bodyA, bodyB) in potentialCollisions){

                if(bodyA.IsStatic && bodyB.IsStatic)
                    continue;

                Vector2f normal;
                float depth;
                if (bodyA.Collider.Intersects(bodyB.Collider, out normal, out depth))
                {
                    if(bodyA.IsStatic && bodyB.IsStatic)
                        continue;

                    Vector2f aToB = bodyB.Position - bodyA.Position;
                    Vector2f bToA = bodyA.Position - bodyB.Position;

                    float checkA = aToB.Dot(bodyA.Velocity);
                    float checkB = bToA.Dot(bodyB.Velocity);

                    if(bodyA.IsStatic)
                    {
                        if(bToA.Dot(normal) < 0)
                            bodyB.UpdatePosition(bodyB.Position + normal * depth );//* 1.1f);
                        else
                            bodyB.UpdatePosition(bodyB.Position - normal * depth );//* 1.1f);

                    }
                    else if(bodyB.IsStatic)
                    {
                        if(aToB.Dot(normal) < 0)
                            bodyA.UpdatePosition(bodyA.Position + normal * depth );//* 1.1f);
                        else
                            bodyA.UpdatePosition(bodyA.Position - normal * depth );//* 1.1f);

                    }
                    else
                    {
                        collisions++;
                        // Console.WriteLine("Collisions: " + collisions);
                        if(bToA.Dot(normal) < 0)
                        {
                            bodyB.UpdatePosition(bodyB.Position + normal * depth * 1.1f / 2);
                            bodyA.UpdatePosition(bodyA.Position - normal * depth * 1.1f / 2);
                        }
                        else
                        {
                            bodyA.UpdatePosition(bodyA.Position + normal * depth * 1.1f / 2);
                            bodyB.UpdatePosition(bodyB.Position - normal * depth * 1.1f / 2);
                        }
                    }
                    
                    if(checkA < 0 && checkB < 0)
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
            foreach (RigidBody b in Bodies){
                b.Draw();
            }


            // collisionManager.root.Draw();
        }
    }
}
