using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SokobanGame.Screen;
using SokobanGame.Input;

namespace SokobanGame
{
    public class SokobanGame : Game
    {
        public static SokobanGame Instance { get; private set; }

        public static int Width { get { return Instance.GraphicsDevice.Viewport.Width; } }
        public static int Height { get { return Instance.GraphicsDevice.Viewport.Height; } }
        public static bool FullScreened { get; private set; } = false;

        public GraphicsDeviceManager Graphics { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        Color clearColor = new Color(117, 140, 142);

        int windowedWidth = 1280;
        int windowedHeight = 720;
        
        public SokobanGame()
        {
            if (Instance != null)
                throw new ApplicationException("SokobanGame.SokobanGame(): Only one instance of SokobanGame is allowed!");
            Instance = this;

            Graphics = new GraphicsDeviceManager(this);

            SetFullScreen(true);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            ScreenManager.Initialize(new MenuScreen());
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.LoadAssets(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            if (InputManager.KeyPressed(Keys.F))
            {
                SetFullScreen(!FullScreened);
            }

            ScreenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(clearColor);

            ScreenManager.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void DrawDebugMessage(string message, Vector2 position, Color color)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Assets.DebugFont, message, position, color);
            SpriteBatch.End();
        }

        private void SetFullScreen(bool fullscreen)
        {
            int fullWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int fullHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            if (fullscreen)
            {
                Graphics.PreferredBackBufferWidth = fullWidth;
                Graphics.PreferredBackBufferHeight = fullHeight;
            }
            else
            {
                Graphics.PreferredBackBufferWidth = windowedWidth;
                Graphics.PreferredBackBufferHeight = windowedHeight;
            }
            Graphics.ApplyChanges();

            FullScreened = fullscreen;
            IsMouseVisible = !fullscreen;
            Window.IsBorderless = fullscreen;
            // TODO: Window always fullscreens to primary monitor!
            Window.Position = fullscreen ? Point.Zero : new Point((fullWidth - windowedWidth) / 2, (fullHeight - windowedHeight) / 2);

            ScreenManager.Resized(Width, Height);
        }
    }
}
