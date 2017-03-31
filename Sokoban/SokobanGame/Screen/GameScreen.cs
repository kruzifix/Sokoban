using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SokobanGame.Tiled;
using System;

namespace SokobanGame.Screen
{
    public class GameScreen : Screen
    {
        private TiledMap map;
        private bool debugMode = false;

        public int Level { get; private set; }

        public GameScreen(int level)
            : base(true, true)
        {
            Level = level;
            map = Assets.Levels[level];
            map.Room.Reset();

            int width = SokobanGame.Instance.Graphics.GraphicsDevice.Viewport.Width;
            int height = SokobanGame.Instance.Graphics.GraphicsDevice.Viewport.Height;
            
            int tileSize = (int)Math.Min(width / (float)(map.Width + 1), height / (float)(map.Height + 1));
            map.SetTileSize(tileSize, tileSize);
            
            int totalMapWidth = map.Width * map.TileWidth;
            int totalMapHeight = map.Height * map.TileHeight;
            
            map.RenderOffset = new IntVec((width - totalMapWidth) / 2, (height - totalMapHeight) / 2);
        }

        public override void Draw(GameTime gameTime)
        {
            if (map.Room.IsSolved())
                SokobanGame.Instance.GraphicsDevice.Clear(Color.Green);

            map.Draw();

            if (debugMode)
                map.DrawDebug();

            if (OnTop)
                SokobanGame.Instance.DrawDebugMessage("Game Screen", new Vector2(10, 10), Color.Black);

            SokobanGame.Instance.DrawDebugMessage(string.Format("History: {0}", map.Room.Moves), new Vector2(200, 10), Color.Black);
            SokobanGame.Instance.DrawDebugMessage(string.Format("Solved: {0}", map.Room.IsSolved()), new Vector2(200, 30), Color.Black);
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
    }
}
