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
using System;

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
            float cos = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3.0);
            float cos2 = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 1.5);

            sb.Begin();
            sb.DrawString(titleFont, "Credits", new Vector2(hw, 120), Color.White, Align.Center);

            string thx = "Thank you for playing!";
            Vector2 size = font.MeasureString(thx);
            sb.DrawString(font, thx, new Vector2(hw, 250), Color.Black, cos2 * 0.1f, size * 0.5f, 1f + cos * 0.05f, SpriteEffects.None, 0);

            sb.DrawString(font, "Programming & Design", new Vector2(hw, 320), Color.White, Align.Center);
            sb.DrawString(font, "David Cukrowicz", new Vector2(hw, 370), Color.Black, Align.Center);

            sb.DrawString(font, "Sokoban Assets", new Vector2(hw, 440), Color.White, Align.Center);
            sb.DrawString(font, "Kenney.nl", new Vector2(hw, 490), Color.Black, Align.Center);

            sb.DrawString(font, "Prompts for Keyboard and Controller", new Vector2(hw, 550), Color.White, Align.Center);
            sb.DrawString(font, "opengameart.org - xelu", new Vector2(hw, 600), Color.Black, Align.Center);

            sb.DrawString(Assets.DebugFont, "This project was made as part of the bachelor's program 'MultiMediaTechnology' of the University of Applied Sciences Salzburg.",
                new Vector2(hw, height - 50), Color.Black, Align.Center);
            sb.DrawString(Assets.DebugFont, "Everything used is licensed under Creative Commons License.", new Vector2(hw, height - 25), Color.Black, Align.Center);

            sb.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Pressed("back") || InputManager.Pressed("confirm"))
            {
                ScreenManager.RemoveScreen();
            }
        }
    }
}
