using Halves_of_Tria.Caches;
using Halves_of_Tria.Components;
using Halves_of_Tria.Configuration;
using Halves_of_Tria.Factories;
using Halves_of_Tria.Input;
using Halves_of_Tria.Systems;
using Halves_of_Tria.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using System.Diagnostics;


namespace Halves_of_Tria
{
    public class GameHost : Game
    {
        #region Properties
        public static GameHost Instance { get; private set; }
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

        public static World WorldInstance { get; private set; }
        #endregion

        #region Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        #endregion

        // test variables:
        public static float FloorLevel = 0.6f; // as a proportion of the window's height
        // end of test variables

        public GameHost()
        {
            Instance = this;
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
            CollisionsCache collisionsCache = new();

            WorldInstance = new WorldBuilder()
            .AddSystem(new SaltInputSystem())
            .AddSystem(new MiscInputSystem())
            .AddSystem(new CollisionDetectionSystem(collisionsCache))
            // [physics-related collision response systems go here]
            // [non-physics-related collision response systems go here]
            .AddSystem(new DynamicBodySystem())
            .AddSystem(new RenderSystem(_spriteBatch))
            .AddSystem(new DebugVectorRenderSystem(GraphicsDevice, _spriteBatch))
            .Build();


            PlayerFactories.CreateSalt(GraphicsDevice, new(WindowWidth / 2, 0.2f * WindowHeight));
            MapTileFactories.CreateRectangle(GraphicsDevice, new(WindowWidth / 2, 0.9f * WindowHeight), (int)(0.9f * WindowWidth), (int)(0.1f * WindowHeight), Color.Brown);
        }

        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update();
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
