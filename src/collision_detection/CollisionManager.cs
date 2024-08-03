using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    public class CollisionManager
    {
        private BVHNode root; // The root of the bounding volume hierarchy

        /**
         * Constructor for the CollisionManager class
         * @param bodies The bodies in the scene
         */
        public CollisionManager(RigidBody[] bodies)
        {
            root = BuildBVH(bodies, 0);
        }

        /**
         * Build the bounding volume hierarchy
         * @param bodies The bodies in the scene
         * @param depth The depth of the node in the hierarchy
         */
        private BVHNode BuildBVH(RigidBody[] bodies, int depth)
        {
            // If there is only one body, return a node with that body
            if (bodies.Length == 1)
                return new BVHNode(bodies[0]);

            // Sort the bodies based on the axis
            int axis = depth % 2;
            if (axis == 0)
                bodies = bodies.OrderBy(b => b.Position.X).ToArray();
            else
                bodies = bodies.OrderBy(b => b.Position.Y).ToArray();

            // Split the bodies into two halves
            int medianIndex = bodies.Length / 2;
            var leftBodies = bodies.Take(medianIndex).ToArray();
            var rightBodies = bodies.Skip(medianIndex).ToArray();

            // Build the left and right nodes
            var leftNode = BuildBVH(leftBodies, depth + 1);
            var rightNode = BuildBVH(rightBodies, depth + 1);

            return new BVHNode(leftNode, rightNode);
        }

        /**
         * Update the bounding volume hierarchy
         * @param bodies The bodies in the scene
         */
        public void UpdateBVH(RigidBody[] bodies)
        {
            root = BuildBVH(bodies, 0);
        }

        /**
         * Get the potential collisions in the scene
         */
        public List<(RigidBody, RigidBody)> GetPotentialCollisions()
        {
            var potentialCollisions = new List<(RigidBody, RigidBody)>();
            TraverseBVH(root, potentialCollisions);
            return potentialCollisions;
        }


        /**
         * Traverse the bounding volume hierarchy to find potential collisions
         * @param node The current node in the hierarchy
         * @param potentialCollisions The list of potential collisions
         *
         * Here is where all the magic happens. We traverse the BVH and check for potential collisions between the bodies.
         */
        private void TraverseBVH(BVHNode node, List<(RigidBody, RigidBody)> potentialCollisions)
        {
            // If the node is a leaf, return
            if (node.IsLeaf)
                return;

            // If the bounding boxes of the left and right nodes intersect, check for potential collisions
            if (node.Left.BoundingBox.Intersects(node.Right.BoundingBox))
            {
                // If both nodes are leaves, add them to the potential collisions list
                if (node.Left.IsLeaf && node.Right.IsLeaf)
                    potentialCollisions.Add((node.Left.rigidBody, node.Right.rigidBody));
                
                // Otherwise, traverse the left and right nodes
                else
                {
                    TraverseBVH(node.Left, potentialCollisions);
                    TraverseBVH(node.Right, potentialCollisions);
                }
            }
        }
    }
}
