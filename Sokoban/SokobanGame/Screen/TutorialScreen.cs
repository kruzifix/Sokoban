using Microsoft.Xna.Framework;
using SokobanGame.Input;

namespace SokobanGame.Screen
{
    public class TutorialScreen : Screen
    {
        public TutorialScreen()
            : base(true, true)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Colors.TutorialScreenBackground);

            float hw = SokobanGame.Width * 0.5f;
            sb.Begin();
            sb.DrawString(Assets.TitleFont, "How to play", new Vector2(hw, 120), Color.White, Align.Center);
            sb.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Pressed("back"))
            {
                ScreenManager.RemoveScreen();
            }
        }
    }
}
