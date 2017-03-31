using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame.Screen
{
    public class LevelSelectScreen : Screen
    {
        int selectedLevel = 0;

        int columns = 3;
        int padding = 20;
        int displayWidth;
        int displayHeight;

        public LevelSelectScreen()
            : base(true, true)
        {
            PositionLevels();
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

                int centerX = (displayWidth - lvl.Width * tileSize) / 2;
                int centerY = (displayHeight - lvl.Height * tileSize) / 2;

                int x = padding + i * (displayWidth + padding) + centerX;
                int y = (height - displayHeight) / 2 + centerY;

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
            SokobanGame.Instance.DrawDebugMessage(string.Format("selected Level: {0}", selectedLevel), new Vector2(200, 10), Color.Black);

            for (int i = 0; i < Assets.Levels.Length; i++)
            {
                var lvl = Assets.Levels[i];
                lvl.Draw();
                
                string lvlName;
                if (!lvl.Properties.TryGetValue("Name", out lvlName))
                    lvlName = "noname";
                if (i == selectedLevel)
                    lvlName = "--[ " + lvlName + " ]--";

                Vector2 textPos = lvl.RenderOffset.ToVector2();
                int height = SokobanGame.Instance.Graphics.GraphicsDevice.Viewport.Height;
                textPos.Y = (height - displayHeight) / 2 - 35;
                
                Vector2 textSize = Assets.DebugFont.MeasureString(lvlName);
                textPos.X += (int)(displayWidth - textSize.X) / 2;
                SokobanGame.Instance.DrawDebugMessage(lvlName, textPos, Color.Black);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPress(Keys.Escape))
            {
                ScreenManager.Instance.RemoveScreen();
            }

            if (InputManager.Instance.KeyPress(Keys.Enter))
            {
                ScreenManager.Instance.AddScreen(new GameScreen(selectedLevel));
            }

            if (InputManager.Instance.KeyPress(Keys.Right))
            {
                selectedLevel++;
                if (selectedLevel >= Assets.Levels.Length)
                    selectedLevel = 0;
            }

            if (InputManager.Instance.KeyPress(Keys.Left))
            {
                selectedLevel--;
                if (selectedLevel < 0)
                    selectedLevel = Assets.Levels.Length - 1;
            }
        }
    }
}
