using System;
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
            SokobanGame.Instance.DrawDebugMessage("MenuScreen", new Vector2(10, 10), Color.Black);
            SokobanGame.Instance.DrawDebugMessage(string.Format("GameTime: {0}", gameTime.TotalGameTime), new Vector2(10, 30), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine("MenuScreen.Update()");

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SokobanGame.Instance.Exit();
                return;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                ScreenManager.Instance.AddScreen(new LevelSelectScreen());
            }
        }
    }
}
