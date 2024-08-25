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
        public Body? body { get;} // The body in the node (if any)
        public BVHNode? Left { get;} // The left child of the node
        public BVHNode? Right { get;} // The right child of the node
        public FloatRect BoundingBox { get;} // The bounding box of the node

        public bool IsLeaf => body != null;

        public BVHNode(Body body, bool discrete)
        {
            this.body = body;

            BoundingBox = discrete ? body.Collider.BoundingBox : body.Collider.SweptAABB;
        }

        public BVHNode(BVHNode left, BVHNode right)
        {
            body = null;
            Left = left;
            Right = right;

            float minX = Math.Min(left.BoundingBox.Left, right.BoundingBox.Left);
            float minY = Math.Min(left.BoundingBox.Top, right.BoundingBox.Top);
            float maxX = Math.Max(left.BoundingBox.Left + left.BoundingBox.Width, right.BoundingBox.Left + right.BoundingBox.Width);
            float maxY = Math.Max(left.BoundingBox.Top + left.BoundingBox.Height, right.BoundingBox.Top + right.BoundingBox.Height);

            BoundingBox = new FloatRect(
                minX,
                minY,
                maxX - minX, // Width
                maxY - minY  // Height
            );
        }

        public void Draw()
        {
            RectangleShape shape = new RectangleShape(BoundingBox.Size * PhysicsConstants.PixelsPerMeter);
            shape.Position = new Vector2f(BoundingBox.Left * PhysicsConstants.PixelsPerMeter, BoundingBox.Top * PhysicsConstants.PixelsPerMeter);
            shape.FillColor = Color.Transparent;
            shape.OutlineColor = Color.Green;
            shape.OutlineThickness = 1;

            RenderWindowManager.window.Draw(shape);

            if (Left != null)
                Left.Draw();
            if (Right != null)
                Right.Draw();
        }
    }
}
