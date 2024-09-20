using System.Formats.Asn1;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Globalization;
using System.Diagnostics;


namespace PhysicsEngine
{
    public class Simulator
    {
        public static readonly int MaxSubsteps = 128; // The maximum number of substeps to take per frame
        public static readonly int MinSubsteps = 1; // The minimum number of substeps to take per frame

        // public Body[] Bodies { get; set; } = new Body[1000]; // The array of bodies in the scene
        public Body[] Bodies { get; set; } = new Body[4000]; // The array of bodies in the scene
        public Vector2f[] StartPositions { get; set; } = new Vector2f[4000]; // The start conditions of the bodies in the scene
        public int usedBodies { get; set; } = 0; // The number of bodies in the scene
        // public /*required*/ LogicObject[] Objects { get; set; } = new LogicObject[1000]; // The array of objects in the scene

        public RenderWindow window { get; set; } // The window to draw to

        public CollisionManager collisionManager { get; set; } // The collision manager for the scene
        public CollisionResolver collisionResolver { get; set; } // The collision resolver for the scene

        public float speedFactor { get; set; } = 1f; // The speed factor of the simulation
        public int substeps { get; set; } = 8; // The number of substeps to take per frame
        private double accumulatedTime { get; set; } = 0; // The time that has accumulated since the last frame

        private Stopwatch stopwatch = new Stopwatch();
        private double accumulatedTimeUpdate = 0;
        private double accumulatedTimeUpdateBVH = 0;
        private double accumulatedTimeGetPotentialCollisions = 0;
        private double accumulatedTimeCollisionLoop = 0;

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
 

        /**
        * Run the simulation
        * @param Bodies The array of bodies in the scene
        */
        public void Run()
        {

            Console.WriteLine("Running simulation");
            Console.WriteLine("Number of bodies: " + usedBodies);
            Console.WriteLine("Substeps: " + substeps);
            Start();

            // Create the clock
            var clock = new Clock();
            float frameTime = 1 / ComputingConstants.FrameRate;
            float substepTime = 1 / (substeps * ComputingConstants.FrameRate);
            int frames = 0;

            Stopwatch sw = new Stopwatch();


            // Main loop
            while (window.IsOpen)
            {
                frames++;
                // Update the time
                float deltaTime = clock.Restart().AsSeconds();
                accumulatedTime += deltaTime;

                if(frames % 5 == 0 && usedBodies < 200)
                {
                    var body =  new CircleRigidBody(
                        // float random radius between 1 and 4
                        radius: 1f,
                        window: window,
                        // Random color
                        color: new Color((byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255)),
                        mass: 1,
                        // Random velocity between -20 and 20
                        velocity: new Vector2f((float).1, (float)-60.0),
                        // velocity: new Vector2f((float)new Random().Next(10, 30), (float)new Random().Next(10, 30)),
                        angularVelocity: 0,
                        elasticity: .3f,
                        friction: 0.1f
                    );
                    AddBody(body, new Vector2f(50, 30));
                }
                // if(frames % 2 == 0 && usedBodies < 400)
                // {
                //     var body = new VerletBody(
                //         collider: new CircleCollider(
                //             radius: 1.0f,
                //             position: new Vector2f(0, 0)
                //         ),
                //         drawer: new CircleDrawer(
                //             _window: window,
                //             radius: 1.0f * PhysicsConstants.PixelsPerMeter,
                //             color: Color.Red
                //         ),
                //         isStatic: false,
                //         velocity: new Vector2f(40, 30)
                //     );
                //     AddBody(body, new Vector2f(50, 30));
                //     body.ApplyAcceleration(new Vector2f(30, 0));
                // }

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

                if(frames % ComputingConstants.FrameRate/2 == 0)
                    Console.WriteLine("True FPS: " + Math.Round(1.0f / elapsed), 0);
                    // Console.WriteLine("True FPS: " + 1.0f / elapsed);
            }

            accumulatedTimeUpdate /= frames;
            accumulatedTimeUpdateBVH /= frames;
            accumulatedTimeGetPotentialCollisions /= frames;
            accumulatedTimeCollisionLoop /= frames;

            Console.WriteLine("UpdateTime: " + accumulatedTimeUpdate);
            Console.WriteLine("UpdateBVHTime: " + accumulatedTimeUpdateBVH);
            Console.WriteLine("GetPotentialCollisionsTime: " + accumulatedTimeGetPotentialCollisions);
            Console.WriteLine("CollisionLoopTime: " + accumulatedTimeCollisionLoop);
        }

