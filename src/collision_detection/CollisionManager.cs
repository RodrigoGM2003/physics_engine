using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    public class CollisionManager
    {
        public BVHNode root {get; set;} // The root of the bounding volume hierarchy
        public bool Discrete {get; set;} // Whether continuous collision detection is enabled

        /**
         * Constructor for the CollisionManager2 class
         * @param bodies The bodies in the scene
         */
        public CollisionManager(RigidBody[] bodies, int usedBodiesCount, bool discrete)
        {
            Discrete = discrete;
            root = BuildBVH(bodies, 0, usedBodiesCount, 0);
        }

        /**
         * Build the bounding volume hierarchy
         * @param bodies The bodies in the scene
         * @param depth The depth of the node in the hierarchy
         */
        private BVHNode BuildBVH(RigidBody[] bodies, int startIndex, int endIndex, int depth)
        {
            int usedBodiesCount = endIndex - startIndex;

            // If there is only one body, return a node with that body
            if (usedBodiesCount == 1)
                return new BVHNode(bodies[startIndex], Discrete);

            // Sort the bodies based on the axis
            int axis = depth % 2;
            if (axis == 0)
                Array.Sort(bodies, startIndex, usedBodiesCount, Comparer<RigidBody>.Create((b1, b2) => b1.Position.X.CompareTo(b2.Position.X)));
            else
                Array.Sort(bodies, startIndex, usedBodiesCount, Comparer<RigidBody>.Create((b1, b2) => b1.Position.Y.CompareTo(b2.Position.Y)));

            // Split the bodies into two halves
            int medianIndex = startIndex + usedBodiesCount / 2;

            // Build the left and right nodes using indices to avoid creating new arrays
            var leftNode = BuildBVH(bodies, startIndex, medianIndex, depth + 1);
            var rightNode = BuildBVH(bodies, medianIndex, endIndex, depth + 1);

            return new BVHNode(leftNode, rightNode);
        }



        /**
         * Update the bounding volume hierarchy
         * @param bodies The bodies in the scene
         */
        public void UpdateBVH(RigidBody[] bodies, int usedBodiesCount)
        {
            root = BuildBVH(bodies, 0, usedBodiesCount, depth: 0);
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
            

            // Traverse the left and right subtrees to find potential collisions within them
            TraverseBVH(node.Left, potentialCollisions);
            TraverseBVH(node.Right, potentialCollisions);

            // Check for potential collisions between the left and right subtrees
            CheckPotentialCollisions(node.Left, node.Right, potentialCollisions);
        }

        /**
         * Check for potential collisions between two nodes
         * @param leftNode The left node
         * @param rightNode The right node
         * @param potentialCollisions The list of potential collisions
         */
        private void CheckPotentialCollisions(BVHNode leftNode, BVHNode rightNode, List<(RigidBody, RigidBody)> potentialCollisions)
        {
            // If the bounding boxes of the nodes intersect, check for potential collisions
            if (leftNode.BoundingBox.Intersects(rightNode.BoundingBox))
            {
                // If both nodes are leaves, add them to the potential collisions list
                if (leftNode.IsLeaf && rightNode.IsLeaf)
                    potentialCollisions.Add((leftNode.body, rightNode.body));
                
                // If one of the nodes is a leaf, check for potential collisions between the leaf and the subtrees of the other node
                else if (leftNode.IsLeaf)
                {
                    CheckPotentialCollisions(leftNode, rightNode.Left, potentialCollisions);
                    CheckPotentialCollisions(leftNode, rightNode.Right, potentialCollisions);
                }
                else if (rightNode.IsLeaf)
                {
                    CheckPotentialCollisions(leftNode.Left, rightNode, potentialCollisions);
                    CheckPotentialCollisions(leftNode.Right, rightNode, potentialCollisions);
                }
                // If neither node is a leaf, check for potential collisions between the subtrees of both nodes
                else
                {
                    CheckPotentialCollisions(leftNode.Left, rightNode.Left, potentialCollisions);
                    CheckPotentialCollisions(leftNode.Left, rightNode.Right, potentialCollisions);
                    CheckPotentialCollisions(leftNode.Right, rightNode.Left, potentialCollisions);
                    CheckPotentialCollisions(leftNode.Right, rightNode.Right, potentialCollisions);
                }
            }
        }
    }
}
