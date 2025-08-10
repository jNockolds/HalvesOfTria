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
        public const float PlayerMass = 1f; // placeholder value
        public const float PlayerWalkingForce = 600; // placeholder value
        public const float PlayerJumpForce = 600; // placeholder value
        public const float PlayerJumpCutFactor = 0.5f; // placeholder value
        #endregion


        #region Physics Constants
        public static readonly Vector2 AccelerationDueToGravity = new Vector2(0, 1000); // placeholder value

        public const float MinimumSpeed = 10f; // minimum speed for any object to be considered moving
        public const float DragCoefficient = 0.013f;
        #endregion


        #region Restitution and Friction
        private static float _restitutionBase = 0.405f;
        private static float _frictionBase = 0.3f;

        public enum RestitutionModifier
        {
            Medium,
            High,
            Low,
        }

        public enum FrictionModifier
        {
            Medium,
            High,
            Low,
        }

        private static Dictionary<RestitutionModifier, float> RestitutionModifierValues = new Dictionary<RestitutionModifier, float>
        {
            { RestitutionModifier.Medium,  0.0f },
            { RestitutionModifier.High,    0.2f },
            { RestitutionModifier.Low,    -0.2f }
        };

        private static Dictionary<FrictionModifier, float> FrictionModifierValues = new Dictionary<FrictionModifier, float>
        {
            { FrictionModifier.Medium, 0.0f },
            { FrictionModifier.High,   0.2f },
            { FrictionModifier.Low,   -0.2f }
        };

        /// <summary>
        /// Calculates the combined restitution coefficient based on two restitution modifiers.
        /// </summary>
        /// <param name="modifier1">The restitution modifier for the first object.</param>
        /// <param name="modifier2">The restitution modifier for the second object.</param>
        /// <returns>
        /// A <see cref="float"/> representing the combined restitution value, 
        /// which is the base restitution adjusted by the modifier values.
        /// The returned value is guaranteed to be between 0.0 and 1.0 inclusive.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if either <paramref name="modifier1"/> or <paramref name="modifier2"/> is not a valid key in the restitution modifier dictionary.
        /// </exception>
        public static float GetRestitution(RestitutionModifier modifier1, RestitutionModifier modifier2)
        {
            if (RestitutionModifierValues.TryGetValue(modifier1, out float modifier1Val) &&
                RestitutionModifierValues.TryGetValue(modifier2, out float modifier2Val))
            {
                return Math.Clamp(_restitutionBase + modifier1Val + modifier2Val, 0, 1);
            }
            throw new ArgumentException("Invalid restitution modifier(s) provided.");
        }

        /// <summary>
        /// Calculates the combined friction coefficient based on two friction modifiers.
        /// </summary>
        /// <param name="modifier1">The friction modifier for the first object.</param>
        /// <param name="modifier2"> The friction modifier for the second object.</param>
        /// <returns>
        /// A <see cref="float"/> representing the combined friction value,
        /// which is the base friction adjusted by the modifier values.
        /// The returned value is guaranteed to be greater than or equal to 0.0.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if either <paramref name="modifier1"/> or <paramref name="modifier2"/> is not a valid key in the friction modifier dictionary.
        /// </exception>
        public static float GetFriction(FrictionModifier modifier1, FrictionModifier modifier2)
        {
            if (FrictionModifierValues.TryGetValue(modifier1, out float modifier1Val) &&
                FrictionModifierValues.TryGetValue(modifier2, out float modifier2Val))
            {
                return Math.Clamp(_frictionBase + modifier1Val + modifier2Val, 0, 1);
            }
            throw new ArgumentException("Invalid friction modifier(s) provided.");
        }
        #endregion
    }
}