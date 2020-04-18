using Microsoft.Xna.Framework;

namespace SpaceMobaClient.GamePlay
{
    /// <summary>
    /// A class that represents the camera that is used to render scenes.
    /// </summary>
    public class Camera
    {
        private Rectangle _Bounds;

        /// <summary>
        /// Gets top left corner position of camera.
        /// </summary>
        public Point Position
        {
            get
            {
                return new Point(_Bounds.X, _Bounds.Y);
            }
        }

        /// <summary>
        /// Gets the width and width of camera.
        /// </summary>
        public Point Size
        {
            get
            {
                return new Point(_Bounds.Width, _Bounds.Height);
            }
        }

        /// <summary>
        /// Gets the bounding rectangle of the camera.
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(new Point(), Size);
            }
        }

        /// <summary>
        /// Returns the offset on the x axis for drawing objects relative
        /// to the camera.
        /// </summary>
        public int OffsetX
        {
            get
            {
                return -_Bounds.X;
            }
        }

        /// <summary>
        /// Returns the offset on the y axis for drawing objects relative
        /// to the camera.
        /// </summary>
        public int OffsetY
        {
            get
            {
                return -_Bounds.Y;
            }
        }

        /// <summary>
        /// A class that represents the camera that is used to render scenes.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Camera(int x, int y, int width, int height)
        {
            _Bounds = new Rectangle(x, y, width, height);
        }
        
        /// <summary>
        /// Centers the camera view on a given position.
        /// </summary>
        /// <param name="tx">X to center camera on.</param>
        /// <param name="ty">Y to center camera on.</param>
        public void CenterOnTarget(int tx, int ty)
        {
            _Bounds.X = tx - _Bounds.Width / 2;
            _Bounds.Y = ty - _Bounds.Height / 2;
        }

        /// <summary>
        /// Sets the position of the camera.
        /// </summary>
        /// <param name="x">Left position.</param>
        /// <param name="y">Top position.</param>
        public void SetPosition(int x, int y)
        {
            _Bounds.X = x;
            _Bounds.Y = y;
        }
    }
}
