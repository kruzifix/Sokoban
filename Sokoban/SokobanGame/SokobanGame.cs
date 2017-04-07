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

        int windowedWidth = 720;
        int windowedHeight = 720;

        //Vector2[] hist = new Vector2[20];
        //int pos = 0;

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

            //SpriteBatch.DrawRect(Width / 2, Height / 2, 400, 400, Color.White, Align.Center);
            //SpriteBatch.DrawRect(Width / 2, Height / 2, 190, 75, Color.DodgerBlue, Align.MidLeft);
            //SpriteBatch.DrawRect(Width / 2, Height / 2, 190, 75, Color.DodgerBlue, Align.MidRight);
            //SpriteBatch.DrawRect(Width / 2, Height / 2, 75, 190, Color.DodgerBlue, Align.TopMid);
            //SpriteBatch.DrawRect(Width / 2, Height / 2, 75, 190, Color.DodgerBlue, Align.BotMid);

            //SpriteBatch.DrawRect(Width / 2 + 200, Height / 2, 60, 125, Color.Brown, Align.MidRight);
            //SpriteBatch.DrawRect(Width / 2 - 200, Height / 2, 60, 125, Color.Brown, Align.MidLeft);
            //SpriteBatch.DrawRect(Width / 2, Height / 2 - 200, 125, 60, Color.Brown, Align.TopMid);
            //SpriteBatch.DrawRect(Width / 2, Height / 2 + 200, 125, 60, Color.Brown, Align.BotMid);

            //SpriteBatch.DrawRect(Width / 2, Height / 2, 100, 100, Color.ForestGreen, Align.Center);

            //hist[pos] = InputManager.LeftThumb;
            //pos = (pos + 1) % hist.Length;
            //for (int i = 0; i < hist.Length; i++)
            //{
            //    int d = pos - i - 1;
            //    if (d < 0)
            //        d += hist.Length;
            //    float op = 1f - d / (float)hist.Length;
            //    SpriteBatch.DrawRect(Width / 2 + hist[i].X * 200, Height / 2 - hist[i].Y * 200, 20, 20, Color.Black * op, Align.Center);
            //    SpriteBatch.DrawRect(Width / 2 + hist[i].X * 200, Height / 2 - hist[i].Y * 200, 8, 60, Color.Black * op, Align.Center);
            //    SpriteBatch.DrawRect(Width / 2 + hist[i].X * 200, Height / 2 - hist[i].Y * 200, 60, 8, Color.Black * op, Align.Center);
            //}

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
