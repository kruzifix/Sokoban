using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame.Screen
{
    public class LevelSelectScreen : Screen
    {
        int selectedLevel = 0;

        int columns = 3;
        int padding = 10;

        private SpriteFont font;

        public LevelSelectScreen()
            : base(true, true)
        {
            ResetAllLevels();

            font = Assets.SpacePortFont;
        }

        private void ResetAllLevels()
        {
            for (int i = 0; i < Assets.Levels.Length; i++)
            {
                var lvl = Assets.Levels[i];
                lvl.Room.Reset();
            }
        }

        public override void Activated()
        {
            ResetAllLevels();
        }
        
        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Color.LightSlateGray);
            
            int w = SokobanGame.Width;
            int h = SokobanGame.Height;

            int s = Math.Min(w, h);

            int dw = (s - (columns+1) * padding) / columns;
            int xo = (w - s) / 2;
            int yo = (h - s) / 2;

            for (int i = 0; i < Assets.Levels.Length; i++)
            {
                int x = i % columns;
                int y = i / columns;

                var lvl = Assets.Levels[i];

                string lvlName;
                if (!lvl.Properties.TryGetValue("Name", out lvlName))
                    lvlName = "noname";
                int tlx = xo + (dw + padding) * x + padding;
                int tly = yo + (dw + padding) * y + padding;
                Color ggray = new Color(51, 51, 51, 255);
                sb.DrawRect(tlx, tly, dw, dw, i == selectedLevel ? Color.DimGray : ggray);
                sb.DrawRect(tlx, tly, dw, 40, i == selectedLevel ? Color.DarkOliveGreen : Color.Gray);
                
                int tSize = (int)Math.Min((dw-20) / (float)lvl.Width, (dw - 40) / (float)lvl.Height);
                lvl.SetTileSize(tSize, tSize);
                float cx = (dw - lvl.PixelWidth) * 0.5f;
                float cy = (dw - lvl.PixelHeight) * 0.5f;
                lvl.RenderOffset = new IntVec(tlx + (int)cx, tly + 20 + (int)cy);
                lvl.Draw();
                
                sb.Begin();
                sb.DrawString(font, lvlName, new Vector2(tlx+dw*0.5f, tly + 20), i == selectedLevel ? Color.GreenYellow : Color.White, Align.Center);
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
