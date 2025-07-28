using HalvesOfTria.Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace HalvesOfTria.Classes
{
    /// <summary>
    /// [TODO: add air resistance and add friction (when on surface); oppose motion whichever direction it is in]
    /// Describes a kinematic object that can be updated with forces and motion.
    /// </summary>
    /// <remarks>
    /// Time is measured in seconds, and distance in pixels.
    /// </remarks>
    public class PhysicsObject : IUpdateableObject
    {
        #region Properties
        public Vector2 Position { get; set; } // a _physicsObject's Position is at its centre unless otherwise specified
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; protected set; }
        public float Mass { get; protected set; }
        public Vector2 ResultantForce { get; protected set; }
        /// <remarks>
        /// Forces that are applied for a single frame (e.g. a jump).
        /// </remarks>
        public Vector2 ResultantInstantaneousForce { get; protected set; }
        /// <remarks>
        /// Currently designed to be constant (i.e. won't change if acceleration due to gravity changes or if heavy items are picked up)
        ///     [TODO: increase weight when picking up items]
        /// </remarks>
        public Vector2 Weight { get; protected set; }
        #endregion


        #region Fields
        private static readonly Vector2 AccelerationDueToGravity = new(0, 1140); // placeholder value
        private const float _minSpeed = 0.01f;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new _physicsObject with the specified initial position and mass.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if mass is less than or equal to zero.</exception>
        public PhysicsObject(Vector2 initialPosition, float mass)
        {
            if (mass <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(mass), "Mass must be greater than zero.");
            }
            Position = initialPosition;
            Mass = mass;
            Weight = Mass * AccelerationDueToGravity;
        }
        #endregion


        #region Update Method
        /// <summary>
        /// Updates the physics object by applying forces, integrating motion, and stopping it if it's sufficiently slow.
        /// </summary>
        /// <param name="gameTime">Provides the elapsed time since the last update, used for time-based calculations.</param>
        /// <remarks>
        /// This method performs the following steps:
        /// <list type="number">
        /// <item><description>Accumulates all forces acting on the object.</description></item>
        /// <item><description>Integrates the object's kinematics to update position and velocity.</description></item>
        /// <item><description>Stops the object if its horizontal velocity is below a defined threshold to prevent unnecessary movement.</description></item>
        /// </list>
        /// When adding forces each frame, make sure to do so before calling this method. Otherwise, the forces will be applied in the next frame.
        /// </remarks>
        public virtual void Update(GameTime gameTime)
        {
            ApplyForce(Weight);

            IntegrateKinematics((float)gameTime.ElapsedGameTime.TotalSeconds);

            StopIfSlow();
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds a force to <see cref="ResultantForce"/>.
        /// </summary>
        /// <param name="force">Force to be applied (Newtons).</param>
        public void ApplyForce(Vector2 force) => ResultantForce += force;

        /// <summary>
        /// Adds an instantaneous force to <see cref="ResultantInstantaneousForce"/> over a frame.
        /// </summary>
        /// <param name="instantaneousForce">Force to be applied (Newtons).</param>
        public void ApplyInstantaneousForce(Vector2 instantaneousForce) => ResultantInstantaneousForce += instantaneousForce;
        #endregion


        #region Misc. Methods
        /// <summary>
        /// If Velocity.X is close to zero (less than <see cref="_minSpeed"/>), set it to zero to stop the object from moving in the opposite direction.
        /// </summary>
        /// <remarks>
        /// Called in <see cref="Update"/> every frame.
        /// </remarks>
        protected void StopIfSlow()
        {
            if (Math.Abs(Velocity.X) < _minSpeed)
            {
                Velocity = new Vector2(0, Velocity.Y);
            }
        }

        /// <summary>
        /// Updates acceleration and integrates (Velocity Verlet scheme) to update the object's position and velocity based on the current acceleration.
        /// </summary>
        /// <param name="deltaTime">The time step in seconds since the last update.</param>
        /// 
        /// <remarks>
        /// <para>
        /// Integrating all at once like this means that deltaTime doesn't need to be used anywhere else 
        /// (since the game only applies forces to objects, never directly modifying positions or velocities (except for <see cref="StopIfSlow"/>)).
        /// </para>
        /// 
        /// <para>
        /// Uses Velocity Verlet integration to update motion by splitting the velocity change into 2 stages: 
        /// one before the Position is updated, and one after.
        /// 
        /// Applying the full veloicty update before or after the Position update causes directional innacuracies.
        /// This method effectivelt averages them over the time step to improve stability and accuracy.
        /// </para>
        /// 
        /// <para>
        /// Assumes that all external forces have been applied to <see cref="ResultantForce"/>
        /// and <see cref="Acceleration"/> is updated prior to calling this method.
        /// </para>
        /// </remarks>
        protected void IntegrateKinematics(float deltaTime)
        {
            Velocity += ResultantInstantaneousForce / Mass; // doesn't multiply by deltaTime because this is an instantaneous force
            ResultantInstantaneousForce = Vector2.Zero; // ensure forces dont't accumulate

            Acceleration = ResultantForce / Mass;
            ResultantForce = Vector2.Zero; // ensure forces dont't accumulate

            Velocity += 0.5f * Acceleration * deltaTime;
            Position += Velocity * deltaTime;
            Velocity += 0.5f * Acceleration * deltaTime;
        }
        #endregion

    }
}
