using Halves_of_Tria;
using Halves_of_Tria.Components;
using Halves_of_Tria.Configuration;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System.Diagnostics;

namespace Halves_of_Tria.Systems
{
    internal class DynamicBodySystem : EntityProcessingSystem
    {
        #region Fields and Components
        private ComponentMapper<DynamicBody> _dynamicBodyMapper;
        private ComponentMapper<Transform2> _transformMapper;

        private float _restitutionCoeff = 0.2f;
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

            BounceIfOnFloor(dynamicBody, transform);

        }
        #endregion

        #region Temporary Methods
        private void BounceIfOnFloor(DynamicBody dynamicBody, Transform2 transform)
        {
            // [Note: Maybe this works? The way the normal force doesn't ever zero feels weird?]
            // [Updated Note (after commenting out the normal force stuff): this works, but it's jittery when landing;
            // it'll do for now until the actual environmental box is implemented]

            if (transform.Position.Y >= Game1.FloorLevel * 720)
            {
                //int normalForceIndex = dynamicBody.Forces.FindIndex(x => x.Type == ForceType.Normal);

                //Force newNormalForce = new(ForceType.Normal, new(0, -dynamicBody.ResultantForce.Y));

                //UpdateForce(dynamicBody, newNormalForce);

                dynamicBody.Velocity = new(dynamicBody.Velocity.X, -_restitutionCoeff * dynamicBody.Velocity.Y);
            }
            else
            { 
                //Force newNormalForce = new(ForceType.Normal, Vector2.Zero);
                //UpdateForce(dynamicBody, newNormalForce);
            }
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
            UpdateForces(dynamicBody);
            dynamicBody.ResultantForce = GetResultantForce(dynamicBody);
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

            Debug.WriteLine($"Acceleration: {dynamicBody.Acceleration}; Velocity: {dynamicBody.Velocity}");
        }

        private Vector2 GetResultantForce(DynamicBody dynamicBody)
        {
            Vector2 totalForce = Vector2.Zero;
            foreach (Force force in dynamicBody.Forces)
            {
                totalForce += force.Value;
            }
            return totalForce;
        }

        public void UpdateForce(DynamicBody dynamicBody, Force newForce)
        {
            int forceIndex = dynamicBody.Forces.FindIndex(x => x.Type == newForce.Type);

            if (forceIndex >= 0) // if dynamicBody.Forces contains the force
            {
                dynamicBody.Forces[forceIndex] = newForce;
            }
        }

        public void UpdateForces(DynamicBody dynamicBody)
        {
            Force newLinearDrag = new(ForceType.LinearDrag, -Config.DefaultLinearDragCoefficient * dynamicBody.Velocity);
            UpdateForce(dynamicBody, newLinearDrag);
        }


        // Most of the following methods are not being used yet, but they could be useful when there are impulses and temporary forces in play:

        public void AddForce(DynamicBody dynamicBody, Force force)
        {
            int forceIndex = dynamicBody.Forces.FindIndex(x => x.Type == force.Type);

            if (forceIndex < 0) // if dynamicBody.Forces doesn't already contain the force
            {
                dynamicBody.Forces.Add(force);
            }
        }

        public void ZeroForce(DynamicBody dynamicBody, Force force)
        {
            int forceIndex = dynamicBody.Forces.FindIndex(x => x.Type == force.Type);

            if (forceIndex >= 0) // if dynamicBody.Forces contains the force
            {
                dynamicBody.Forces[forceIndex] = new(force.Type, Vector2.Zero);
            }
        }

        public void RemoveForce(DynamicBody dynamicBody, Force force)
        {
            dynamicBody.Forces.Remove(force);
        }

        public void ApplyImpulse(DynamicBody dynamicBody, Vector2 impulse)
        {
            dynamicBody.UnspentImpulse += impulse;
        }

        public void ClearImpulses(DynamicBody dynamicBody)
        {
            dynamicBody.UnspentImpulse = Vector2.Zero;
        }
        #endregion
    }
}