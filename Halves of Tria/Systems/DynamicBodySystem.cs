using Halves_of_Tria.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Halves_of_Tria.Systems
{
    internal class DynamicBodySystem : EntityUpdateSystem
    {
        private readonly Vector2 _accelerationDueToGravity = new Vector2(0f, 9.81f); // [TODO: don't hardcode this]
        private float _linearDragCoefficient = 0.1f; // [TODO: don't hardcode this]


        private ComponentMapper<DynamicBody> _dynamicBodyMapper;
        private ComponentMapper<Transform2> _transformMapper;

        public DynamicBodySystem()
            : base(Aspect.All(typeof(DynamicBody), typeof(Transform2))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _dynamicBodyMapper = mapperService.GetMapper<DynamicBody>();
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities)
            {
                DynamicBody dynamicBody = _dynamicBodyMapper.Get(entityId);
                Transform2 transform = _transformMapper.Get(entityId);

                UpdateKinematics(dynamicBody, transform, gameTime);
            }
        }

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
            Vector2 initialPositionDependentForces = PositionDependentForces(transform.Position, dynamicBody.Mass);
            Vector2 initialAcceleration = initialPositionDependentForces * dynamicBody.InverseMass;

            // Predict position via Velocity Verlet integration:
            Vector2 newPosition = transform.Position + dynamicBody.Velocity * dt + 0.5f * initialAcceleration * (dt * dt);

            // Predict half-step velocity:
            Vector2 halfStepVelocity = dynamicBody.Velocity + 0.5f * initialAcceleration * dt;

            // Compute velocity-dependent forces at the half-step velocity:
            Vector2 velocityDependentForces = VelocityDependentForces(halfStepVelocity);

            // Compute position-dependent forces at the new position:
            Vector2 newPositionDependentForces = PositionDependentForces(newPosition, dynamicBody.Mass);

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

        private Vector2 PositionDependentForces(Vector2 position, float mass)
        {
            Vector2 gravityForce = mass * _accelerationDueToGravity;

            return gravityForce; // [Apply any other position-dependent forces by summing them here]
        }

        private Vector2 VelocityDependentForces(Vector2 velocity)
        {
            Vector2 dragForce = -_linearDragCoefficient * velocity;
            return dragForce; // [Apply any other velocity-dependent forces by summing them here]
        }
        #endregion
    }
}