using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame.Tiled;
using Microsoft.Xna.Framework;

namespace SokobanGame
{
    public static class Assets
    {
        public static SpriteFont DebugFont { get; private set; }
        public static SpriteFont TextFont { get; private set; }
        public static SpriteFont TitleFont { get; private set; }

        public static Texture2D PixelTexture { get; private set; }

        public static Texture2D Keys { get; private set; }
        public static Rectangle SrcUp { get; private set; }
        public static Rectangle SrcRight { get; private set; }
        public static Rectangle SrcDown { get; private set; }
        public static Rectangle SrcLeft { get; private set; }
        public static Rectangle SrcEsc { get; private set; }
        public static Rectangle SrcR { get; private set; }
        public static Rectangle SrcZ { get; private set; }
        public static Rectangle SrcEnter { get; private set; }

        public static TiledMap[] Levels { get; private set; }

        public static void LoadAssets(ContentManager content)
        {
            DebugFont = content.Load<SpriteFont>("debug_font");
            TextFont = content.Load<SpriteFont>("text_font");
            TitleFont = content.Load<SpriteFont>("title_font");

            PixelTexture = content.Load<Texture2D>("pixel_1x1");

            Keys = content.Load<Texture2D>("keys");
            SrcUp = new Rectangle(0, 0, 100, 100);
            SrcRight = new Rectangle(100, 0, 100, 100);
            SrcDown = new Rectangle(200, 0, 100, 100);
            SrcLeft = new Rectangle(300, 0, 100, 100);
            SrcEsc = new Rectangle(400, 0, 100, 100);
            SrcR = new Rectangle(500, 0, 100, 100);
            SrcZ = new Rectangle(600, 0, 100, 100);
            SrcEnter = new Rectangle(700, 0, 100, 100);

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
