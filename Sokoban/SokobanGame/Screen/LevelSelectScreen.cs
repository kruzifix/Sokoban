using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame.Input;

namespace SokobanGame.Screen
{
    public class LevelSelectScreen : Screen
    {
        public int SelectedLevel { get; set; }

        int columns = 3;
        int padding = 20;

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

            float cos = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3.5);
            float op = 0.65f + 0.35f * cos;

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
                sb.DrawRect(tlx - 5, tly - 5, dw + 10, dw + 10, i == SelectedLevel ? ggray : Color.LightGray);
                Color bg = (i == SelectedLevel ? Color.DimGray : ggray);
                if (i == SelectedLevel)
                    bg *= op;
                sb.DrawRect(tlx, tly, dw, dw, bg);

                sb.DrawRect(tlx, tly, dw, 40, i == SelectedLevel ? Color.DarkOliveGreen : Color.Gray);
                
                int tSize = (int)Math.Min((dw-20) / (float)lvl.Width, (dw - 40) / (float)lvl.Height);
                lvl.SetTileSize(tSize, tSize);
                float cx = (dw - lvl.PixelWidth) * 0.5f;
                float cy = (dw - lvl.PixelHeight) * 0.5f;
                lvl.RenderOffset = new IntVec(tlx + (int)cx, tly + 20 + (int)cy);
                lvl.Draw();

                Color txtCol = i == SelectedLevel ? Color.GreenYellow : Color.White;
                sb.Begin();
                Vector2 size = font.MeasureString(lvlName);
                float scale = 1f;
                if (i == SelectedLevel)
                    scale = 0.97f - 0.03f * cos;
                sb.DrawString(font, lvlName, new Vector2(tlx + dw * 0.5f, tly + 20),
                               txtCol, 0f, size * 0.5f, scale, SpriteEffects.None, 0);
                sb.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Pressed("back"))
            {
                ScreenManager.RemoveScreen();
            }

            if (InputManager.Pressed("confirm"))
            {
                ScreenManager.AddScreen(new GameScreen(SelectedLevel));
            }

            if (InputManager.Pressed("right"))
            {
                if ((SelectedLevel % columns) < columns - 1 && SelectedLevel < Assets.Levels.Length - 1)
                    SelectedLevel++;
            }

            if (InputManager.Pressed("left"))
            {
                if ((SelectedLevel % columns) > 0 && SelectedLevel > 0)
                    SelectedLevel--;
            }

            if (InputManager.Pressed("up"))
            {
                if (SelectedLevel >= columns)
                    SelectedLevel -= columns;
            }

            if (InputManager.Pressed("down"))
            {
                if (SelectedLevel / columns < columns && SelectedLevel + columns < Assets.Levels.Length)
                    SelectedLevel += columns;
            }
        }
    }
}
