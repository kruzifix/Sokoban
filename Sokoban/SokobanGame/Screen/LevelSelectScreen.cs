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

        int horzPad = 80;
        int topPad = 60;
        int borderSize = 10;

        private SpriteFont font;
        private bool enteringLevel = false;
        private float animProg = 0f;

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
            enteringLevel = false;
            animProg = 0f;

            ResetAllLevels();
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Colors.LevelSelectScreenBackground);

            int w = SokobanGame.Width;
            int h = SokobanGame.Height - topPad - borderSize;

            float midTopPad = topPad * 0.5f;
            int s = topPad / 2 + 4;
            int ah = Assets.ArrowTexture.Height;
            int aw = Assets.ArrowTexture.Width;

            float cos = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3.5);
            if (enteringLevel)
                cos = 1f;
            float op = 0.65f + 0.35f * cos;

            float aof = 3f * cos;

            int lw = w - horzPad * 2;

            int dwi = (lw - (columns + 1) * padding) / columns;
            int dhe = (h - (rows + 1) * padding) / rows;

            int dw = Math.Min(dwi, dhe);

            int xo = (lw - dw * columns - (columns + 1) * padding) / 2;
            int yo = (h - dw * rows - (rows + 1) * padding) / 2;

            int lvlStart = selectedPage * columns * rows;
            int lvlEnd = Math.Min((selectedPage + 1) * columns * rows, Assets.Levels.Length);

            float aprog = MathHelper.Clamp(animProg, 0f, 1f);

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
                int tlx = horzPad + xo + (dw + padding) * x + padding;
                int tly = (topPad + borderSize) + yo + (dw + padding) * y + padding;

                if (enteringLevel)
                {
                    if (i == selectedLevel)
                    {
                        int nx = horzPad + xo + (dw + padding) * (columns / 2) + padding;
                        tlx = (int)MathHelper.Lerp(tlx, nx, aprog);
                        tly = (int)MathHelper.Lerp(tly, (topPad + borderSize) + yo + (h - dw) * 0.5f, aprog);
                    }
                    else
                    {
                        if (y == 0)
                            tly -= (int)(aprog * h * 0.75f);
                        else
                            tly += (int)(aprog * h);
                    }
                }

                int tw = dw;

                sb.DrawRect(tlx - 5, tly - 5, tw + 10, tw + 10, i == selectedLevel ? Colors.BoxBorderSel : Colors.BoxBorder);
                Color bg = (i == selectedLevel ? Colors.BoxBackSel : Colors.BoxBack);
                if (i == selectedLevel)
                    bg *= op;
                sb.DrawRect(tlx, tly, tw, tw, bg);

                sb.DrawRect(tlx, tly, tw, 40, i == selectedLevel ? Colors.BoxTextBackSel : Colors.BoxTextBack);

                int tSize = (int)Math.Min((tw - 20) / (float)lvl.Width, (tw - 40) / (float)lvl.Height);
                lvl.SetTileSize(tSize, tSize);
                float cx = (tw - lvl.PixelWidth) * 0.5f;
                float cy = (tw - lvl.PixelHeight) * 0.5f;
                lvl.RenderOffset = new IntVec(tlx + (int)cx, tly + 20 + (int)cy);
                lvl.Draw();

                Color txtCol = i == selectedLevel ? Colors.BoxTextSel : Colors.BoxText;
                sb.Begin();
                Vector2 size = font.MeasureString(lvlName);
                float scale = 1f;
                if (i == selectedLevel)
                    scale = 0.97f - 0.03f * cos;
                sb.DrawString(font, lvlName, new Vector2(tlx + tw * 0.5f, tly + 20),
                               txtCol, 0f, size * 0.5f, scale, SpriteEffects.None, 0);
                sb.End();

                if (LockLevels && j > UnlockedLevel)
                {
                    // draw lock
                    Color back = new Color(27, 27, 27, 158);
                    sb.DrawRect(tlx, tly + 40, tw, tw - 40, back);

                    int liw = Math.Min(Assets.LockIcon.Width / 2, tw - 40);
                    int lih = Math.Min(Assets.LockIcon.Height / 2, tw - 80);
                    int lsize = Math.Min(liw, lih);

                    Rectangle re = new Rectangle(tlx + (tw - lsize) / 2, tly + 40 + (tw - 40 - lsize) / 2, lsize, lsize);
                    sb.Begin();
                    sb.Draw(Assets.LockIcon, re, Color.White);
                    sb.End();
                }
            }

            sb.DrawRect(0, 0, w, topPad, Colors.PadBackground);
            sb.DrawRect(0, topPad, w, borderSize, Colors.PadBorder);

            sb.Begin();
            if (selectedPage > 0)
            {
                sb.Draw(Assets.ArrowTexture, new Vector2(15 + aof, topPad + borderSize + (h - ah) / 2), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
            }

            if (Assets.Levels.Length > (selectedPage + 1) * columns * rows)
            {
                sb.Draw(Assets.ArrowTexture, new Vector2(w - aw - 15 - aof, topPad + borderSize + (h - ah) / 2), Color.White);
            }

            var tr = sb.DrawString(font, "Select Level", new Vector2(w * 0.5f, midTopPad), Color.White, Align.Center);

            Rectangle r = new Rectangle(tr.X - s * 2 - 8, (int)(midTopPad - s), s * 2, s * 2);
            sb.Draw(Assets.Keys, r, Assets.SrcEnter, Color.White);
            r.X = tr.Right;
            sb.Draw(Assets.PadBtns, r, Assets.SrcA, Color.White);

            r.X = 40;
            sb.Draw(Assets.Keys, r, Assets.SrcEsc, Color.White);
            tr = sb.DrawString(font, "Go Back", new Vector2(r.Right, midTopPad), Colors.PadText, Align.MidLeft);
            r.X = tr.Right;
            sb.Draw(Assets.PadBtns, r, Assets.SrcB, Color.White);

            sb.End();
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (enteringLevel)
            {
                animProg += time * 2f;
                if (animProg >= 1.3f)
                    ScreenManager.AddScreen(new GameScreen(SelectedLevel));
            }

            if (InputManager.Pressed("back"))
            {
                ScreenManager.RemoveScreen();
            }

            if (InputManager.Pressed("confirm"))
            {
                if (!LockLevels || (SelectedLevel <= UnlockedLevel))
                {
                    enteringLevel = true;
                    animProg = 0f;
                }
            }

            if (enteringLevel)
                return;

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

            if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.U))
            {
                LockLevels = !LockLevels;
            }
        }
    }
}
