// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame.Input;
using System;

namespace SokobanGame.Screen
{
    public class MenuScreen : Screen
    {
        private int selectedOption;
        private string[] options = {
            "Play!",
            "Credits",
            "Exit"
        };

        private string title = "Sokoban";

        private SpriteFont titleFont;
        private SpriteFont font;

        float animProg = 0f;
        bool exiting = false;

        public MenuScreen()
            : base(true, true)
        {
            selectedOption = 0;

            titleFont = Assets.TitleFont;
            font = Assets.TextFont;
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Colors.MenuScreenBackground);

            int width = SokobanGame.Width;
            int height = SokobanGame.Height;

            float k = MathHelper.Clamp(animProg * animProg * (3 - 2 * animProg), 0f, 1f);

            float cos = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3.5);
            float op = 20f + 8f * cos;

            sb.Begin();
            sb.DrawString(titleFont, title, new Vector2(width * 0.5f, -120 + 240 * k), Color.White, Align.Center);
            sb.DrawString(Assets.DebugFont, "Toggle Fullscreen with F10", new Vector2(20, height + 20 - 40 * k), Color.Black, Align.MidLeft);
            sb.DrawString(Assets.DebugFont, "A Game made by David Cukrowicz", new Vector2(width - 20, height + 20 - 40 * k), Color.Black, Align.MidRight);
            sb.End();

            int textPadding = 70;
            int topOffset = height / 2 - options.Length * textPadding / 2 + 50;
            int btnWidth = 250;

            for (int i = 0; i < options.Length; i++)
            {
                float y = topOffset + i * textPadding;
                int pa = i == selectedOption ? 10 : 0;

                float x = width * 0.5f;
                y += (1 - k) * (height * 0.5f + i * 80);

                Color bBorder = i == selectedOption ? Colors.BtnBorderSel : Colors.BtnBorder;
                bBorder *= k;
                Color bBackg = i == selectedOption ? Colors.BtnBackSel : Colors.BtnBack;
                bBackg *= k;

                float bw = i == selectedOption ? btnWidth + op : btnWidth;

                sb.DrawRect(x, y, bw + pa * 2 + 10, 50 + pa, bBorder, Align.Center);
                sb.DrawRect(x, y, bw + pa * 2, 40 + pa, bBackg, Align.Center);

                sb.Begin();
                Vector2 size = font.MeasureString(options[i]);
                float scale = 1f;
                if (i == selectedOption)
                    scale = 1.12f - 0.03f * cos;
                Color txtCol = i == selectedOption ? Colors.BtnTextSel : Colors.BtnText;
                sb.DrawString(font, options[i], new Vector2(x, y), txtCol,
                              0f, size * 0.5f, scale, SpriteEffects.None, 0);
                sb.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (exiting)
            {
                animProg -= time * 2f;
                if (animProg <= 0f)
                    SokobanGame.Instance.Exit();
            }
            else
            {
                animProg += time * 2f;
            }
            animProg = MathHelper.Clamp(animProg, 0f, 1f);

            if (InputManager.Pressed("back"))
            {
                exiting = true;
                return;
            }

            if (InputManager.Pressed("down"))
            {
                selectedOption = (selectedOption + 1) % options.Length;
            }

            if (InputManager.Pressed("up"))
            {
                selectedOption--;
                if (selectedOption < 0)
                    selectedOption = options.Length - 1;
            }

            if (InputManager.Pressed("confirm"))
            {
                switch (selectedOption)
                {
                    case 0:
                        ScreenManager.AddScreen(new LevelSelectScreen());
                        break;
                    case 1:
                        ScreenManager.AddScreen(new CreditsScreen());
                        break;
                    case 2:
                        exiting = true;
                        return;
                }
            }
        }
    }
}
