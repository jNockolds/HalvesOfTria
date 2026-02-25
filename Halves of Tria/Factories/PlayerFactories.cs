using Halves_of_Tria.Components;
using Halves_of_Tria.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;

namespace Halves_of_Tria.Factories
{
    internal static class PlayerFactories
    {
        public static int CreateSalt(GraphicsDevice graphicsDevice, Vector2 position)
        {
            int width = 40;
            int height = 120;

            Texture2D saltTexture = TextureGenerator.Rectangle(graphicsDevice, width, height, Color.White, true);

            Entity salt = GameHost.WorldInstance.CreateEntity();
            salt.Attach(new Tags.Players.Salt());
            salt.Attach(new Transform(position));
            salt.Attach(new AxisAlignedRectCollider(width, height));
            salt.Attach(new PhysicsBody(1));
            salt.Attach(saltTexture);
            salt.Attach(new Speed(200));

            return salt.Id;
        }
    }
}
