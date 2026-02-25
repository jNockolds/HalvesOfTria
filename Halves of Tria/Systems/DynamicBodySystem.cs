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
        private ComponentMapper<PhysicsBody> _dynamicBodyMapper;
        private ComponentMapper<Transform2> _transformMapper;

        private float _requiredYVelocityForBounce = 200;
        private float _bounceIntensity = 50;
        #endregion

        public DynamicBodySystem()
            : base(Aspect.All(typeof(PhysicsBody), typeof(Transform2))) { }

        #region Game Loop Methods
        public override void Initialize(IComponentMapperService mapperService)
        {
            _dynamicBodyMapper = mapperService.GetMapper<PhysicsBody>();
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            PhysicsBody dynamicBody = _dynamicBodyMapper.Get(entityId);
            Transform2 transform = _transformMapper.Get(entityId);

            UpdateKinematics(dynamicBody, transform, gameTime);

            UpdateAllDynamicForces(dynamicBody, transform);
        }
        #endregion

        #region Public Methods
        public void AddImpulse(PhysicsBody dynamicBody, Vector2 impulse)
        {
            dynamicBody.UnspentImpulse += impulse;
        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// Updates the PhysicsBody's position, velocity, and acceleration, according to set forces acting upon it.
        /// </summary>
        /// <param name="gameTime">The elapsed game time information used to calculate the time step for the update. Must not be null.</param>
        private void UpdateKinematics(PhysicsBody dynamicBody, Transform2 transform, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            ApplyImpulses(dynamicBody);
            UpdateAllDynamicForces(dynamicBody, transform);
            ApplyForces(dynamicBody, transform, deltaTime);
        }

        private void ApplyImpulses(PhysicsBody dynamicBody)
        {
            dynamicBody.Velocity += dynamicBody.UnspentImpulse * dynamicBody.InverseMass;
            ClearImpulses(dynamicBody);
        }

        private void ApplyForces(PhysicsBody dynamicBody, Transform2 transform, float deltaTime)
        {
            Vector2 acceleration = dynamicBody.ResultantForce * dynamicBody.InverseMass;
            dynamicBody.Velocity += acceleration * deltaTime;
            transform.Position += dynamicBody.Velocity * deltaTime;

            Debug.WriteLine($"Position: {transform.Position}, Velocity: {dynamicBody.Velocity}, Acceleration: {acceleration}");
        }

        private Vector2 GetResultantForce(PhysicsBody dynamicBody)
        {
            Vector2 totalForce = Vector2.Zero;
            foreach (Vector2 value in dynamicBody.Forces.Values)
            {
                totalForce += value;
            }
            return totalForce;
        }

        private void UpdateForce(PhysicsBody dynamicBody, ForceType forceType, Vector2 newValue, bool updateResultantForceAfter = true)
        {
            dynamicBody.Forces[forceType] = newValue;

            if (updateResultantForceAfter)
            {
                UpdateResultantForce(dynamicBody);
            }
        }

        private void UpdateAllDynamicForces(PhysicsBody dynamicBody, Transform2 transform)
        {
            Vector2 newLinearDrag = -Config.DefaultLinearDragCoefficient * dynamicBody.Velocity;
            UpdateForce(dynamicBody, ForceType.LinearDrag, newLinearDrag, false); // false to prevent repeated redundant updating when more forces are updated here

            //float floorY = GameHost.FloorLevel * 720;

            //if (transform.Position.Y >= floorY)
            //{
            //    // Sum vertical components of all forces except Normal
            //    float nonNormalVerticalForces = 0f;
            //    foreach (var force in dynamicBody.Forces)
            //    {
            //        if (force.Key == ForceType.Normal) continue;
            //        nonNormalVerticalForces += force.Value.Y;
            //    }

            //    // Normal should exactly cancel other vertical forces while on the floor.
            //    Vector2 normalForce = new Vector2(0f, -nonNormalVerticalForces);
            //    UpdateForce(dynamicBody, ForceType.Normal, normalForce, false);
            //}
            //else
            //{
            //    UpdateForce(dynamicBody, ForceType.Normal, Vector2.Zero, false);
            //}

            UpdateResultantForce(dynamicBody);
        }

        private void ClearImpulses(PhysicsBody dynamicBody)
        {
            dynamicBody.UnspentImpulse = Vector2.Zero;
        }
        #endregion

        #region Helper Methods
        private void UpdateResultantForce(PhysicsBody dynamicBody)
        {
            dynamicBody.ResultantForce = GetResultantForce(dynamicBody);
        }
        #endregion
    }
}