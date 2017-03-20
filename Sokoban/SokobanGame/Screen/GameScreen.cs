using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame.Screen
{
    public class GameScreen : Screen
    {
        public GameScreen()
            : base(true, true)
        {
            Assets.TestMap.SetTileSize(64, 64);
        }

        public override void Draw(GameTime gameTime)
        {
            Assets.TestMap.Draw();

            SokobanGame.Instance.DrawDebugMessage("Game Screen", new Vector2(10, 10), Color.Black);
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
