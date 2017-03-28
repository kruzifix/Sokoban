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

        private SpriteFont font;
        private SpriteBatch sb;

        public MenuScreen()
            : base(true, true)
        {
            selectedOption = 0;

            font = Assets.DebugFont;
            sb = SokobanGame.Instance.SpriteBatch;
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.DrawDebugMessage("Menu Screen", new Vector2(10, 10), Color.Black);

            sb.Begin();

            int topOffset = 180;
            int textPadding = 35;
            int width = sb.GraphicsDevice.Viewport.Width;
            for (int i = 0; i < options.Length; i++)
            {
                string opt = options[i];
                if (i == selectedOption)
                    opt = "--{ " + opt + " }--";

                Vector2 size = font.MeasureString(opt);
                Vector2 pos = new Vector2(width / 2 - size.X / 2, topOffset + i * textPadding);

                sb.DrawString(font, opt, pos, Color.Black);
            }

            sb.End();
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
                        ScreenManager.Instance.AddScreen(new LevelSelectScreen());
                        break;
                    case 4:
                        SokobanGame.Instance.Exit();
                        return;
                }
            }
        }
    }
}
