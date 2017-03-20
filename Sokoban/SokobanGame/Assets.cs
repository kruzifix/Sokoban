using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame.Tiled;

namespace SokobanGame
{
    public static class Assets
    {
        public static SpriteFont DebugFont { get; private set; }
        public static TiledMap TestMap { get; private set; }

        public static void LoadAssets(ContentManager content)
        {
            DebugFont = content.Load<SpriteFont>("debug_font");
            TestMap = content.Load<TiledMap>("sokoban");
        }
    }
}
