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
using Microsoft.Xna.Framework.Input;
using SokobanGame.Animation;
using SokobanGame.Input;
using SokobanGame.Logic;
using SokobanGame.Tiled;
using System;
using System.Collections.Generic;

namespace SokobanGame.Screen
{
    public class GameScreen : Screen
    {
        private TiledMap map;
        private bool debugMode = false;
        private Queue<IntVec> movements;
        MoveAnimation currentAnim = null;
        private Vector2 playerPos;
        private int playerTile = 8;

        private SpriteFont font;

        public int Level { get; private set; }

        private int borderSize = 10;
        private int topPad = 60;
        private int botPad = 60;

        public GameScreen(int level)
            : base(true, true)
        {
            font = Assets.TextFont;

            Level = level;
            map = Assets.Levels[level];
            map.Room.Reset();

            movements = new Queue<IntVec>();
            playerPos = map.Room.CurrentState.PlayerPosition.ToVector2();

            CalcPositions();
        }

        private void CalcPositions()
        {
            int width = SokobanGame.Width;
            int height = SokobanGame.Height - (topPad + botPad);

            int tileSize = (int)Math.Min(width / (float)(map.Width + 1), height / (float)(map.Height + 1));

            map.SetTileSize(Math.Min(tileSize, map.Tileset.TileWidth), Math.Min(tileSize, map.Tileset.TileHeight));

            map.RenderOffset = new IntVec((width - map.PixelWidth) / 2, topPad + (height - map.PixelHeight) / 2);
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Colors.GameScreenBackground);

            map.Draw();

            sb.Begin();
            var tex = map.Tileset.Texture;
            Rectangle r = new Rectangle((int)(playerPos.X * map.TileWidth + map.RenderOffset.X),
                                        (int)(playerPos.Y * map.TileHeight + map.RenderOffset.Y),
                                        map.TileWidth,
                                        map.TileHeight);
            sb.Draw(tex, r, map.Tileset.GetSourceRect(playerTile), Color.White);
            sb.End();

            if (debugMode)
                map.DrawDebug();

            int width = SokobanGame.Width;
            int height = SokobanGame.Height;

            sb.DrawRect(0, 0, width, topPad, Colors.PadBackground);
            sb.DrawRect(0, height - botPad, width, botPad, Colors.PadBackground);

            sb.DrawRect(0, topPad, width, borderSize, Colors.PadBorder);
            sb.DrawRect(0, height - botPad - borderSize, width, borderSize, Colors.PadBorder);

            string lvlName;
            if (!map.Properties.TryGetValue("Name", out lvlName))
                lvlName = "noname";

            sb.Begin();
            float midTopPad = topPad * 0.5f;
            int s = topPad / 2 + 4;

            r.X = 40;
            r.Y = (int)(midTopPad - s);
            r.Width = s * 2;
            r.Height = s * 2;

            sb.Draw(Assets.Keys, r, Assets.SrcEsc, Color.White);
            var tr = sb.DrawString(font, "Go Back", new Vector2(r.Right, midTopPad), Colors.PadText, Align.MidLeft);
            r.X = tr.Right;
            sb.Draw(Assets.PadBtns, r, Assets.SrcB, Color.White);

            sb.DrawString(font, lvlName, new Vector2(width * 0.5f, midTopPad), Colors.PadText, Align.Center);

            string moves = string.Format("Moves: {0}", map.Room.Moves);
            sb.DrawString(font, moves, new Vector2(width * 0.75f, midTopPad), Colors.PadText, Align.MidLeft);

            float midBotPad = height - botPad * 0.5f;
            s = botPad / 2 + 4;

            r.X = width / 2 - s;
            r.Y = (int)(midBotPad - s);
            r.Width = s * 2;
            r.Height = s * 2;
            sb.Draw(Assets.Keys, r, Assets.SrcR, Color.White);

            tr = sb.DrawString(font, "Reset", new Vector2(r.Right, midBotPad), Colors.PadText, Align.MidLeft);
            r.X = r.X - r.Width - 48;
            sb.Draw(Assets.PadBtns, r, Assets.SrcDPad, Color.White);
            var mr = sb.DrawString(font, "Move", new Vector2(r.X - 8, midBotPad), Colors.PadText, Align.MidRight);

