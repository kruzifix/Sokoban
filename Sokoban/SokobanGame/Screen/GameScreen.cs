﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SokobanGame.Tiled;

namespace SokobanGame.Screen
{
    public class GameScreen : Screen
    {
        TiledMap map;

        public GameScreen(TiledMap map)
            : base(true, true)
        {
            this.map = map;
            map.SetTileSize(64, 64);
        }

        public override void Draw(GameTime gameTime)
        {
            map.Draw();

            SokobanGame.Instance.DrawDebugMessage("Game Screen", new Vector2(10, 10), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPress(Keys.Escape))
            {
                ScreenManager.Instance.RemoveScreen();
            }

            if (InputManager.Instance.KeyPress(Keys.R))
            {
                map.Room.Reset();
            }

            if (InputManager.Instance.KeyPress(Keys.Z))
            {
                map.Room.Undo();
            }

            if (InputManager.Instance.KeyPress(Keys.Up))
            {
                map.Room.Update(new IntVec(0, -1));
            }

            if (InputManager.Instance.KeyPress(Keys.Down))
            {
                map.Room.Update(new IntVec(0, 1));
            }

            if (InputManager.Instance.KeyPress(Keys.Left))
            {
                map.Room.Update(new IntVec(-1, 0));
            }

            if (InputManager.Instance.KeyPress(Keys.Right))
            {
                map.Room.Update(new IntVec(1, 0));
            }
        }
    }
}
