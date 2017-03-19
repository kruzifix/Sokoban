using System;
using Microsoft.Xna.Framework;

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
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
