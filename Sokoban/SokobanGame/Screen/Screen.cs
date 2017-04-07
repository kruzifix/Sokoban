using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * Reference: https://blogs.msdn.microsoft.com/etayrien/2006/12/12/basic-game-engine-structure/
 */

namespace SokobanGame.Screen
{
    public abstract class Screen
    {
        public bool BlocksDraw { get; private set; }
        public bool BlocksUpdate { get; private set; }

        public bool OnTop { get { return ScreenManager.TopScreen == this; } }

        protected SpriteBatch sb { get; private set; }

        public Screen(bool blocksDraw, bool blocksUpdate)
        {
            BlocksDraw = blocksDraw;
            BlocksUpdate = blocksUpdate;

            sb = SokobanGame.Instance.SpriteBatch;
        }
        
        public virtual void Activated() { }
        public virtual void Disabled() { }

        public virtual void Resized(int width, int height) { }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
