using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Halves_of_Tria.Classes
{
    /// <summary>
    /// A utility class for generating simple textures such as rectangles, squares, and circles.
    /// </summary>
    public static class TextureGenerator
    {
        #region Public Methods
        /// <summary>
        /// Generates semiMajorAxis rectangular texture with the specified dimensions, color, and style.
        /// </summary>
        /// <remarks>For unfilled rectangles, the border is drawn with the specified thickness.</remarks>
        /// <param name="graphicsDevice">The graphics device used to create the texture.</param>
        /// <param name="width">The width of the rectangle in pixels. Must be positive.</param>
        /// <param name="height">The height of the rectangle in pixels. Must be positive.</param>
        /// <param name="colour">The color to use for the rectangle.</param>
        /// <param name="isFilled">A value indicating whether the rectangle should be filled.  <see langword="true"/> to create semiMajorAxis filled
        /// rectangle; <see langword="false"/> to create an unfilled rectangle with semiMajorAxis border.</param>
        /// <param name="thickness">The thickness of the border for an unfilled rectangle. Ignored if <paramref name="isFilled"/> is <see
        /// langword="true"/>.  Must be positive.</param>
        /// <returns>A <see cref="Texture2D"/> representing the generated rectangle.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="width"/>, <paramref name="height"/>, or <paramref name="thickness"/> is less than
        /// or equal to zero.</exception>
        public static Texture2D GenerateRectangleTexture(GraphicsDevice graphicsDevice, int width, int height, Color colour, bool isFilled = false, int thickness = 1)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be positive.");
            }
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "Height must positive.");
            }
            if (thickness <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(thickness), "Thickness must be positive.");
            }

            Texture2D texture = new(graphicsDevice, width, height);
            Color[] flatPixelColours = new Color[width * height];

            if (isFilled)
            {
                flatPixelColours = MapFilledRectangle(flatPixelColours, width, height, colour);
            }
            else
            {
                flatPixelColours = MapUnfilledRectangle(flatPixelColours, width, height, colour, thickness);
            }

            texture.SetData(flatPixelColours);
            return texture;
        }

        /// <summary>
        /// Generates semiMajorAxis square texture with the specified side length, color, and style.
        /// </summary>
        /// <remarks>For unfilled squares, the border is drawn with the specified thickness.</remarks>
        /// <param name="graphicsDevice">The graphics device used to create the texture.</param>
        /// <param name="sideLength">The side length of the square in pixels. Must be positive.</param>
        /// <param name="colour">The color to use for the square.</param>
        /// <param name="isFilled">A value indicating whether the square should be filled.  <see langword="true"/> to create semiMajorAxis filled
        /// square; <see langword="false"/> to create an unfilled square with semiMajorAxis border.</param>
        /// <param name="thickness">The thickness of the border for an unfilled square. Ignored if <paramref name="isFilled"/> is <see
        /// langword="true"/>.  Must be positive.</param>
        /// <returns>A <see cref="Texture2D"/> representing the generated square.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="width"/>, <paramref name="height"/>, or <paramref name="thickness"/> is less than
        /// or equal to zero.</exception>
        public static Texture2D GenerateSquareTexture(GraphicsDevice graphicsDevice, int sideLength, Color colour, bool isFilled = false, int thickness = 1)
        {
            return GenerateRectangleTexture(graphicsDevice, sideLength, sideLength, colour, isFilled, thickness);
        }



        /// <summary>
        /// Generates an ellipse texture with the specified width, height, colour, and style.
        /// </summary>
        /// <remarks>For unfilled ellipses, the border is drawn with the specified thickness.</remarks>
        /// <param name="graphicsDevice">The graphics device used to create the texture.</param>
        /// <param name="width">The width of the ellipse texture in pixels. Must be positive.</param>
        /// <param name="height">The height of the ellipse texture in pixels. Must be positive.</param>
        /// <param name="colour">The colour to use for the ellipse.</param>
        /// <param name="isFilled">True to create semiMajorAxis filled ellipse; false to create an unfilled ellipse with semiMajorAxis border.</param>
        /// <param name="thickness">The thickness of the border for an unfilled ellipse. Ignored if isFilled is true. Must be >= 1 for unfilled.</param>
        /// <returns>A <see cref="Texture2D"/> representing the generated ellipse.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if width or height is less than or equal to zero, or thickness is less than one for an unfilled ellipse.</exception>
        public static Texture2D GenerateEllipseTexture(
            GraphicsDevice graphicsDevice,
            int width,
            int height,
            Color colour,
            bool isFilled = false,
            int thickness = 1)
        {
            if (graphicsDevice == null) throw new ArgumentNullException(nameof(graphicsDevice));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
            if (!isFilled && thickness < 1) throw new ArgumentOutOfRangeException(nameof(thickness));

            Color[] pixels = new Color[width * height];

            if (isFilled)
            {
                MapFilledEllipse(pixels, width, height, colour);
            }
            else
            {
                MapUnfilledEllipse(pixels, width, height, colour, thickness);
            }

            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            texture.SetData(pixels);
            return texture;
        }

        private static void MapFilledEllipse(Color[] flatPixelColours, int width, int height, Color colour)
        {
            int totalPixels = width * height;
            float semiMajorAxis = width / 2f;
            float semiMinorAxis = height / 2f;
            float centreX = semiMajorAxis;
            float centreY = semiMinorAxis;

            float invsSemiMajorAxisSquared = 1f / (semiMajorAxis * semiMajorAxis);
            float invSemiMinorAxisSquared = 1f / (semiMinorAxis * semiMinorAxis);

            for (int i = 0; i < totalPixels; i++)
            {
                int x = i % width;
                int y = i / width;

                float dx = (x + 0.5f) - centreX;
                float dy = (y + 0.5f) - centreY;

                if ((dx * dx) * invsSemiMajorAxisSquared + (dy * dy) * invSemiMinorAxisSquared <= 1f)
                {
                    flatPixelColours[i] = colour;
                }
                else
                {
                    flatPixelColours[i] = Color.Transparent;
                }
            }
        }

        private static void MapUnfilledEllipse(Color[] flatPixelColours, int width, int height, Color colour, int thickness)
        {
            int totalPixels = width * height;
            float semiMajorAxis = width / 2f;
            float semiMinorAxis = height / 2f;
            float centreX = semiMajorAxis;
            float centreY = semiMinorAxis;

            // Inner ellipse approximation: shrink each semi-axis by thickness.
            // If the thickness is large enough to collapse the inner ellipse, we treat the inner ellipse as degenerate.
            float innerA = Math.Max(0f, semiMajorAxis - thickness);
            float innerB = Math.Max(0f, semiMinorAxis - thickness);

            float invA2 = 1f / (semiMajorAxis * semiMajorAxis);
            float invB2 = 1f / (semiMinorAxis * semiMinorAxis);
            float invIA2 = innerA > 0f ? 1f / (innerA * innerA) : float.PositiveInfinity;
            float invIB2 = innerB > 0f ? 1f / (innerB * innerB) : float.PositiveInfinity;

            for (int i = 0; i < totalPixels; i++)
            {
                int x = i % width;
                int y = i / width;

                float dx = (x + 0.5f) - centreX;
                float dy = (y + 0.5f) - centreY;

                float outerE = (dx * dx) * invA2 + (dy * dy) * invB2;
                bool insideOuter = outerE <= 1f;

                bool outsideInner;
                if (innerA <= 0f || innerB <= 0f)
                {
                    // inner ellipse collapsed; treat insideInner as false so border fills the whole ellipse
                    outsideInner = true;
                }
                else
                {
                    float innerE = (dx * dx) * invIA2 + (dy * dy) * invIB2;
                    outsideInner = innerE >= 1f;
                }

                flatPixelColours[i] = (insideOuter && outsideInner) ? colour : Color.Transparent;
            }
        }



        /// <summary>
        /// Generates semiMajorAxis circle texture with the specified radius, color, and style.
        /// </summary>
        /// <remarks>For unfilled circles, the border is drawn with the specified thickness.</remarks>
        /// <param name="graphicsDevice">The graphics device used to create the texture.</param>
        /// <param name="radius">The radius of the circle in pixels. Must be positive.</param>
        /// <param name="colour">The color to use for the circle.</param>
        /// <param name="isFilled">A value indicating whether the circle should be filled.  <see langword="true"/> to create semiMajorAxis filled
        /// circle; <see langword="false"/> to create an unfilled circle with semiMajorAxis border.</param>
        /// <param name="thickness">The thickness of the border for an unfilled circle. Ignored if <paramref name="isFilled"/> is <see
        /// langword="true"/>.  Must be positive.</param>
        /// <returns>A <see cref="Texture2D"/> representing the generated circle.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="radius"/> or <paramref name="thickness"/> is less than
        /// or equal to zero.</exception>
        public static Texture2D GenerateCircleTexture(GraphicsDevice graphicsDevice, int radius, Color colour, bool isFilled = false, int thickness = 1)
        {
            if (thickness < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(thickness), "Thickness must be at least 1 pixel.");
            }
            if (radius < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be at least 1 pixel.");
            }
            if (thickness > radius)
            {
                throw new ArgumentOutOfRangeException(nameof(thickness), "Thickness must be less than or equal to the radius.");
            }

            int diameter = radius * 2;
            Texture2D circleTexture = new(graphicsDevice, diameter, diameter);
            Color[] flatPixelColours = new Color[diameter * diameter];

            if (isFilled)
            {
                flatPixelColours = MapFilledCircle(flatPixelColours, diameter, radius, colour);
            }
            else
            {
                flatPixelColours = MapUnfilledCircle(flatPixelColours, diameter, radius, colour, thickness);
            }

            circleTexture.SetData(flatPixelColours);
            return circleTexture;
        }
        #endregion



        #region Helper Methods
        private static Color[] MapUnfilledRectangle(Color[] flatPixelColours, int width, int height, Color colour, int thickness)
        {
            int totalPixels = width * height;
            for (int i = 0; i < totalPixels; i++)
            {
                int x = i % width;
                int y = i / width;
                if (IsPointOnRectangleBorder(x, y, width, height, thickness))
                {
                    flatPixelColours[i] = colour;
                }
                else
                {
                    flatPixelColours[i] = Color.Transparent;
                }
            }
            return flatPixelColours;
        }
        private static Color[] MapFilledRectangle(Color[] flatPixelColours, int width, int height, Color colour)
        {
            int totalPixels = width * height;
            for (int i = 0; i < totalPixels; i++)
            {
                flatPixelColours[i] = colour;
            }
            return flatPixelColours;
        }
        private static bool IsPointOnRectangleBorder(int x, int y, int width, int height, int thickness)
        {
            return x < thickness || x >= width - thickness || y < thickness || y >= height - thickness;
        }


        private static Color[] MapUnfilledCircle(Color[] flatPixelColours, int diameter, int radius, Color colour, int thickness)
        {
            int totalPixels = diameter * diameter;
            for (int i = 0; i < totalPixels; i++)
            {
                int x = i % diameter;
                int y = i / diameter;
                if (IsPointOnCircleBorder(x, y, radius, thickness))
                {
                    flatPixelColours[i] = colour;
                }
                else
                {
                    flatPixelColours[i] = Color.Transparent;
                }
            }
            return flatPixelColours;

        }
        private static Color[] MapFilledCircle(Color[] flatPixelColours, int diameter, int radius, Color colour)
        {
            int totalPixels = diameter * diameter;
            for (int i = 0; i < totalPixels; i++)
            {
                int x = i % diameter;
                int y = i / diameter;
                float distanceSquared = ((radius - x - 0.5f) * (radius - x - 0.5f)) + ((radius - y - 0.5f) * (radius - y - 0.5f));
                if (distanceSquared <= radius * radius)
                {
                    flatPixelColours[i] = colour;
                }
                else
                {
                    flatPixelColours[i] = Color.Transparent;
                }
            }
            return flatPixelColours;
        }
        private static bool IsPointOnCircleBorder(int x, int y, int radius, int thickness)
        {
            float distanceSquared = ((radius - x - 0.5f) * (radius - x - 0.5f)) + ((radius - y - 0.5f) * (radius - y - 0.5f));
            return (distanceSquared <= radius * radius) && (distanceSquared >= (radius - thickness) * (radius - thickness));
        }
        #endregion
    }
}
