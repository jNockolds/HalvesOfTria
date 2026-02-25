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
    internal class PhysicsBodySystem : EntityProcessingSystem
    {
        #region Fields and Components
        private ComponentMapper<PhysicsBody> _physicsBodyMapper;
        private ComponentMapper<Transform> _transformMapper;

        private float _requiredYVelocityForBounce = 200;
        private float _bounceIntensity = 50;
        #endregion

        public PhysicsBodySystem()
            : base(Aspect.All(typeof(PhysicsBody), typeof(Transform))) { }

        #region Game Loop Methods
        public override void Initialize(IComponentMapperService mapperService)
        {
            _physicsBodyMapper = mapperService.GetMapper<PhysicsBody>();
            _transformMapper = mapperService.GetMapper<Transform>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            PhysicsBody physicsBody = _physicsBodyMapper.Get(entityId);

            if (physicsBody.InverseMass == 0f) // static bodies don't need to be updated here
            {
                return;
            }

            Transform transform = _transformMapper.Get(entityId);

            UpdateKinematics(physicsBody, transform, gameTime);

            UpdateAllDynamicForces(physicsBody, transform);
        }
        #endregion

        #region Public Methods
        public void AddImpulse(PhysicsBody physicsBody, Vector2 impulse)
        {
            physicsBody.UnspentImpulse += impulse;
        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// Updates the PhysicsBody's position, velocity, and acceleration, according to set forces acting upon it.
        /// </summary>
        /// <param name="gameTime">The elapsed game time information used to calculate the time step for the update. Must not be null.</param>
        private void UpdateKinematics(PhysicsBody physicsBody, Transform transform, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            ApplyImpulses(physicsBody);
            UpdateAllDynamicForces(physicsBody, transform);
            VerletIntegrate(physicsBody, transform, deltaTime);
        }

        private void ApplyImpulses(PhysicsBody physicsBody)
        {
            physicsBody.Velocity += physicsBody.UnspentImpulse * physicsBody.InverseMass;
            ClearImpulses(physicsBody);
        }

        private void VerletIntegrate(PhysicsBody physicsBody, Transform transform, float deltaTime)
        {
            Vector2 approxVelocity = transform.Position - transform.PreviousPosition;
            Vector2 acceleration = physicsBody.ResultantForce * physicsBody.InverseMass;

            Vector2 newPosition = transform.Position + approxVelocity + acceleration * (deltaTime * deltaTime);

            transform.PreviousPosition = transform.Position;
            transform.Position = newPosition;
        }

        private Vector2 GetResultantForce(PhysicsBody physicsBody)
        {
            Vector2 totalForce = Vector2.Zero;
            foreach (Vector2 value in physicsBody.Forces.Values)
            {
                totalForce += value;
            }
            return totalForce;
        }

        private void UpdateForce(PhysicsBody physicsBody, ForceType forceType, Vector2 newValue, bool updateResultantForceAfter = true)
        {
            physicsBody.Forces[forceType] = newValue;

            if (updateResultantForceAfter)
            {
                UpdateResultantForce(physicsBody);
            }
        }

        private void UpdateAllDynamicForces(PhysicsBody physicsBody, Transform transform)
        {
            Vector2 newLinearDrag = -Config.DefaultLinearDragCoefficient * physicsBody.Velocity;
            UpdateForce(physicsBody, ForceType.LinearDrag, newLinearDrag, false); // false to prevent repeated redundant updating when more forces are updated here

            //float floorY = GameHost.FloorLevel * 720;

            //if (transform.Position.Y >= floorY)
            //{
            //    // Sum vertical components of all forces except Normal
            //    float nonNormalVerticalForces = 0f;
            //    foreach (var force in physicsBody.Forces)
            //    {
            //        if (force.Key == ForceType.Normal) continue;
            //        nonNormalVerticalForces += force.Value.Y;
            //    }

            //    // Normal should exactly cancel other vertical forces while on the floor.
            //    Vector2 normalForce = new Vector2(0f, -nonNormalVerticalForces);
            //    UpdateForce(physicsBody, ForceType.Normal, normalForce, false);
            //}
            //else
            //{
            //    UpdateForce(physicsBody, ForceType.Normal, Vector2.Zero, false);
            //}

            UpdateResultantForce(physicsBody);
        }

        private void ClearImpulses(PhysicsBody physicsBody)
        {
            physicsBody.UnspentImpulse = Vector2.Zero;
        }
        #endregion

        #region Helper Methods
        private void UpdateResultantForce(PhysicsBody physicsBody)
        {
            physicsBody.ResultantForce = GetResultantForce(physicsBody);
        }
        #endregion
    }
}