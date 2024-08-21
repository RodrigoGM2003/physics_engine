using SFML.Window;
using SFML.Graphics;

namespace PhysicsEngine
{
    public static class RenderWindowManager
    {
        public static RenderWindow window { get; set; } = null!;
                /**
         * Event handler for when the window is closed
         * @param sender The object that sent the event
         * @param e The event arguments
         */
        public static void OnWindowClosed(object? sender, EventArgs e)
        {
            window.Close();
        }

        /**
         * Event handler for when the window is resized
         * @param sender The object that sent the event
         * @param e The event arguments
         */
        public static void OnWindowResized(object? sender, SizeEventArgs e)
        {
            // Maintain aspect ratio
            float windowAspect = e.Width / (float)e.Height;
            float viewWidth = window.Size.X;
            float viewHeight = window.Size.Y;
            View view = window.GetView();


            // Window is too wide
            if (windowAspect > ComputingConstants.AspectRatio)
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
    }
}