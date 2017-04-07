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
                sb.DrawRect(tlx, tly, dw, dw, i == SelectedLevel ? Color.DimGray : ggray);
                sb.DrawRect(tlx, tly, dw, 40, i == SelectedLevel ? Color.DarkOliveGreen : Color.Gray);
                
                int tSize = (int)Math.Min((dw-20) / (float)lvl.Width, (dw - 40) / (float)lvl.Height);
                lvl.SetTileSize(tSize, tSize);
                float cx = (dw - lvl.PixelWidth) * 0.5f;
                float cy = (dw - lvl.PixelHeight) * 0.5f;
                lvl.RenderOffset = new IntVec(tlx + (int)cx, tly + 20 + (int)cy);
                lvl.Draw();
                
                sb.Begin();
                sb.DrawString(font, lvlName, new Vector2(tlx+dw*0.5f, tly + 20), i == SelectedLevel ? Color.GreenYellow : Color.White, Align.Center);
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
                SelectedLevel++;
                if (SelectedLevel >= Assets.Levels.Length)
                    SelectedLevel = 0;
            }

            if (InputManager.Pressed("left"))
            {
                SelectedLevel--;
                if (SelectedLevel < 0)
                    SelectedLevel = Assets.Levels.Length - 1;
            }
        }
    }
}
