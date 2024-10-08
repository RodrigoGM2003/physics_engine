// using System;
// using SFML.Graphics;
// using SFML.Window;
// using SFML.System;
// using System.Data;

// namespace PhysicsEngine
// {
//     public class CollisionManagerOld
//     {
//         public BVHNode root {get; set;} // The root of the bounding volume hierarchy
//         public bool Discrete {get; set;} // Whether continuous collision detection is enabled

//         /**
//          * Constructor for the CollisionManagerOld class
//          * @param bodies The bodies in the scene
//          */
//         public CollisionManagerOld(RigidBody[] bodies, bool discrete)
//         {
//             Discrete = discrete;
//             root = BuildBVH(bodies, 0);
//         }

//         /**
//          * Build the bounding volume hierarchy
//          * @param bodies The bodies in the scene
//          * @param depth The depth of the node in the hierarchy
//          */
//         private BVHNode BuildBVH(RigidBody[] bodies, int depth)
//         {

//             // If there is only one body, return a node with that body
//             if (bodies.Length == 1)
//                 return new BVHNode(bodies[0], Discrete);

//             // Sort the bodies based on the axis
//             int axis = depth % 2;
//             if (axis == 0)
//                 // bodies = bodies.OrderBy(b => b.Position.X).ToArray();
//                 Array.Sort(bodies, (b1, b2) => b1.Position.X.CompareTo(b2.Position.X));
//             else
//                 // bodies = bodies.OrderBy(b => b.Position.Y).ToArray();
//                 Array.Sort(bodies, (b1, b2) => b1.Position.Y.CompareTo(b2.Position.Y));


//             // Split the bodies into two halves
//             int medianIndex = bodies.Length / 2;
//             var leftBodies = bodies.Take(medianIndex).ToArray();
//             var rightBodies = bodies.Skip(medianIndex).ToArray();

//             // Build the left and right nodes
//             var leftNode = BuildBVH(leftBodies, depth + 1);
//             var rightNode = BuildBVH(rightBodies, depth + 1);

//             return new BVHNode(leftNode, rightNode);
//         }

//         /**
//          * Update the bounding volume hierarchy
//          * @param bodies The bodies in the scene
//          */
//         public void UpdateBVH(RigidBody[] bodies)
//         {
//             root = BuildBVH(bodies, 0);
//         }

//         /**
//          * Get the potential collisions in the scene
//          */
//         public List<(RigidBody, RigidBody)> GetPotentialCollisions()
//         {
//             var potentialCollisions = new List<(RigidBody, RigidBody)>();
//             TraverseBVH(root, potentialCollisions);
//             return potentialCollisions;
//         }


//         /**
//          * Traverse the bounding volume hierarchy to find potential collisions
//          * @param node The current node in the hierarchy
//          * @param potentialCollisions The list of potential collisions
//          *
//          * Here is where all the magic happens. We traverse the BVH and check for potential collisions between the bodies.
//          */
//         private void TraverseBVH(BVHNode node, List<(RigidBody, RigidBody)> potentialCollisions)
//         {
//             // If the node is a leaf, return
//             if (node.IsLeaf)
//                 return;
            

//             // Traverse the left and right subtrees to find potential collisions within them
//             TraverseBVH(node.Left, potentialCollisions);
//             TraverseBVH(node.Right, potentialCollisions);

//             // Check for potential collisions between the left and right subtrees
//             CheckPotentialCollisions(node.Left, node.Right, potentialCollisions);
//         }

//         /**
//          * Check for potential collisions between two nodes
//          * @param leftNode The left node
//          * @param rightNode The right node
//          * @param potentialCollisions The list of potential collisions
//          */
//         private void CheckPotentialCollisions(BVHNode leftNode, BVHNode rightNode, List<(RigidBody, RigidBody)> potentialCollisions)
//         {
//             // If the bounding boxes of the nodes intersect, check for potential collisions
//             if (leftNode.BoundingBox.Intersects(rightNode.BoundingBox))
//             {
//                 // If both nodes are leaves, add them to the potential collisions list
//                 if (leftNode.IsLeaf && rightNode.IsLeaf)
//                     potentialCollisions.Add((leftNode.body, rightNode.body));
                
//                 // If one of the nodes is a leaf, check for potential collisions between the leaf and the subtrees of the other node
//                 else if (leftNode.IsLeaf)
//                 {
//                     CheckPotentialCollisions(leftNode, rightNode.Left, potentialCollisions);
//                     CheckPotentialCollisions(leftNode, rightNode.Right, potentialCollisions);
//                 }
//                 else if (rightNode.IsLeaf)
//                 {
//                     CheckPotentialCollisions(leftNode.Left, rightNode, potentialCollisions);
//                     CheckPotentialCollisions(leftNode.Right, rightNode, potentialCollisions);
//                 }
//                 // If neither node is a leaf, check for potential collisions between the subtrees of both nodes
//                 else
//                 {
//                     CheckPotentialCollisions(leftNode.Left, rightNode.Left, potentialCollisions);
//                     CheckPotentialCollisions(leftNode.Left, rightNode.Right, potentialCollisions);
//                     CheckPotentialCollisions(leftNode.Right, rightNode.Left, potentialCollisions);
//                     CheckPotentialCollisions(leftNode.Right, rightNode.Right, potentialCollisions);
//                 }
//             }
//         }
//     }
// }