            r.X = mr.X - r.Width - 4;
            sb.Draw(Assets.Keys, r, Assets.SrcUp, Color.White);
            r.X = r.X - r.Width + 12;
            sb.Draw(Assets.Keys, r, Assets.SrcRight, Color.White);
            r.X = r.X - r.Width + 12;
            sb.Draw(Assets.Keys, r, Assets.SrcDown, Color.White);
            r.X = r.X - r.Width + 12;
            sb.Draw(Assets.Keys, r, Assets.SrcLeft, Color.White);

            r.X = tr.Right;
            sb.Draw(Assets.PadBtns, r, Assets.SrcY, Color.White);

            r.X = r.Right + 48;
            sb.Draw(Assets.Keys, r, Assets.SrcZ, Color.White);
            var ur = sb.DrawString(font, "Undo", new Vector2(r.Right, midBotPad), Colors.PadText, Align.MidLeft);
            r.X = ur.Right;
            sb.Draw(Assets.PadBtns, r, Assets.SrcX, Color.White);

            sb.End();

            if (!debugMode)
                return;
            SokobanGame.Instance.DrawDebugMessage(string.Format("Solved: {0}", map.Room.IsSolved()), new Vector2(300, 60), Color.Black);
            SokobanGame.Instance.DrawDebugMessage(string.Format("Entities: {0}", map.Room.CurrentState.Entities.Count), new Vector2(SokobanGame.Width - 300, 60), Color.Black);
            int i = 0;
            foreach (Entity e in map.Room.CurrentState.Entities)
            {
                string info = string.Format("{0} => {1}", e.Pos, e.GetType().Name);
                SokobanGame.Instance.DrawDebugMessage(info, new Vector2(SokobanGame.Width - 300, 80 + i * 20), Color.Black);
                i++;
            }
            InputManager.DrawDebugThumbStick(100, 100, 200);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Pressed("back"))
            {
                ScreenManager.RemoveScreen();
            }

            if (InputManager.KeyPressed(Keys.F1))
            {
                debugMode = !debugMode;
            }

            if (InputManager.Pressed("reset"))
            {
                Reset();
            }

            if (InputManager.Pressed("undo"))
            {
                Undo();
            }

            if (currentAnim != null)
            {
                currentAnim.Update(gameTime);
                playerPos = currentAnim.Position;
                playerTile = currentAnim.TileId;
                if (currentAnim.Finished)
                {
                    currentAnim = null;

                    if (map.Room.IsSolved())
                    {
                        ScreenManager.AddScreen(new FinishedScreen(Level));
                        return;
                    }
                }
            }

            if (currentAnim == null && movements.Count > 0)
            {
                IntVec move = movements.Dequeue();
                var nextAnim = map.Room.Update(move);
                if (nextAnim != null)
                {
                    currentAnim = nextAnim;
                    currentAnim.MoveDir = move.ToMovementDir();
                }
            }

            if (InputManager.Pressed("up"))
            {
                movements.Enqueue(new IntVec(0, -1));
            }
            else if (InputManager.Pressed("down"))
            {
                movements.Enqueue(new IntVec(0, 1));
            }
            else if (InputManager.Pressed("left"))
            {
                movements.Enqueue(new IntVec(-1, 0));
            }
            else if (InputManager.Pressed("right"))
            {
                movements.Enqueue(new IntVec(1, 0));
            }
        }

        public override void Resized(int width, int height)
        {
            CalcPositions();
        }

        public void Undo()
        {
            movements.Clear();
            currentAnim = null;

            map.Room.Undo();
            playerPos = map.Room.CurrentState.PlayerPosition.ToVector2();
            playerTile = 8;
        }

        public void Reset()
        {
            movements.Clear();
            currentAnim = null;

            map.Room.Reset();
            playerPos = map.Room.CurrentState.PlayerPosition.ToVector2();
            playerTile = 8;
        }
    }
}
