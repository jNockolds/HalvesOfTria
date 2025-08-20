using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HalvesOfTria.Classes;
using HalvesOfTria.Interfaces;
using System.Collections.Generic;

namespace HalvesOfTria
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static int WindowWidth;
        public static int WindowHeight;

        // collections:
        public static List<EntityNode> EntityNodes { get; set; }

        // test variables:
        private Texture2D _testTexture;
        private PlayerSalt _testPlayerSalt;

        public static RoomBoundary _testRoomBoundary;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            WindowWidth = 1280;
            WindowHeight = 9 * WindowWidth / 16; // keeps a 16:9 aspect ratio
            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // test variables:
            _testTexture = TextureMaker.GenerateCircleTexture(GraphicsDevice, 20, Color.Red, false);
            _testPlayerSalt = new PlayerSalt(
                _testTexture,
                new Vector2(300, 300)
            );
            _testRoomBoundary = new RoomBoundary(
                GraphicsDevice,
                8 * WindowWidth / 10,
                8 * WindowHeight / 10
            );

            EntityNodes = new List<EntityNode> { _testPlayerSalt };
        }

        protected override void Update(GameTime gameTime)
        {
            if (InputManager.IsActionJustPressed(InputAction.QuickQuit))
            {
                Exit();
            }

            _testPlayerSalt.Update(gameTime);
            _testRoomBoundary.Update(gameTime);

            base.Update(gameTime);
            InputManager.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin();
            _testPlayerSalt.Draw(_spriteBatch);
            _testRoomBoundary.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
