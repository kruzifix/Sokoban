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
    public class FinishedScreen : Screen
    {
        private int finishedLevel;
        private int moveCount;

        private SpriteFont font;

        public FinishedScreen(int finishedLevel)
            : base(false, true)
        {
            this.finishedLevel = finishedLevel;
            this.moveCount = Assets.Levels[finishedLevel].Room.Moves;

            var lvlScreen = ScreenManager.GetScreen<LevelSelectScreen>();
            lvlScreen.UnlockedLevel = Math.Max(lvlScreen.UnlockedLevel, (finishedLevel + 1) % Assets.Levels.Length);

            font = Assets.TextFont;
        }

        public override void Draw(GameTime gameTime)
        {
            int width = SokobanGame.Width;
            int height = SokobanGame.Height;
            sb.DrawRect(width * 0.5f, height * 0.5f, 530, 230, Color.Black, Align.Center);
            sb.DrawRect(width * 0.5f, height * 0.5f, 520, 220, Colors.BoxBorderSel, Align.Center);
            sb.DrawRect(width * 0.5f, height * 0.5f, 500, 200, Colors.BoxBack, Align.Center);
            sb.DrawRect(width * 0.5f, height * 0.5f - 75, 500, 50, Colors.BoxTextBackSel, Align.Center);

            string msg = "Level finished!";
            sb.Begin();
            sb.DrawString(font, msg, new Vector2(width * 0.5f, height * 0.5f - 75), Colors.BoxTextSel, Align.Center);
            sb.DrawString(font, string.Format("Score: {0} Moves", moveCount), new Vector2(width * 0.5f, height * 0.5f - 25), Colors.BoxText, Align.Center);

            var lr = sb.DrawString(font, "Next Level", new Vector2(width * 0.5f, height * 0.5f + 45), Colors.BoxText, Align.Center);
            int s = 80;
            Rectangle r = new Rectangle(lr.X - s - 8, lr.Y + (lr.Height - s) / 2, s, s);

            sb.Draw(Assets.Keys, r, Assets.SrcEnter, Color.White);
            r.X = lr.Right;
            sb.Draw(Assets.PadBtns, r, Assets.SrcA, Color.White);
            sb.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Pressed("back"))
            {
                // HACK
                // TODO: add function to screenmanager to remove screens until specific is found
                ScreenManager.RemoveScreen(); // finished
                ScreenManager.RemoveScreen(); // gamescreen
                (ScreenManager.TopScreen as LevelSelectScreen).SelectedLevel = finishedLevel;
            }

            if (InputManager.Pressed("confirm"))
            {
                ScreenManager.RemoveScreen(); // finished
                ScreenManager.RemoveScreen(); // gamescreen
                int newLevel = (finishedLevel + 1) % Assets.Levels.Length;
                (ScreenManager.TopScreen as LevelSelectScreen).SelectedLevel = newLevel;
                ScreenManager.AddScreen(new GameScreen(newLevel));
            }

            if (InputManager.Pressed("reset"))
            {
                ScreenManager.RemoveScreen();
                (ScreenManager.TopScreen as GameScreen).Reset();
            }

            if (InputManager.Pressed("undo"))
            {
                ScreenManager.RemoveScreen();
                (ScreenManager.TopScreen as GameScreen).Undo();
            }
        }
    }
}
