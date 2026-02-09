using Halves_of_Tria.Components;
using Halves_of_Tria.Input;
using Halves_of_Tria.PrimitiveTextures;
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

        protected override void Initialize()
        {
            WindowWidth = 1280;
            WindowHeight = 720;
            InputManager.Initialize();
            JsonLoader.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _world = new WorldBuilder()
            .AddSystem(new SaltMovementSystem())
            .AddSystem(new RenderSystem(_spriteBatch))
            .Build();


            Vector2 saltInitialPosition = new Vector2(WindowWidth / 2, WindowHeight / 2);
            Texture2D saltTexture = TextureGenerator.GenerateRectangleTexture(GraphicsDevice, 40, 120, Color.White, true);

            _salt = _world.CreateEntity();
            _salt.Attach(saltTexture);
            _salt.Attach(new Transform2(saltInitialPosition));
            _salt.Attach(new Speed(200));
            _salt.Attach(new DynamicBody());
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            // [Todo: couple each InputAction to some code representing what it does in-game rather than hardcoding it here]
            if (InputManager.WasActionJustPressed(InputAction.QuickQuit))
                Exit();

            if (InputManager.WasActionJustPressed(InputAction.ReloadConfig))
                JsonLoader.LoadConfig();

            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _world.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
