using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Halves_of_Tria.Classes;
using Halves_of_Tria.Classes.Entities;
using MonoGame.Extended.Input;

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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _saltTexture;
        private Salt _salt;
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

            _saltTexture = TextureGenerator.GenerateRectangleTexture(GraphicsDevice, 40, 120, Color.White, true);
            Vector2 saltInitialPosition = new Vector2(WindowWidth / 2, WindowHeight / 2);
            _salt = new Salt(saltInitialPosition, _saltTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            // [Todo: couple each InputAction to some code representing what it does in-game rather than hardcoding it here]
            if (InputManager.WasActionJustPressed(InputAction.QuickQuit))
                Exit();

            TransformSystem.Update(gameTime);
            SpriteSystem.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin();
            SpriteSystem.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
