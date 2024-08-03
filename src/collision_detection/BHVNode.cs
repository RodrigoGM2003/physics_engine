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
        public RigidBody? body { get;} // The body in the node (if any)
        public BVHNode? Left { get;} // The left child of the node
        public BVHNode? Right { get;} // The right child of the node
        public FloatRect BoundingBox { get;} // The bounding box of the node

        public BVHNode(RigidBody body)
        {
            this.body = body;
            BoundingBox = body.RBCollider.SweptAABB;
        }

        public BVHNode(BVHNode left, BVHNode right)
        {
            body = null;
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
