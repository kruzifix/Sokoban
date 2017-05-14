// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

using Microsoft.Xna.Framework;
using SokobanGame.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame.Screen
{
    public class CreditsScreen : Screen
    {
        SpriteFont titleFont;
        SpriteFont font;

        public CreditsScreen()
            : base(true, true)
        {
            titleFont = Assets.TitleFont;
            font = Assets.TextFont;
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Colors.MenuScreenBackground);

            int width = SokobanGame.Width;
            int height = SokobanGame.Height;

            float hw = width * 0.5f;

            sb.Begin();
            sb.DrawString(titleFont, "Credits", new Vector2(hw, 120), Color.White, Align.Center);

            sb.DrawString(font, "Programming & Design", new Vector2(hw, 300), Color.White, Align.Center);
            sb.DrawString(font, "David Cukrowicz", new Vector2(hw, 350), Color.Black, Align.Center);

            sb.DrawString(font, "Assets", new Vector2(hw, 420), Color.White, Align.Center);
            sb.DrawString(font, "Kenney.nl", new Vector2(hw, 470), Color.Black, Align.Center);

            sb.DrawString(Assets.DebugFont, "This project was made as part of the bachelor's program 'MultiMediaTechnology' of the University of Applied Sciences Salzburg.",
                new Vector2(hw, height - 100), Color.Black, Align.Center);

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
