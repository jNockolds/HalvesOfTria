using Halves_of_Tria.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System.Diagnostics;

namespace Halves_of_Tria.Systems
{
    // [BUG: gravity is being applied twice; linear drag isn't being applied]
    internal class DynamicBodySystem : EntityProcessingSystem
    {
        #region Fields and Components
        private ComponentMapper<DynamicBody> _dynamicBodyMapper;
        private ComponentMapper<Transform2> _transformMapper;
        #endregion

        public DynamicBodySystem()
            : base(Aspect.All(typeof(DynamicBody), typeof(Transform2))) { }

        #region Game Loop Methods
        public override void Initialize(IComponentMapperService mapperService)
        {
            _dynamicBodyMapper = mapperService.GetMapper<DynamicBody>();
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            DynamicBody dynamicBody = _dynamicBodyMapper.Get(entityId);
            Transform2 transform = _transformMapper.Get(entityId);

            UpdateKinematics(dynamicBody, transform, gameTime);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Updates the DynamicBody's position, velocity, and acceleration, according to set forces acting upon it.
        /// </summary>
        /// <param name="gameTime">The elapsed game time information used to calculate the time step for the update. Must not be null.</param>
        private void UpdateKinematics(DynamicBody dynamicBody, Transform2 transform, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            ApplyImpulses(dynamicBody);

            ApplyForces(dynamicBody, transform, deltaTime);
        }

        private void ApplyImpulses(DynamicBody dynamicBody)
        {
            dynamicBody.Velocity += dynamicBody.ResultantImpulse * dynamicBody.InverseMass;
            dynamicBody.ClearImpulses();
        }


        private void ApplyForces(DynamicBody dynamicBody, Transform2 transform, float dt)
        {
            // If you're debugging this... I wish you good fortune in the wars to come. Good luck!

            // Accumulate forces for this update:
            Vector2 initialPositionDependentForces = TotalNonVelocityDependentForces(dynamicBody, transform.Position);
            Vector2 initialAcceleration = initialPositionDependentForces * dynamicBody.InverseMass;

            // Predict position via Velocity Verlet integration:
            Vector2 newPosition = transform.Position + dynamicBody.Velocity * dt + 0.5f * initialAcceleration * (dt * dt);

            // Predict half-step velocity:
            Vector2 halfStepVelocity = dynamicBody.Velocity + 0.5f * initialAcceleration * dt;

            // Compute velocity-dependent forces at the half-step velocity:
            Vector2 velocityDependentForces = TotalVelocityDependentForces(dynamicBody, halfStepVelocity);

            // Compute position-dependent forces at the new position:
            Vector2 newPositionDependentForces = TotalNonVelocityDependentForces(dynamicBody, newPosition);

            // Sum all forces to get the total force at the new position and half-step velocity:
            Vector2 newResultantForce = newPositionDependentForces + velocityDependentForces;
            Vector2 newAcceleration = newResultantForce * dynamicBody.InverseMass;

            // Update velocity using the average of the initial and new acceleration:
            Vector2 newVelocity = dynamicBody.Velocity + 0.5f * (initialAcceleration + newAcceleration) * dt;

            // Update the dynamic body's state:
            transform.Position = newPosition;
            dynamicBody.Velocity = newVelocity;
            dynamicBody.Acceleration = newAcceleration;
        }

        private Vector2 TotalNonVelocityDependentForces(DynamicBody dynamicBody, Vector2 position)
        {
            dynamicBody.UpdateNonVelocityDependentForces(position);
            Vector2 totalForce = Vector2.Zero;
            foreach (Force force in dynamicBody.NonVelocityDependentForces)
            {
                totalForce += force.Value;
            }
            return totalForce;
        }

        private Vector2 TotalVelocityDependentForces(DynamicBody dynamicBody, Vector2 overrideVelocity)
        {
            dynamicBody.UpdateVelocityDependentForces(overrideVelocity);
            Vector2 totalForce = Vector2.Zero;
            foreach (Force force in dynamicBody.VelocityDependentForces)
            {
                totalForce += force.Value;
            }
            return totalForce;
        }
        #endregion
    }
}