        /**
        * Start all the bodies in the scene
        * @param Bodies The array of bodies in the scene
        */
        private void Start()
        {
            for(int i = 0; i < usedBodies; i++)
                Bodies[i].Start(StartPositions[i]);
        }

        /**
        * Update all the bodies in the scene
        * @param Bodies The array of bodies in the scene
        */
        private void Update(float deltaTime)
        {
            stopwatch.Restart();
            stopwatch.Start();

            for(int i = 0; i < usedBodies; i++)
                // Task.Run(() => Bodies[i].Update(deltaTime));
                Bodies[i].Update(deltaTime);

            // Task.WaitAll();

            stopwatch.Stop();
            accumulatedTimeUpdate += stopwatch.Elapsed.TotalMilliseconds;
                

            stopwatch.Restart();
            stopwatch.Start();
            collisionManager.UpdateBVH(Bodies, usedBodiesCount: usedBodies);
            stopwatch.Stop();
            accumulatedTimeUpdateBVH += stopwatch.Elapsed.TotalMilliseconds;

            stopwatch.Restart();
            stopwatch.Start();
            var potentialCollisions = collisionManager.GetPotentialCollisions();
            stopwatch.Stop();
            accumulatedTimeGetPotentialCollisions += stopwatch.Elapsed.TotalMilliseconds;


            stopwatch.Restart();
            stopwatch.Start();
            foreach (var (bodyA, bodyB) in potentialCollisions){

                if(bodyA.IsStatic && bodyB.IsStatic)
                    continue;

                Vector2f normal;
                float depth;
                // if(!bodyA.IsStatic && !bodyB.IsStatic)
                //     continue;
                // else
                if (bodyA.Collider.Intersects(bodyB.Collider, out normal, out depth))
                {
                    if(collisionResolver.HandleOverlap(bodyA, bodyB, normal, depth))
                        continue;
                    
                    collisionResolver.ResolveCollision(bodyA, bodyB, normal, depth);
                }
                
            }
            stopwatch.Stop();
            accumulatedTimeCollisionLoop += stopwatch.Elapsed.TotalMilliseconds;
        }

        /**
        * Add a body to the scene
        * @param body The body to add to the scene
        * @param Bodies The array of bodies in the scene
        */
        public void AddBody(Body body, Vector2f position)
        {
            if(usedBodies >= Bodies.Length)
                IncrementCapacity();
            

            Bodies[usedBodies] = body;
            StartPositions[usedBodies] = position;
            Bodies[usedBodies].Start(StartPositions[usedBodies]);

            usedBodies++;
        }
        /**
        * Increment the capacity of the bodies array
        * @param Bodies The array of bodies in the scene
        */
        private void IncrementCapacity(){
            Body[] newBodies = new Body[Bodies.Length * 2];
            Bodies.CopyTo(newBodies, 0);
            Bodies = newBodies;
        }

        /**
        * Draw all the bodies in the scene
        * @param Bodies The array of bodies in the scene
        */
        private void Draw()
        {
            for(int i = 0; i < usedBodies; i++)
                Bodies[i].Draw();
            


            // collisionManager.root.Draw();
        }

