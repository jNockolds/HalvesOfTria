using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using MonoGame.Extended;
using Halves_of_Tria.Classes;
using Halves_of_Tria.Systems;
using Halves_of_Tria.Input;
using Halves_of_Tria.Components;

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
        private Entity playerEntity;
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _world = new WorldBuilder()
            .AddSystem(new PlayerSystem())
            .AddSystem(new RenderSystem(_spriteBatch))
            .Build();

            Vector2 saltInitialPosition = new Vector2(WindowWidth / 2, WindowHeight / 2);
            Texture2D saltTexture = TextureGenerator.GenerateRectangleTexture(GraphicsDevice, 40, 120, Color.White, true);

            playerEntity = _world.CreateEntity();
            playerEntity.Attach(saltTexture);
            playerEntity.Attach(new Player(100, saltInitialPosition));
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            // [Todo: couple each InputAction to some code representing what it does in-game rather than hardcoding it here]
            if (InputManager.WasActionJustPressed(InputAction.QuickQuit))
                Exit();


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
