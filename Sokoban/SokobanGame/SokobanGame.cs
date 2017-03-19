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

        public SpriteFont DebugFont { get; private set; }
                
        public SokobanGame()
        {
            if (Instance != null)
                throw new ApplicationException("SokobanGame.SokobanGame(): Only one instance of SokobanGame is allowed!");
            Instance = this;

            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 720;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            ScreenManager.CreateScreenManager(new MenuScreen());
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            DebugFont = Content.Load<SpriteFont>("debug_font");
        }

        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            ScreenManager.Instance.Update(gameTime);
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            ScreenManager.Instance.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void DrawDebugMessage(string message, Vector2 position, Color color)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(DebugFont, message, position, color);
            SpriteBatch.End();
        }
    }
}
