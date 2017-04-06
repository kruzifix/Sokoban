using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame.Tiled;

namespace SokobanGame
{
    public static class Assets
    {
        public static SpriteFont DebugFont { get; private set; }
        public static SpriteFont SpacePortFont { get; private set; }

        public static Texture2D PixelTexture { get; private set; }

        public static TiledMap[] Levels { get; private set; }

        public static void LoadAssets(ContentManager content)
        {
            DebugFont = content.Load<SpriteFont>("debug_font");
            SpacePortFont = content.Load<SpriteFont>("spaceport_font");

            PixelTexture = content.Load<Texture2D>("pixel_1x1");

            string levelsPath = "Levels";
            DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "/" + levelsPath);
            if (!dir.Exists)
                return;

            FileInfo[] files = dir.GetFiles("*.*");
            Levels = new TiledMap[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                string key = Path.GetFileNameWithoutExtension(files[i].Name);
                Levels[i] = content.Load<TiledMap>(levelsPath + "/" + key);
            }
        }
    }
}
