﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SokobanGame.Logic;
using SokobanGame.Tiled;
using System;

namespace SokobanGame.Screen
{
    public class GameScreen : Screen
    {
        private TiledMap map;
        private bool debugMode = false;

        private SpriteFont font;
        private SpriteBatch sb;

        public int Level { get; private set; }

        public GameScreen(int level)
            : base(true, true)
        {
            font = Assets.SpacePortFont;
            sb = SokobanGame.Instance.SpriteBatch;

            Level = level;
            map = Assets.Levels[level];
            map.Room.Reset();

            CalcPositions();
        }

        private void CalcPositions()
        {
            int width = SokobanGame.Width;
            int height = SokobanGame.Height;

            int tileSize = (int)Math.Min(width / (float)(map.Width + 1), height / (float)(map.Height + 1));
            Console.WriteLine("Level: {0} Tilesize: {1}", Level, tileSize);
            map.SetTileSize(tileSize, tileSize);

            int totalMapWidth = map.Width * map.TileWidth;
            int totalMapHeight = map.Height * map.TileHeight;

            map.RenderOffset = new IntVec((width - totalMapWidth) / 2, (height - totalMapHeight) / 2);
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Color.LightSlateGray);
            
            map.Draw();

            if (debugMode)
                map.DrawDebug();
            
            int width = SokobanGame.Width;
            int height = SokobanGame.Height;

            string lvlName;
            if (!map.Properties.TryGetValue("Name", out lvlName))
                lvlName = "noname";

            Vector2 txtSize = font.MeasureString(lvlName);
            Vector2 txtPos = new Vector2((width - txtSize.X) * 0.5f, (40 - txtSize.Y)*0.5f);
            txtPos.Round();

            sb.DrawRect(0, 0, width, 40, Color.Black);

            sb.Begin();
            sb.DrawString(font, lvlName, txtPos, Color.White);
            sb.End();

            SokobanGame.Instance.DrawDebugMessage("Arrow keys to move", new Vector2(40, height - 80), Color.Black);
            SokobanGame.Instance.DrawDebugMessage("R to reset", new Vector2(40, height - 60), Color.Black);
            SokobanGame.Instance.DrawDebugMessage("Z to undo", new Vector2(40, height - 40), Color.Black);


            if (!debugMode)
                return;
            SokobanGame.Instance.DrawDebugMessage(string.Format("Solved: {0}", map.Room.IsSolved()), new Vector2(200, 30), Color.Black);

            SokobanGame.Instance.DrawDebugMessage(string.Format("Entities: {0}", map.Room.CurrentState.Entities.Count), new Vector2(400, 10), Color.Black);
            int i = 0;
            foreach (Entity e in map.Room.CurrentState.Entities)
            {
                string info = string.Format("{0} => {1}", e.Pos, e.GetType().Name);
                SokobanGame.Instance.DrawDebugMessage(info, new Vector2(400, 30 + i * 20), Color.Black);
                i++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (KeyPress(Keys.Escape))
            {
                ScreenManager.RemoveScreen();
            }

            if (KeyPress(Keys.F1))
            {
                debugMode = !debugMode;
            }

            if (KeyPress(Keys.R))
            {
                map.Room.Reset();
            }

            if (KeyPress(Keys.Z))
            {
                map.Room.Undo();
            }
            
            if (KeyPress(Keys.Up))
            {
                map.Room.Update(new IntVec(0, -1));
            }

            if (KeyPress(Keys.Down))
            {
                map.Room.Update(new IntVec(0, 1));
            }

            if (KeyPress(Keys.Left))
            {
                map.Room.Update(new IntVec(-1, 0));
            }

            if (KeyPress(Keys.Right))
            {
                map.Room.Update(new IntVec(1, 0));
            }

            if (map.Room.IsSolved())
            {
                ScreenManager.AddScreen(new FinishedScreen(Level));
            }
        }

        public override void Resized(int width, int height)
        {
            CalcPositions();
        }
    }
}
