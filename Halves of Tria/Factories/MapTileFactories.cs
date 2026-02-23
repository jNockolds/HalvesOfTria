using Halves_of_Tria.Components;
using Halves_of_Tria.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;


namespace Halves_of_Tria.Factories
{
    internal class MapTileFactories
    {
        public static int CreateRectangle(GraphicsDevice graphicsDevice, Vector2 position, int width, int height, Color colour)
        {
            Texture2D texture = TextureGenerator.Rectangle(graphicsDevice, width, height, colour, true);

            Entity rectangle = GameHost.WorldInstance.CreateEntity();
            rectangle.Attach(new Transform2(position));
            rectangle.Attach(new AxisAlignedRectCollider(width, height));
            rectangle.Attach(texture);

            return rectangle.Id;
        }
    }
}
