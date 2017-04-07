using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SokobanGame.Screen
{
    public class MenuScreen : Screen
    {
        private int selectedOption;
        private string[] options = {
            "Play!",
            "Tutorial",
            "Options",
            "Credits",
            "Exit"
        };

        private string title = "Sokoban";

        private SpriteFont titleFont;
        private SpriteFont font;

        float animProg = 0f;
        bool exiting = false;

        public MenuScreen()
            : base(true, true)
        {
            selectedOption = 0;

            titleFont = Assets.GreenwichFont;
            font = Assets.SpacePortFont;
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Color.LightSlateGray);

            int width = SokobanGame.Width;
            int height = SokobanGame.Height;

            sb.Begin();
            sb.DrawString(titleFont, title, new Vector2(width * 0.5f, 120), Color.White, Align.Center);
            sb.End();

            int textPadding = 60;
            int topOffset = height / 2 - options.Length * textPadding / 2 + 50;
            int btnWidth = 250;

            for (int i = 0; i < options.Length; i++)
            {
                float y = topOffset + i * textPadding;
                int pa = i == selectedOption ? 10 : 0;

                float x = width * 0.5f;
                float k = MathHelper.Clamp(animProg * animProg * (3 - 2 * animProg), 0f, 1f);
                y += (1-k) * (height * 0.5f + i * 80);
                Color bBorder = i == selectedOption ? Color.LightGray : Color.DimGray;
                bBorder *= k;
                Color bBackg = i == selectedOption ? Color.DarkOliveGreen : Color.Gray;
                bBackg *= k;

                sb.DrawRect(x, y, btnWidth + pa * 2 + 10, 50 + pa, bBorder, Align.Center);
                sb.DrawRect(x, y, btnWidth + pa * 2, 40 + pa, bBackg, Align.Center);

                sb.Begin();
                sb.DrawString(font, options[i], new Vector2(x, y), i == selectedOption ? Color.GreenYellow : Color.White, Align.Center);
                sb.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (exiting)
            {
                animProg -= time * 2f;
                if (animProg <= 0f)
                    SokobanGame.Instance.Exit();
            }
            else
            {
                animProg += time * 2f;
            }
            animProg = MathHelper.Clamp(animProg, 0f, 1f);

            if (InputManager.KeyPress(Keys.Escape))
            {
                exiting = true;
                return;
            }

            if (InputManager.KeyPress(Keys.Down))
            {
                selectedOption = (selectedOption + 1) % options.Length;
            }

            if (InputManager.KeyPress(Keys.Up))
            {
                selectedOption--;
                if (selectedOption < 0)
                    selectedOption = options.Length - 1;
            }

            if (InputManager.KeyPress(Keys.Enter))
            {
                switch (selectedOption)
                {
                    case 0:
                        ScreenManager.AddScreen(new LevelSelectScreen());
                        break;
                    case 4:
                        exiting = true;
                        return;
                }
            }
        }
    }
}
