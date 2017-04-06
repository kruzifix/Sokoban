﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame.Screen
{
    public class LevelSelectScreen : Screen
    {
        int selectedLevel = 0;

        int columns = 3;
        int padding = 20;
        int displayWidth;
        int displayHeight;

        private SpriteFont font;
        private SpriteBatch sb;

        public LevelSelectScreen()
            : base(true, true)
        {
            PositionLevels();

            sb = SokobanGame.Instance.SpriteBatch;
            font = Assets.SpacePortFont;
        }

        private void PositionLevels()
        {
            int width = SokobanGame.Instance.Graphics.GraphicsDevice.Viewport.Width;
            int height = SokobanGame.Instance.Graphics.GraphicsDevice.Viewport.Height;
            
            displayWidth = (width - columns * padding) / columns;
            displayHeight = displayWidth;

            // set level tilesizes for displaying
            for (int i = 0; i < Assets.Levels.Length; i++)
            {
                var lvl = Assets.Levels[i];
                lvl.Room.Reset();

                int tileSize = (int)MathHelper.Min(displayWidth / (float)lvl.Width, displayHeight / (float)lvl.Height);
                lvl.SetTileSize(tileSize, tileSize);

                int centerX = (displayWidth - lvl.PixelWidth) / 2;
                int centerY = (displayHeight - lvl.PixelHeight) / 2;

                int x = padding + (i%3) * (displayWidth + padding) + centerX;
                int y = (height - displayHeight) / 4 + centerY + (i/3) * (displayHeight + padding);

                lvl.RenderOffset = new IntVec(x, y);
            }
        }

        public override void Activated()
        {
            PositionLevels();
        }
        
        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.DrawDebugMessage("Level Select Screen", new Vector2(10, 10), Color.Black);
            SokobanGame.Instance.DrawDebugMessage(string.Format("selected Level: {0}", selectedLevel), new Vector2(260, 10), Color.Black);
            
            for (int i = 0; i < Assets.Levels.Length; i++)
            {
                var lvl = Assets.Levels[i];

                sb.Begin();
                sb.Draw(Assets.PixelTexture, new Rectangle(lvl.RenderOffset.X, lvl.RenderOffset.Y, lvl.PixelWidth, lvl.PixelHeight), Color.Black);
                sb.End();

                lvl.Draw();
                
                string lvlName;
                if (!lvl.Properties.TryGetValue("Name", out lvlName))
                    lvlName = "noname";
                if (i == selectedLevel)
                    lvlName = "--[ " + lvlName + " ]--";

                Vector2 textPos = lvl.RenderOffset.ToVector2();
                int height = SokobanGame.Instance.Graphics.GraphicsDevice.Viewport.Height;
                textPos.Y -= 35;
                
                Vector2 textSize = font.MeasureString(lvlName);
                textPos.X += (int)(displayWidth - textSize.X) / 2;
                //SokobanGame.Instance.DrawDebugMessage(lvlName, textPos, Color.Black);
                sb.Begin();
                sb.DrawString(font, lvlName, textPos, Color.Black);
                sb.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (KeyPress(Keys.Escape))
            {
                ScreenManager.RemoveScreen();
            }

            if (KeyPress(Keys.Enter))
            {
                ScreenManager.AddScreen(new GameScreen(selectedLevel));
            }

            if (KeyPress(Keys.Right))
            {
                selectedLevel++;
                if (selectedLevel >= Assets.Levels.Length)
                    selectedLevel = 0;
            }

            if (KeyPress(Keys.Left))
            {
                selectedLevel--;
                if (selectedLevel < 0)
                    selectedLevel = Assets.Levels.Length - 1;
            }
        }
    }
}
