﻿using Microsoft.Xna.Framework;

namespace SokobanGame.Screen
{
    public class LevelSelectScreen : Screen
    {
        public LevelSelectScreen()
            : base(true, true)
        { }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.DrawDebugMessage("Level Select Screen", new Vector2(10, 10), Color.Black);
            SokobanGame.Instance.DrawDebugMessage(string.Format("GameTime: {0}", gameTime.TotalGameTime), new Vector2(10, 30), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
