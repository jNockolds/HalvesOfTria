using Halves_of_Tria.Components;
using Halves_of_Tria.Configuration;
using Halves_of_Tria.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS.Systems;

namespace Halves_of_Tria.Systems
{
    internal class MiscInputSystem : UpdateSystem
    {
        public MiscInputSystem() { }

        #region Game Loop Methods

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.WasActionJustPressed(InputAction.QuickQuit))
                GameHost.Instance.Exit();

            if (InputHandler.WasActionJustPressed(InputAction.ReloadConfig))
                JsonLoader.LoadConfig();
        }
        #endregion
    }
}
