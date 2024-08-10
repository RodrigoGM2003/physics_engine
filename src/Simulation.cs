﻿using System;
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
        private static float AspectRatio = ComputingConstants.AspectRatio;  // Desired aspect ratio (e.g., 4:3)
        private static readonly float FrameTime = 1.0f / ComputingConstants.FrameRate;   // Frame time for 60 FPS
        private static RenderWindow window = null!; // The window to draw to
        private static Clock clock = null!; // The clock to keep track of time
        private static float accumulatedTime = 0.0f; // The time that has accumulated since the last frame
        private static CollisionManager collisionManager;
        private static CollisionResolver collisionResolver;
        private static float speedFactor = 2f; // The speed factor of the simulation


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
            RigidBody[] Bodies = new RigidBody[2];

            // Bodies[0] = new CircleRigidBody(
            //     radius: 1f, 
            //     window: window, 
            //     color: Color.White, 
            //     mass: 1f, 
            //     velocity: new Vector2f(0, 0),
            //     angularVelocity: 0f,
            //     elasticity: 1f,
            //     friction: .9f
            // );
            Bodies[0] = new RectangleRigidBody(
                size: new Vector2f(10, 2),
                window: window, 
                color: Color.White, 
                mass: 1f, 
                velocity: new Vector2f(0, 0),
                angularVelocity: 0f,
                elasticity: 1f,
                friction: 0f,
                rotation: 2
            );
            Bodies[1] = new RectangleRigidBody(
                size: new Vector2f(30, 30),
                window: window, 
                color: Color.Green, 
                mass: 1f, 
                velocity: new Vector2f(0, 0),
                angularVelocity: 0f,
                elasticity: 1f,
                friction: 0.9f,
                isStatic: true
            );

            // Bodies[1] = new CircleRigidBody(
            //     radius: 15f, 
            //     window: window, 
            //     color: Color.Green, 
            //     mass: 100f, 
            //     velocity: new Vector2f(0, 0),
            //     angularVelocity: 0f,
            //     elasticity: 1f,
            //     friction: 0.9f,
            //     isStatic: true
            // );

            // Bodies[1] = new CircleRigidBody(
            //     radius: 1f, window: 
            //     window, 
            //     color: Color.Green, 
            //     mass: 1f, 
            //     velocity: new Vector2f(-10, -20),
            //     angularVelocity: 0f,
            //     elasticity: .9f,
            //     friction: 0.9f
            // );

            // Bodies[2] = new RectangleRigidBody(
            //     size: new Vector2f(15, 2),
            //     window: window, 
            //     color: Color.Magenta,                             
            //     mass: 1, 
            //     velocity: new Vector2f(10, -30),
            //     angularVelocity: 3f,
            //     elasticity: 1f,
            //     friction: 0.5f,
            //     rotation: 0
            // );

            // Bodies[3] = new RectangleRigidBody(
            //     size: new Vector2f(3, 3),
            //     window: window, 
            //     color: Color.Cyan, 
            //     mass: 1, 
            //     velocity: new Vector2f(0, 0),
            //     angularVelocity: 0f,
            //     elasticity: 1f,
            //     friction: 0.5f
            // );

            // Bodies[4] = new CircleRigidBody(
            //     radius: 4f,
            //     window: window, 
            //     color: Color.Yellow, 
            //     mass: 1, 
            //     velocity: new Vector2f(-20, -20),
            //     angularVelocity: 1,
            //     elasticity: 1f,
            //     friction: 0.1f
            // );


            // Bodies[5] = new RectangleRigidBody(
            //     size: new Vector2f(30, 30),
            //     velocity: new Vector2f(0, -20),
            //     angularVelocity: 2,
            //     window: window,
            //     color: Color.Red,
            //     elasticity: 1,
            //     friction: 0.5f,
            //     rotation: 0
            // );

            // Bodies[6] = new CircleRigidBody(
            //     radius: 4f,
            //     velocity: new Vector2f(-10, 0),
            //     window: window,
            //     color: Color.Yellow,
            //     elasticity: 1,
            //     friction: 0.5f,
            //     rotation: 0,
            //     solid: false
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
        private static void Start(ref RigidBody[] Bodies)
        {
            Bodies[0].Start(new Vector2f(70, 10f));
            Bodies[1].Start(new Vector2f(60f, 50f));
            // Bodies[1].Start(new Vector2f(50f, 60f));
            // Bodies[2].Start(new Vector2f(10f, 70f));
            // Bodies[3].Start(new Vector2f(30f, 10f));
            // Bodies[4].Start(new Vector2f(0f, 30f));
            // Bodies[5].Start(new Vector2f(50f, 50f));
            // // // Bodies[5].Start(new Vector2f(50f, 75f));
            // Bodies[6].Start(new Vector2f(50f, 10f));
        }

        //     Bodies[0].Start(new Vector2f(0, 60f));
        //     Bodies[1].Start(new Vector2f(30f, 60f));
        //     Bodies[2].Start(new Vector2f(10f, 70f));
        //     Bodies[3].Start(new Vector2f(30f, 10f));
        //     Bodies[4].Start(new Vector2f(0f, 30f));
        //     Bodies[5].Start(new Vector2f(50f, 50f));
        //     // // Bodies[5].Start(new Vector2f(50f, 75f));
        //     Bodies[6].Start(new Vector2f(50f, 10f));
        // }

        /**
         * Update all the bodies in the scene
         * @param Bodies The array of bodies in the scene
         */
        private static void Update(ref RigidBody[] Bodies, float deltaTime)
        {
            foreach (RigidBody b in Bodies)
                b.Update(deltaTime);

            collisionManager.UpdateBVH(Bodies.ToArray());

            var potentialCollisions = collisionManager.GetPotentialCollisions();

            foreach (var (bodyA, bodyB) in potentialCollisions){
                Vector2f normal;
                float depth;
                Console.WriteLine(bodyA.Collider.GetType() + " " + bodyB.Collider.GetType());
                if (bodyA.Collider.Intersects(bodyB.Collider, out normal, out depth)){
                    Console.WriteLine("Collision");
                    collisionResolver.ResolveCollision(bodyA, bodyB, normal, depth);
                }
                else
                    Console.WriteLine("No collision");
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

                if(b.Collider is PolygonCollider polygon){
                    var a = (PolygonCollider)b.Collider;

                    //Calculate true vertices should be private
                    var vertices = a.CalculateTrueVertices(polygon.Vertices, b.Position, b.Rotation);
                    //Calculate true vertices should be private

                    for(int i = 0; i < vertices.Length; i++){
                        vertices[i] *= PhysicsConstants.PixelsPerMeter;
                    }

                    foreach (var vertex in vertices){
                        CircleShape c = new CircleShape(4);
                        c.Origin = new Vector2f(2, 2);
                        c.Position = vertex;
                        c.FillColor = Color.Blue;
                        window.Draw(c);
                    }
                }
            }


            collisionManager.root.Draw();
        }
    }
}
