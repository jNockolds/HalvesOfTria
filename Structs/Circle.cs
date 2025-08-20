using Microsoft.Xna.Framework;
using System;

namespace HalvesOfTria.Structs
{
    /// <summary>
    /// Describes a circle.
    /// </summary>
    public struct Circle
    {
        #region Properties
        public Vector2 Centre;
        public int Radius;
        public int RadiusSquared => Radius * Radius;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="Circle"/> struct.
        /// </summary>
        /// <param name="centre">The center point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <exception cref="ArgumentOutOfRangeException">"Thrown when the radius is negative."</exception>
        public Circle(Vector2 centre, int radius)
        {
            if (radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative.");
            }
            Centre = centre;
            Radius = radius;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Determines whether the specified point is contained within the circle.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns>
        /// <c>true</c> if the point is within or on the boundary of the circle; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Vector2 point)
        {
            return Vector2.DistanceSquared(Centre, point) <= RadiusSquared;
        }

        /// <summary>
        /// Determines whether this circle intersects with another circle.
        /// </summary>
        /// <param name="other">The other circle to check for intersection.</param>
        /// <returns>
        /// <c>true</c> if the circles intersect or touch; otherwise, <c>false</c>.
        /// </returns>
        public bool Intersects(Circle other)
        {
            float distanceSquared = Vector2.DistanceSquared(Centre, other.Centre);
            float radiusSum = Radius + other.Radius;
            return distanceSquared <= radiusSum * radiusSum;
        }

        /// <summary>
        /// Determines whether this circle intersects with a rectangle.
        /// </summary>
        /// <param name="other">The rectangle to check for intersection.</param>
        /// <returns>
        /// <c>true</c> if the two intersect or touch; otherwise, <c>false</c>.
        /// </returns>
        public bool Intersects(Rectangle rectangle)
        {
            // Check if the rectangle and circle's axis-aligned bounding box intersect:
            if (!GetAABB().Intersects(rectangle)) // if not, no collision occurs
            {
                return false;
            }

            // Find the closest point to the circle within the rectangle:
            float closestX = MathHelper.Clamp(Centre.X, rectangle.Left, rectangle.Right);
            float closestY = MathHelper.Clamp(Centre.Y, rectangle.Top, rectangle.Bottom);

            // Calculate the distance from the circle's center to this closest point:
            float distanceX = Centre.X - closestX;
            float distanceY = Centre.Y - closestY;
            // No absolute value needed, as we are squaring the distance anyway.

            // If the distance is less than the circle's radius, an intersection occurs:
            return (distanceX * distanceX + distanceY * distanceY) < RadiusSquared;
        }

        /// <summary>
        /// Calculates the axis-aligned bounding box (AABB) of the circle in the form of a Rectangle.
        /// </summary>
        /// <returns>
        /// A <see cref="Rectangle"/> that represents the smallest axis-aligned rectangle
        /// which completely contains the circle.
        /// </returns>
        public Rectangle GetAABB()
        {
            return new Rectangle(
                (int)(Centre.X - Radius),
                (int)(Centre.Y - Radius),
                Radius * 2,
                Radius * 2);
        }
        #endregion
    }
}
