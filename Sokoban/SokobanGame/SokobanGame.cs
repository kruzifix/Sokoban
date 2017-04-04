﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SokobanGame.Screen;
using SokobanGame.Tiled;

namespace SokobanGame
{
    public class SokobanGame : Game
    {
        public static SokobanGame Instance { get; private set; }

        public GraphicsDeviceManager Graphics { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        Color clearColor = new Color(117, 140, 142);

        bool fullscreened = false;
        int windowedWidth = 720;
        int windowedHeight = 720;

        public SokobanGame()
        {
            if (Instance != null)
                throw new ApplicationException("SokobanGame.SokobanGame(): Only one instance of SokobanGame is allowed!");
            Instance = this;

            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = windowedWidth;
            Graphics.PreferredBackBufferHeight = windowedHeight;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();

            ScreenManager.Initialize(new MenuScreen());
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.LoadAssets(Content);
        }

        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();

            if (InputManager.Instance.KeyPress(Keys.F))
            {
                int fullWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                int fullHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

                if (fullscreened)
                {
                    Graphics.PreferredBackBufferWidth = windowedWidth;
                    Graphics.PreferredBackBufferHeight = windowedHeight;
                }
                else
                {
                    Graphics.PreferredBackBufferWidth = fullWidth;
                    Graphics.PreferredBackBufferHeight = fullHeight;
                }
                Graphics.ApplyChanges();

                fullscreened = !fullscreened;
                Window.IsBorderless = fullscreened;
                // TODO: Window always fullscreens to primary monitor!
                Window.Position = fullscreened ? Point.Zero : new Point((fullWidth - windowedWidth) / 2, (fullHeight - windowedHeight) / 2);

                ScreenManager.Resized(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
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
    }
}
