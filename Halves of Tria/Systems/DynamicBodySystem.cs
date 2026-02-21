using Halves_of_Tria.Components;
using Halves_of_Tria.Configuration;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System;
using System.Diagnostics;

namespace Halves_of_Tria.Systems
{
    internal class DynamicBodySystem : EntityProcessingSystem
    {
        #region Fields and Components
        private ComponentMapper<DynamicBody> _dynamicBodyMapper;
        private ComponentMapper<Transform2> _transformMapper;

        private float _requiredYVelocityForBounce = 200;
        private float _bounceIntensity = 50;
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

            ReactIfOnFloor(dynamicBody, transform);

            UpdateAllDynamicForces(dynamicBody, transform);

        }
        #endregion

        #region Public Methods
        public void AddImpulse(DynamicBody dynamicBody, Vector2 impulse)
        {
            dynamicBody.UnspentImpulse += impulse;
        }
        #endregion




        #region Temporary Methods
        private void ReactIfOnFloor(DynamicBody dynamicBody, Transform2 transform)
        {
            float floorY = GameHost.FloorLevel * 720;

            if (transform.Position.Y < floorY)
            {
                return;
            }

            transform.Position = new Vector2(transform.Position.X, floorY);

            float impactVelocity = dynamicBody.Velocity.Y;

            if (impactVelocity >= _requiredYVelocityForBounce)
            {
                AddImpulse(dynamicBody, new Vector2(0, -_bounceIntensity));
            }

            dynamicBody.Velocity = new(dynamicBody.Velocity.X, 0);
            dynamicBody.Acceleration = new(dynamicBody.Acceleration.X, 0);
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
            UpdateAllDynamicForces(dynamicBody, transform);
            ApplyForces(dynamicBody, transform, deltaTime);
        }

        private void ApplyImpulses(DynamicBody dynamicBody)
        {
            dynamicBody.Velocity += dynamicBody.UnspentImpulse * dynamicBody.InverseMass;
            ClearImpulses(dynamicBody);
        }

        private void ApplyForces(DynamicBody dynamicBody, Transform2 transform, float deltaTime)
        {

            dynamicBody.Acceleration = dynamicBody.ResultantForce * dynamicBody.InverseMass;
            dynamicBody.Velocity += dynamicBody.Acceleration * deltaTime;
            transform.Position += dynamicBody.Velocity * deltaTime;

            Debug.WriteLine($"Position: {transform.Position}, Velocity: {dynamicBody.Velocity}, Acceleration: {dynamicBody.Acceleration}");
        }

        private Vector2 GetResultantForce(DynamicBody dynamicBody)
        {
            Vector2 totalForce = Vector2.Zero;
            foreach (Vector2 value in dynamicBody.Forces.Values)
            {
                totalForce += value;
            }
            return totalForce;
        }

        private void UpdateForce(DynamicBody dynamicBody, ForceType forceType, Vector2 newValue, bool updateResultantForceAfter = true)
        {
            if (!dynamicBody.Forces.ContainsKey(forceType))
            {
                dynamicBody.Forces.Add(forceType, newValue);
            }
            else
            {
                dynamicBody.Forces[forceType] = newValue;
            }

            if (updateResultantForceAfter)
            {
                UpdateResultantForce(dynamicBody);
            }
        }

        private void UpdateAllDynamicForces(DynamicBody dynamicBody, Transform2 transform)
        {
            Vector2 newLinearDrag = -Config.DefaultLinearDragCoefficient * dynamicBody.Velocity;
            UpdateForce(dynamicBody, ForceType.LinearDrag, newLinearDrag, false); // false to prevent repeated redundant updating when more forces are updated here

            float floorY = GameHost.FloorLevel * 720;

            if (transform.Position.Y >= floorY)
            {
                // Sum vertical components of all forces except Normal
                float nonNormalVerticalForces = 0f;
                foreach (var force in dynamicBody.Forces)
                {
                    if (force.Key == ForceType.Normal) continue;
                    nonNormalVerticalForces += force.Value.Y;
                }

                // Normal should exactly cancel other vertical forces while on the floor.
                Vector2 normalForce = new Vector2(0f, -nonNormalVerticalForces);
                UpdateForce(dynamicBody, ForceType.Normal, normalForce, false);
            }
            else
            {
                UpdateForce(dynamicBody, ForceType.Normal, Vector2.Zero, false);
            }

            UpdateResultantForce(dynamicBody);
        }

        private void AddForce(DynamicBody dynamicBody, ForceType forceType, Vector2 value)
        {
            dynamicBody.Forces.Add(forceType, value);
        }

        private void ZeroForce(DynamicBody dynamicBody, ForceType forceType)
        {
            if (!dynamicBody.Forces.ContainsKey(forceType))
            {
                throw new ArgumentException($"The specified force type ({forceType}) does not exist in the DynamicBody's forces.", nameof(forceType));
            }
            dynamicBody.Forces[forceType] = Vector2.Zero;
        }

        private void RemoveForce(DynamicBody dynamicBody, ForceType forceType)
        {
            if (!dynamicBody.Forces.ContainsKey(forceType))
            {
                throw new ArgumentException($"The specified force type ({forceType}) does not exist in the DynamicBody's forces.", nameof(forceType));
            }
            dynamicBody.Forces.Remove(forceType);
        }

        private void ClearImpulses(DynamicBody dynamicBody)
        {
            dynamicBody.UnspentImpulse = Vector2.Zero;
        }
        #endregion

        #region Helper Methods
        private void UpdateResultantForce(DynamicBody dynamicBody)
        {
            dynamicBody.ResultantForce = GetResultantForce(dynamicBody);
        }
        #endregion
    }
}