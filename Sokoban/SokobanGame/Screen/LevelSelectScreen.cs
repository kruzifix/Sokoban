using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame.Input;

namespace SokobanGame.Screen
{
    public class LevelSelectScreen : Screen
    {
        public static bool LockLevels { get; set; } = true;

        private int selectedLevel;
        private int selectedPage;

        public int SelectedLevel
        {
            get
            {
                return selectedPage * columns * rows + selectedLevel;
            }
            set
            {
                selectedPage = value / (columns * rows);
                selectedLevel = value - selectedPage * columns * rows;
            }
        }

        public int UnlockedLevel { get; set; }

        int columns = 3;
        int rows = 2;
        int padding = 20;

        int topPad = 60;
        int borderSize = 10;

        private SpriteFont font;

        public LevelSelectScreen()
            : base(true, true)
        {
            ResetAllLevels();

            font = Assets.TextFont;
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
            int h = SokobanGame.Height - topPad - borderSize;

            sb.DrawRect(0, 0, w, topPad, Colors.PadBackground);
            sb.DrawRect(0, topPad, w, borderSize, Colors.PadBorder);

            float midTopPad = topPad * 0.5f;

            sb.Begin();
            if (selectedPage > 0)
                sb.DrawString(font, "<<<", new Vector2(40, midTopPad), Color.White, Align.MidLeft);

            int s = topPad / 2;
            var tr = sb.DrawString(font, "Select a Level", new Vector2(w * 0.5f + s, midTopPad), Color.White, Align.Center);

            Rectangle r = new Rectangle(tr.X - s * 2 - 4, (int)(midTopPad - s), s * 2, s * 2);
            sb.Draw(Assets.Keys, r, Assets.SrcEnter, Color.White);

            if (Assets.Levels.Length > (selectedPage + 1) * columns * rows)
                sb.DrawString(font, ">>>", new Vector2(w - 40, midTopPad), Color.White, Align.MidRight);
            sb.End();

            int dwi = (w - (columns + 1) * padding) / columns;
            int dhe = (h - (rows + 1) * padding) / rows;

            int dw = Math.Min(dwi, dhe);

            int xo = (w - dw * columns - (columns + 1) * padding) / 2;
            int yo = (h - dw * rows - (rows + 1) * padding) / 2;

            float cos = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3.5);
            float op = 0.65f + 0.35f * cos;

            int lvlStart = selectedPage * columns * rows;
            int lvlEnd = Math.Min((selectedPage + 1) * columns * rows, Assets.Levels.Length);

            for (int i = 0; i < Math.Min(columns * rows, Assets.Levels.Length); i++)
            {
                int x = i % columns;
                int y = i / columns;

                int j = lvlStart + i;
                if (j >= Assets.Levels.Length)
                    break;
                var lvl = Assets.Levels[j];

                string lvlName;
                if (!lvl.Properties.TryGetValue("Name", out lvlName))
                    lvlName = "noname";
                int tlx = xo + (dw + padding) * x + padding;
                int tly = (topPad + borderSize) + yo + (dw + padding) * y + padding;

                Color ggray = new Color(51, 51, 51);
                sb.DrawRect(tlx - 5, tly - 5, dw + 10, dw + 10, i == selectedLevel ? Colors.BoxBorderSel : Colors.BoxBorder);
                Color bg = (i == selectedLevel ? Colors.BoxBackSel : Colors.BoxBack);
                if (i == selectedLevel)
                    bg *= op;
                sb.DrawRect(tlx, tly, dw, dw, bg);

                sb.DrawRect(tlx, tly, dw, 40, i == selectedLevel ? Colors.BoxTextBackSel : Colors.BoxTextBack);

                int tSize = (int)Math.Min((dw - 20) / (float)lvl.Width, (dw - 40) / (float)lvl.Height);
                lvl.SetTileSize(tSize, tSize);
                float cx = (dw - lvl.PixelWidth) * 0.5f;
                float cy = (dw - lvl.PixelHeight) * 0.5f;
                lvl.RenderOffset = new IntVec(tlx + (int)cx, tly + 20 + (int)cy);
                lvl.Draw();

                Color txtCol = i == selectedLevel ? Colors.BoxTextSel : Colors.BoxText;
                sb.Begin();
                Vector2 size = font.MeasureString(lvlName);
                float scale = 1f;
                if (i == selectedLevel)
                    scale = 0.97f - 0.03f * cos;
                sb.DrawString(font, lvlName, new Vector2(tlx + dw * 0.5f, tly + 20),
                               txtCol, 0f, size * 0.5f, scale, SpriteEffects.None, 0);
                sb.End();

                if (LockLevels && j > UnlockedLevel)
                {
                    // draw lock
                    Color back = new Color(27, 27, 27, 158);
                    sb.DrawRect(tlx, tly + 40, dw, dw - 40, back);
                }
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
                if (!LockLevels || (SelectedLevel <= UnlockedLevel))
                    ScreenManager.AddScreen(new GameScreen(SelectedLevel));
            }

            int maxIndex = columns * rows;

            if (InputManager.Pressed("right"))
            {
                if ((selectedLevel % columns) == columns - 1)
                {
                    selectedPage++;
                    selectedLevel -= (columns - 1);
                    if (SelectedLevel >= Assets.Levels.Length)
                        selectedLevel = 0;
                }
                else if ((selectedLevel % columns) < columns - 1 && selectedLevel < maxIndex - 1)
                    selectedLevel++;
            }

            if (InputManager.Pressed("left"))
            {
                if ((selectedLevel % columns) == 0 && selectedPage > 0)
                {
                    selectedPage--;
                    selectedLevel += (columns - 1);
                }
                else if ((selectedLevel % columns) > 0)
                    selectedLevel--;
            }

            if (InputManager.Pressed("up"))
            {
                if (selectedLevel >= columns)
                    selectedLevel -= columns;
            }

            if (InputManager.Pressed("down"))
            {
                int max = Math.Min(maxIndex, Assets.Levels.Length - selectedPage * columns * rows);
                if (selectedLevel / columns < columns && selectedLevel + columns < max)
                    selectedLevel += columns;
            }
            if (SelectedLevel >= Assets.Levels.Length)
                SelectedLevel = Assets.Levels.Length - 1;
        }
    }
}
