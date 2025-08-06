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
        #region Game Properties
        public static int WindowWidth => Game1.WindowWidth;
        public static int WindowHeight => Game1.WindowHeight;
        #endregion


        #region Player Constants
        public const float PlayerWalkingForce = 900; // placeholder value
        public const float PlayerJumpForce = 500; // placeholder value
        public const float PlayerJumpCutFactor = 0.5f; // placeholder value
        #endregion


        #region Physics Constants
        public static readonly Vector2 AccelerationDueToGravity = new Vector2(0, 1140); // placeholder value
        public const float MinimumSpeed = 10f; // minimum speed for any object to be considered moving
        #endregion


        #region Player Properties
        public const float SidewaysPlayerDragCoefficient = 0.02f; // placeholder value
        public const float DownwardsPlayerDragCoefficient = 0.001f; // placeholder value


        public const float _walkingForce = 1200; // placeholder value
        #endregion


        #region Fields
        public enum MaterialType
        {
            Stone,
            Wood,
            Player,
            SoftCreature,
            HardCreature,
            Thatch,
            DryDirt,
            WetDirt,
            Sand,

            ElasticLowFriction,
            ElasticHighFriction,
            InelasticLowFriction,
            InelasticHighFriction
        }

        private static Dictionary<MaterialType, float> BaseRestitution = new Dictionary<MaterialType, float>
        {
            { MaterialType.Stone,                 0.55f },
            { MaterialType.Wood,                  0.45f },
            { MaterialType.Player,                0.00f },
            { MaterialType.SoftCreature,          0.20f },
            { MaterialType.HardCreature,          0.40f },
            { MaterialType.Thatch,                0.20f },
            { MaterialType.DryDirt,               0.50f },
            { MaterialType.WetDirt,               0.35f },
            { MaterialType.Sand,                  0.05f },

            { MaterialType.ElasticLowFriction,    1.00f },
            { MaterialType.ElasticHighFriction,   1.00f },
            { MaterialType.InelasticLowFriction,  0.00f },
            { MaterialType.InelasticHighFriction, 0.00f },
        };

        private static Dictionary<MaterialType, float> BaseFriction = new Dictionary<MaterialType, float>
        {
            { MaterialType.Stone,                 0.60f },
            { MaterialType.Wood,                  0.40f },
            { MaterialType.Player,                0.30f },
            { MaterialType.SoftCreature,          0.50f },
            { MaterialType.HardCreature,          0.30f },
            { MaterialType.Thatch,                0.50f },
            { MaterialType.DryDirt,               0.80f },
            { MaterialType.WetDirt,               0.65f },
            { MaterialType.Sand,                  0.55f },

            { MaterialType.ElasticLowFriction,    0.00f },
            { MaterialType.ElasticHighFriction,   1.00f },
            { MaterialType.InelasticLowFriction,  0.00f },
            { MaterialType.InelasticHighFriction, 1.00f },
        };
        #endregion


        #region Material Properties For Collisions
        /// <summary>
        /// Calculates the combined restitution coefficient for two materials, using the geometric mean of each material's 'base restitution'.
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
                return GeometricMean(restitution1, restitution2);
            }
            throw new ArgumentException("Invalid material type(s) provided.");
        }

        /// <summary>
        /// Calculates the combined friction coefficient for two materials, using the geometric mean of each material's 'base friction'.
        /// </summary>
        /// <remarks>
        /// It is biased towards the larger value.
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when an invalid material type is provided.</exception>
        public static float GetCombinedFriction(MaterialType material1, MaterialType material2)
        {
            if (BaseFriction.TryGetValue(material1, out float friction1) &&
                BaseFriction.TryGetValue(material2, out float friction2))
            {
                return GeometricMean(friction1, friction2);
            }
            throw new ArgumentException("Invalid material type(s) provided.");
        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// Calculates the geometric mean of two values.
        /// </summary>
        private static float GeometricMean(float a, float b)
        {
            return (float)Math.Sqrt(a * b);
        }
        #endregion
    }
}