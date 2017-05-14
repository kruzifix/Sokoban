using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SokobanGame.Input;

namespace SokobanGame.Screen
{
    public class CreditsScreen : Screen
    {
        public CreditsScreen()
            : base(true, true)
        { }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Colors.MenuScreenBackground);


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
