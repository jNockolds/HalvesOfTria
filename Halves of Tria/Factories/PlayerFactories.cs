using Halves_of_Tria.Components;
using Halves_of_Tria.Input;
using Halves_of_Tria.Textures;
using Halves_of_Tria.Systems;
using Halves_of_Tria.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using System.Diagnostics;


namespace Halves_of_Tria.Factories
{
    internal static class PlayerFactories
    {
        public static int CreateSalt(GraphicsDevice graphicsDevice, Vector2 position)
        {

            Texture2D _saltTexture = TextureGenerator.Rectangle(graphicsDevice, 40, 120, Color.White, true);

            Entity salt = Game1.WorldInstance.CreateEntity();
            salt.Attach(new SaltTag());
            salt.Attach(_saltTexture);
            salt.Attach(new Transform2(position));
            salt.Attach(new Speed(200));
            salt.Attach(new DynamicBody(1));

            return salt.Id;
        }
    }
}
