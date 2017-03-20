using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame.Screen
{
    public class MenuScreen : Screen
    {
        public MenuScreen()
            : base(true, true)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.DrawDebugMessage("Menu Screen", new Vector2(10, 10), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPress(Keys.Escape))
            {
                SokobanGame.Instance.Exit();
                return;
            }
            
            if (InputManager.Instance.KeyPress(Keys.Enter))
            {
                ScreenManager.Instance.AddScreen(new GameScreen(Assets.TestMap));
            }
        }
    }
}
