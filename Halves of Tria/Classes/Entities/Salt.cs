using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Halves_of_Tria.Classes.Components;
using Halves_of_Tria.Classes;

namespace Halves_of_Tria.Classes.Entities
{
    class Salt : Entity
    {
        public Salt(Vector2 initialPosition, Texture2D texture)
        {
            Transform transform = new Transform();
            transform.Position = initialPosition;
            AddComponent(transform);

            Sprite sprite = new Sprite(texture);
            AddComponent(sprite);
        }
    }
}
