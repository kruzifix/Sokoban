using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame.Screen
{
    public class LevelSelectScreen : Screen
    {
        public LevelSelectScreen()
            : base(true, true)
        { }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.DrawDebugMessage("Level Select Screen", new Vector2(10, 10), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPress(Keys.Escape))
            {
                ScreenManager.Instance.RemoveScreen();
            }
        }
    }
}
