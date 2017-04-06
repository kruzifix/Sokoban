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

            int width = SokobanGame.Width;
            int height = SokobanGame.Height;
            
            sb.Begin();
            sb.DrawString(titleFont, title, new Vector2(width*0.5f, 120), Color.White, Align.Center);
            sb.End();

            int textPadding = 60;
            int topOffset = height / 2 - options.Length * textPadding / 2+50;
            int btnWidth = 250;

            for (int i = 0; i < options.Length; i++)
            {
                int y = topOffset + i * textPadding;
                int pa = i == selectedOption ? 10 : 0;

                sb.DrawRect(width * 0.5f, y, btnWidth + pa * 2, 40 + pa, i == selectedOption ? Color.DarkOliveGreen : Color.Gray, Align.Center);

                sb.Begin();
                sb.DrawString(font, options[i], new Vector2(width*0.5f, y), i == selectedOption ? Color.GreenYellow : Color.White, Align.Center);
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
