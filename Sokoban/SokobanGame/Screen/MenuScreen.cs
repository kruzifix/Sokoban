using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private SpriteBatch sb;

        public MenuScreen()
            : base(true, true)
        {
            selectedOption = 0;

            titleFont = Assets.GreenwichFont;
            font = Assets.SpacePortFont;
            sb = SokobanGame.Instance.SpriteBatch;
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Color.LightSlateGray);

            int width = SokobanGame.Instance.GraphicsDevice.Viewport.Width;
            int height = SokobanGame.Instance.GraphicsDevice.Viewport.Height;

            Vector2 titleSize = titleFont.MeasureString(title);
            Vector2 titlePos = new Vector2((width - titleSize.X) * 0.5f, 50);
            titlePos.Round();

            sb.Begin();
            sb.DrawString(titleFont, title, titlePos, Color.White);
            sb.End();

            int textPadding = 60;
            int topOffset = height / 2 - options.Length * textPadding / 2;
            int btnWidth = 220;

            for (int i = 0; i < options.Length; i++)
            {
                string opt = options[i];

                Vector2 txtSize = font.MeasureString(opt);
                Vector2 txtPos = new Vector2((width - txtSize.X) * 0.5f, topOffset + i * textPadding);
                txtPos.Round();

                int pa = i == selectedOption ? 5 : 0;

                sb.DrawRect((width - btnWidth) * 0.5f - pa, txtPos.Y - 10 - pa, btnWidth + pa * 2, txtSize.Y + 20 + pa * 2, i == selectedOption ? Color.DarkOliveGreen : Color.Gray);

                sb.Begin();
                sb.DrawString(font, opt, txtPos, i == selectedOption ? Color.GreenYellow : Color.White);
                sb.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPress(Keys.Escape))
            {
                SokobanGame.Instance.Exit();
                return;
            }

            if (InputManager.Instance.KeyPress(Keys.Down))
            {
                selectedOption = (selectedOption + 1) % options.Length;
            }

            if (InputManager.Instance.KeyPress(Keys.Up))
            {
                selectedOption--;
                if (selectedOption < 0)
                    selectedOption = options.Length - 1;
            }

            if (InputManager.Instance.KeyPress(Keys.Enter))
            {
                switch (selectedOption)
                {
                    case 0:
                        ScreenManager.AddScreen(new LevelSelectScreen());
                        break;
                    case 4:
                        SokobanGame.Instance.Exit();
                        return;
                }
            }
        }
    }
}
