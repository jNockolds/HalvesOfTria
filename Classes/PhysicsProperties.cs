using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace HalvesOfTria.Classes
{
    public static class PhysicsProperties
    {
        public static readonly Vector2 AccelerationDueToGravity = new Vector2(0, 1140); // placeholder value
        public const float PlayerDragCoefficient = 0.01f; // placeholder value

        public enum MaterialType
        {
            Stone,
            Wood,
            Flesh,
            Thatch,
            Dirt,
            WetDirt,
            PerfectlyElastic,
            PerfectlyInelastic
        }

        private static Dictionary<MaterialType, float> BaseRestitution = new Dictionary<MaterialType, float>
        {
            { MaterialType.Stone,              0.35f },
            { MaterialType.Wood,               0.50f },
            { MaterialType.Flesh,              0.30f },
            { MaterialType.Thatch,             0.30f },
            { MaterialType.Dirt,               0.60f },
            { MaterialType.WetDirt,            0.40f },
            { MaterialType.PerfectlyElastic,   1f },
            { MaterialType.PerfectlyInelastic, 0f },
        };

        /// <summary>
        /// Calculates the combined restitution coefficient for two materials, using the root mean square of each material's 'base restitution'.
        /// </summary>
        /// <remarks>
        /// It is biased towards the larger value.
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when an invalid material type is provided.</exception>
        public static float GetCombinedRestitution(MaterialType material1, MaterialType material2)
        {
            if (BaseRestitution.TryGetValue(material1, out float restitution1) &&
                BaseRestitution.TryGetValue(material2, out float restitution2))
            {
                return RootMeanSquare(restitution1, restitution2);
            }
            throw new ArgumentException("Invalid material type(s) provided.");
        }

        /// <summary>
        /// Calculates the root mean square of two values.
        /// </summary>
        /// <remarks>
        /// It is biased towards the larger value, this method is used to combine restitution coefficients of two materials.
        /// </remarks>
        private static float RootMeanSquare(float a, float b)
        {
            return (float)Math.Sqrt((a * a + b * b) / 2);
        }
    }
}