using Microsoft.Xna.Framework;

namespace SokobanGame.Screen
{
    public abstract class GameScreen
    {
        public bool BlocksDraw { get; private set; }
        public bool BlocksUpdate { get; private set; }

        public GameScreen(bool blocksDraw, bool blocksUpdate)
        {
            BlocksDraw = blocksDraw;
            BlocksUpdate = blocksUpdate;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
