using Halves_of_Tria.Components;
using Halves_of_Tria.Configuration;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halves_of_Tria.Systems
{
    internal class VelocityUpdateSystem : EntityProcessingSystem
    {
        #region Fields and Components
        private ComponentMapper<PhysicsBody> _physicsBodyMapper;
        private ComponentMapper<Transform> _transformMapper;
        #endregion

        public VelocityUpdateSystem()
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
            Transform transform = _transformMapper.Get(entityId);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 deltaPosition = transform.Position - transform.PreviousPosition;

            if (deltaTime > 0f)
                physicsBody.Velocity = deltaPosition / deltaTime;
        }
        #endregion
    }
}
