using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    /**
     * Class for the bounding volume hierarchy node
     */
    public class BVHNode
    {
        public RigidBody? rigidBody { get;} // The rigidBody in the node (if any)
        public BVHNode? Left { get;} // The left child of the node
        public BVHNode? Right { get;} // The right child of the node
        public FloatRect BoundingBox { get;} // The bounding box of the node

        public bool IsLeaf => rigidBody != null;

        public BVHNode(RigidBody rigidBody)
        {
            this.rigidBody = rigidBody;
            BoundingBox = rigidBody.RBCollider.SweptAABB;
        }

        public BVHNode(BVHNode left, BVHNode right)
        {
            rigidBody = null;
            Left = left;
            Right = right;
            BoundingBox = new FloatRect(
                Math.Min(left.BoundingBox.Left, right.BoundingBox.Left),
                Math.Min(left.BoundingBox.Top, right.BoundingBox.Top),
                Math.Max(left.BoundingBox.Left + left.BoundingBox.Width, right.BoundingBox.Left + right.BoundingBox.Width),
                Math.Max(left.BoundingBox.Top + left.BoundingBox.Height, right.BoundingBox.Top + right.BoundingBox.Height)
            );
        }
    }
}
