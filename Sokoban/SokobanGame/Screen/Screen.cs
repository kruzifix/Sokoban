using Microsoft.Xna.Framework;

/*
 * Reference: https://blogs.msdn.microsoft.com/etayrien/2006/12/12/basic-game-engine-structure/
 */

namespace SokobanGame.Screen
{
    public abstract class Screen
    {
        public bool BlocksDraw { get; private set; }
        public bool BlocksUpdate { get; private set; }

        public Screen(bool blocksDraw, bool blocksUpdate)
        {
            BlocksDraw = blocksDraw;
            BlocksUpdate = blocksUpdate;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
