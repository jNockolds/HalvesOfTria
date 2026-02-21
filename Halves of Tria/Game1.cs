using Halves_of_Tria.Components;
using Halves_of_Tria.Input;
using Halves_of_Tria.Textures;
using Halves_of_Tria.Systems;
using Halves_of_Tria.Configuration;
using Halves_of_Tria.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using System.Diagnostics;


namespace Halves_of_Tria
{
    public class Game1 : Game
    {
        #region Properties
        public int WindowWidth
        {
            get => _graphics.PreferredBackBufferWidth;
            set
            {
                _graphics.PreferredBackBufferWidth = value;
                _graphics.ApplyChanges();
            }
        }
        public int WindowHeight
        {
            get => _graphics.PreferredBackBufferHeight;
            set
            {
                _graphics.PreferredBackBufferHeight = value;
                _graphics.ApplyChanges();
            }
        }

        public static World WorldInstance;
        #endregion

        #region Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Entity _salt;
        #endregion

        // test variables:
        public static float FloorLevel = 0.6f; // as a proportion of the window's height
        // end of test variables

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        #region Game Loop Methods
        protected override void Initialize()
        {
            WindowWidth = 1280;
            WindowHeight = 720;
            InputHandler.Initialize();
            JsonLoader.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            WorldInstance = new WorldBuilder()
            .AddSystem(new RenderSystem(_spriteBatch))
            .AddSystem(new DynamicBodySystem())
            .AddSystem(new SaltMovementSystem())
            .AddSystem(new DebugVectorRenderSystem(GraphicsDevice, _spriteBatch))
            .Build();

            Vector2 saltInitialPosition = new Vector2(640, 360);
            PlayerFactories.CreateSalt(GraphicsDevice, saltInitialPosition);
        }

        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update();

            // [Todo: couple each InputAction to some code representing what it does in-game rather than hardcoding it here]
            if (InputHandler.WasActionJustPressed(InputAction.QuickQuit))
                Exit();

            if (InputHandler.WasActionJustPressed(InputAction.ReloadConfig))
                JsonLoader.LoadConfig();

            if (InputHandler.IsActionDown(InputAction.SaltDebugMove))
            {
                Vector2 mousePosition = InputHandler.GetMousePosition();
                Transform2 saltTransform = _salt.Get<Transform2>();
                saltTransform.Position = mousePosition;
            }

            WorldInstance.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            WorldInstance.Draw(gameTime);
            base.Draw(gameTime);
        }
        #endregion
    }
}