        //------------------------------------Main Constructor------------------------------------//
        /**
        * Read a scene from a file
        * @param sceneName The name of the scene file
        */
        public Simulator(in string sceneName)
        {
            // Assuming your file uses the dot as the decimal separator:
            var cultureInfo = CultureInfo.InvariantCulture;
            // Read the file
            try
            {
                using (StreamReader sr = new StreamReader(sceneName))
                {
                    string? line;

                    line = sr.ReadLine();
                    if(line != "SCN")
                    {
                        Console.WriteLine("Invalid scene file: Expected 'SCN' format");
                        return;
                    }

                    float aspectRatio = ComputingConstants.AspectRatio;
                    int windowHeight = 600;
                    float pixelsPerMeter = PhysicsConstants.PixelsPerMeter;
                    Vector2f gravityVector = PhysicsConstants.GravityVector;
                    string collisionManagerType = "classic";
                    bool discrete = true;
                    collisionResolver = new DCD();
                    speedFactor = PhysicsConstants.DefaultSpeedFactor;
                    substeps = ComputingConstants.DefaultSubsteps;

                    
                    if((line = sr.ReadLine()) != "#Settings")
                    {
                        Console.WriteLine("Invalid scene file: Expected #Settings");
                        return;
                    }

                    string value = "";
                    while((line = sr.ReadLine()) != "#EndSettings")
                    {
                        //------------------------------------AspectRatio------------------------------------//
                        if(line.StartsWith("AspectRatio:")){
                            //Read a division of two floats
                            value = line.Substring("AspectRatio:".Length).Trim();
                            string[] parts = value.Split('/');
                            if(parts.Length == 2 && float.TryParse(parts[0], ComputingConstants.CultureInfo, out float num) && 
                                                    float.TryParse(parts[1], ComputingConstants.CultureInfo, out float den))
                                aspectRatio = num / den;
                            else
                                Console.WriteLine("Invalid aspect ratio value");
                        }
                        //------------------------------------WindowHeight------------------------------------//
                        else if (line.StartsWith("WindowHeight:"))
                            windowHeight = Utils.ReadInt(in line, "WindowHeight:");

                        //------------------------------------PixelsPerMeter------------------------------------//
                        else if (line.StartsWith("PixelsPerMeter:"))
                            pixelsPerMeter = Utils.ReadFloat(in line, "PixelsPerMeter:");

                        //------------------------------------Gravity------------------------------------//
                        else if (line.StartsWith("Gravity:"))
                            gravityVector = Utils.ReadVector2f(in line, "Gravity:");

                        //------------------------------------CollisionManager------------------------------------//
                        else if(line.StartsWith("CollisionManager:"))
                            collisionManagerType = line.Substring("CollisionManager:".Length).Trim();

                        //------------------------------------Discrete------------------------------------//
                        else if(line.StartsWith("Discrete:")){
                            value = line.Substring("Discrete:".Length).Trim();
                            discrete = bool.Parse(value);
                        }

                        //------------------------------------CollisionResolver------------------------------------//
                        else if(line.StartsWith("CollisionResolver:"))
                        {
                            // Extract the value after "CollisionResolver:"
                            value = line.Substring("CollisionResolver:".Length).Trim();
                            if(value == "dcd")
                                collisionResolver = new DCD();
                            else if(value == "ccd")
                                collisionResolver = new CCD();
                            else if(value == "verlet")
                                collisionResolver = new Verlet();

                            else
                                Console.WriteLine("Invalid collision resolver value");
                        }

                        //------------------------------------SpeedFactor------------------------------------//
                        else if(line.StartsWith("SpeedFactor:"))
                            speedFactor = Utils.ReadFloat(in line, "SpeedFactor:");

                        //------------------------------------Substeps------------------------------------//
                        else if(line.StartsWith("Substeps:"))
                            substeps = Utils.ReadInt(in line, "Substeps:");
                    }


                    //Create a window with the name of ths schene without the extension
                    this.window = new RenderWindow(new VideoMode((uint)(windowHeight * aspectRatio), (uint)windowHeight), 
                                                    sceneName.Substring(0, sceneName.Length - 4));
                    RenderWindowManager.window = window;
                    window.Closed += RenderWindowManager.OnWindowClosed;
                    window.Resized += RenderWindowManager.OnWindowResized;


                    if((line = sr.ReadLine()) != "#SceneDefinition")
                    {
                        Console.WriteLine("Invalid scene file: Expected #SceneDefinition");
                        return;
                    }


                    line = sr.ReadLine();
                    usedBodies = 0;
                    if(!line.StartsWith("BodyCount:"))
                    {
                        Console.WriteLine("Invalid scene file: Expected BodyCount");
                        return;
                    }

                    //Read the number of bodies in the scene
                    usedBodies = int.Parse(line.Substring("BodyCount:".Length).Trim(), cultureInfo);


                    for(int i = 0; i < usedBodies; i++)
                    {
                        line = sr.ReadLine();
                        line = line.TrimStart(' ');
                        if(!line.TrimStart().StartsWith("type:"))
                        {
                            Console.WriteLine("Invalid scene file: Expected Body in body " + i);
                            Bodies[i] = new CircleRigidBody(window: window);
                            continue;
                        }
                        //------------------------------------Type------------------------------------//
                        string bodyType = "";
                        if(line.StartsWith("type:"))
                            bodyType = line.Substring("type:".Length).Trim();

                        Vector2f size = new Vector2f(1, 1);
                        float radius = 1;
                        Vector2f[] vertices = new Vector2f[4];
                        vertices[0] = new Vector2f(-0.5f, -0.5f);
                        vertices[1] = new Vector2f(-0.5f, 0.5f);
                        vertices[2] = new Vector2f(0.5f, 0.5f);
                        vertices[3] = new Vector2f(0.5f, -0.5f);

                        Color color = Color.White;
                        float mass = 1;

                        Vector2f velocity = new Vector2f(0, 0);
                        Vector2f acceleration = new Vector2f(0, 0);
                    
                        float angularVelocity = 0;
                        float angularAcceleration = 0;

                        float elasticity = PhysicsConstants.DefaultElasticity;
                        float friction = PhysicsConstants.DefaultFriction;
                        bool isStatic = false;
                        bool solid = true;

                        StartPositions[i] = new Vector2f(0, 0);
                        float rotation = 0;                    

                        while((line = sr.ReadLine()) != "End")
                        {
                            line = line.TrimStart(' ');
                            //------------------------------------Size------------------------------------//
                            if(line.StartsWith("size:"))
                                size = Utils.ReadVector2f(in line, "size:");

                            //------------------------------------Radius------------------------------------//
                            else if(line.StartsWith("radius:"))
                                radius = Utils.ReadFloat(in line, "radius:");
                            

                            //------------------------------------Vertices------------------------------------//
                            else if(line.StartsWith("vertices:"))
                            {
                                // Extract the int value after "vertices:"
                                value = line.Substring("vertices:".Length).Trim();

                                //Format for vertices is: vertices: num, x1, y1, x2, y2, ...
                                string[] parts = value.Split(',');
                                if(parts.Length < 3 || !int.TryParse(parts[0], out int numVertices))
                                {
                                    Console.WriteLine("Invalid vertices value in body " + i);
                                    continue;
                                }

                                if(parts.Length != 2 * numVertices + 1)
                                {
                                    Console.WriteLine("Invalid number of vertices in " + i);
                                    continue;
                                }

                                var auxVertices = new Vector2f[numVertices];
                                bool valid = true;
                                for(int j = 0; j < numVertices; j++)
                                {
                                    if(!float.TryParse(parts[2 * j + 1], ComputingConstants.CultureInfo, out float x) ||
                                        !float.TryParse(parts[2 * j + 2], ComputingConstants.CultureInfo, out float y))
                                    {
                                        Console.WriteLine("Invalid vertex in body " + i);
                                        valid = false;
                                        break; // Exit loop on error
                                    }
                                    auxVertices[j] = new Vector2f(x, y);
                                }

                                if (!valid)
                                {
                                    // Handle the error, such as resetting `vertices` or skipping the current body
                                    vertices = null;
                                    continue;
                                }
                                auxVertices.CopyTo(vertices, 0);

                            }
                            //------------------------------------Color------------------------------------//
                            else if(line.StartsWith("color:"))
                                Console.WriteLine("Color not implemented");

                            //------------------------------------Mass------------------------------------//
                            else if(line.StartsWith("mass:"))
                                mass = Utils.ReadFloat(in line, "mass:");
                            
                            //------------------------------------Velocity------------------------------------//
                            else if(line.StartsWith("velocity:"))
                                velocity = Utils.ReadVector2f(in line, "velocity:");
                            
                            //------------------------------------Acceleration------------------------------------//
                            else if(line.StartsWith("acceleration:"))
                                acceleration = Utils.ReadVector2f(in line, "acceleration:");
                            
                            //------------------------------------AngularVelocity------------------------------------//
                            else if(line.StartsWith("angularVelocity:"))
                                angularVelocity = Utils.ReadFloat(in line, "angularVelocity:");
                            
                            //------------------------------------AngularAcceleration------------------------------------//
                            else if(line.StartsWith("angularAcceleration:"))
                                angularAcceleration = Utils.ReadFloat(in line, "angularAcceleration:");
                            
                            //------------------------------------Elasticity------------------------------------//
                            else if(line.StartsWith("elasticity:"))
                                elasticity = Utils.ReadFloat(in line, "elasticity:");

                            //------------------------------------Friction------------------------------------//
                            else if(line.StartsWith("friction:"))
                                friction = Utils.ReadFloat(in line, "friction:");

                            //------------------------------------IsStatic------------------------------------//
                            else if(line.StartsWith("isStatic:"))
                            {
                                // Extract the bool value after "isStatic:"
                                value = line.Substring("isStatic:".Length).Trim();
                                isStatic = bool.Parse(value);
                            }
                            else if(line.StartsWith("solid:"))
                            {
                                // Extract the bool value after "isStatic:"
                                value = line.Substring("solid:".Length).Trim();
                                solid = bool.Parse(value);
                            }
                            //------------------------------------Position------------------------------------//
                            else if(line.StartsWith("position:"))
                                StartPositions[i] = Utils.ReadVector2f(in line, "position:");
                            
                            //------------------------------------Rotation------------------------------------//
                            else if(line.StartsWith("rotation:"))
                                rotation = Utils.ReadFloat(in line, "rotation:");
                            

                        }
                
                        if(bodyType == "circle")
                        {
                            Bodies[i] = new CircleRigidBody(
                                radius: radius,
                                window: window,
                                color: color,
                                mass: mass,
                                velocity: velocity,
                                rotation: rotation,
                                acceleration: acceleration,
                                angularVelocity: angularVelocity,
                                angularAcceleration: angularAcceleration,
                                elasticity: elasticity,
                                friction: friction,
                                isStatic: isStatic,                                
                                solid: solid
                            );
                        }
                        else if(bodyType == "rectangle")
                        {
                            Bodies[i] = new RectangleRigidBody(
                                size: size,
                                window: window,
                                color: color,
                                mass: mass,
                                velocity: velocity,
                                rotation: rotation,
                                acceleration: acceleration,
                                angularVelocity: angularVelocity,
                                angularAcceleration: angularAcceleration,
                                elasticity: elasticity,
                                friction: friction,
                                isStatic: isStatic,
                                solid: solid
                            );
                        }
                        else if(bodyType == "polygon")
                        {
                            Bodies[i] = new PolygonRigidBody(
                                vertices: vertices,
                                window: window,
                                color: color,
                                mass: mass,
                                rotation: rotation,
                                velocity: velocity,
                                acceleration: acceleration,
                                angularVelocity: angularVelocity,
                                angularAcceleration: angularAcceleration,
                                elasticity: elasticity,
                                friction: friction,
                                isStatic: isStatic,
                                solid: solid
                            );
                        }
                        else
                        {
                            Console.WriteLine("Invalid body type in body " + i + " " + bodyType);
                            Bodies[i] = new CircleRigidBody(window: window);
                        }                    
                    }

                    //Show all fields in console
                    /*Console.WriteLine("Aspect Ratio: " + aspectRatio);
                    Console.WriteLine("Window Height: " + windowHeight);
                    Console.WriteLine("Pixels Per Meter: " + pixelsPerMeter);
                    Console.WriteLine("Gravity: " + gravityVector);
                    Console.WriteLine("Collision Manager: " + collisionManagerType);
                    Console.WriteLine("Collision Resolver: " + collisionResolver);
                    Console.WriteLine("Speed Factor: " + speedFactor);
                    Console.WriteLine("Substeps: " + substeps);

                    Console.WriteLine();
                    Console.WriteLine("Bodies: " + usedBodies);
                    for(int i = 0; i < usedBodies; i++)
                    {
                        Console.WriteLine("Body " + i + ": ");
                        Console.WriteLine("Type: " + Bodies[i].GetType());

                        if(Bodies[i] is CircleRigidBody circle)
                            Console.WriteLine("Radius: " + circle.Radius);

                        else if(Bodies[i] is RectangleRigidBody rectangle)
                            Console.WriteLine("Size: " + rectangle.Size);

                        else if(Bodies[i] is PolygonRigidBody polygon)
                        {
                            Console.WriteLine("Vertices: ");
                            foreach(var vertex in polygon.Vertices)
                                Console.WriteLine(vertex);
                        }
                        Console.WriteLine("Mass: " + Bodies[i].Mass);
                        Console.WriteLine("Velocity: " + Bodies[i].Velocity);
                        Console.WriteLine("Acceleration: " + Bodies[i].Acceleration);
                        Console.WriteLine("Angular Velocity: " + Bodies[i].AngularVelocity);
                        Console.WriteLine("Angular Acceleration: " + Bodies[i].AngularAcceleration);
                        Console.WriteLine("Is Static: " + Bodies[i].IsStatic);
                    }*/

                    //Create a collision manager
                    collisionManager = new CollisionManager(Bodies, usedBodies, discrete);


                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return;
            }
        }
    }
}
