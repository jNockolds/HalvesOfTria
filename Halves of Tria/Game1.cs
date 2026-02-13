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
        #endregion

        #region Fields
        // Class level definition
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private World _world;
        private Entity _salt;
        #endregion

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

            _world = new WorldBuilder()
            .AddSystem(new RenderSystem(_spriteBatch))
            .AddSystem(new DynamicBodySystem())
            .AddSystem(new SaltMovementSystem())
            .AddSystem(new DebugVectorRenderSystem(GraphicsDevice))
            .Build();


            Vector2 saltInitialPosition = new Vector2(WindowWidth / 2, WindowHeight / 2);
            Texture2D saltTexture = TextureGenerator.Rectangle(GraphicsDevice, 40, 120, Color.White, true);

            _salt = _world.CreateEntity();
            _salt.Attach(saltTexture);
            _salt.Attach(new Transform2(saltInitialPosition));
            _salt.Attach(new Speed(200));
            _salt.Attach(new DynamicBody(1));
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

            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _world.Draw(gameTime);
            base.Draw(gameTime);
        }
        #endregion
    }
}
