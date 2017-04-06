using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame
{
    public static class Utility
    {
        public static void DrawRect(this SpriteBatch sb, float x, float y, float width, float height, Color color)
        {
            DrawRect(sb, (int)x, (int)y, (int)width, (int)height, color);
        }

        public static void DrawRect(this SpriteBatch sb, int x, int y, int width, int height, Color color)
        {
            DrawRect(sb, new Rectangle(x, y, width, height), color);
        }

        public static void DrawRect(this SpriteBatch sb, Rectangle rect, Color color)
        {
            sb.Begin();
            sb.Draw(Assets.PixelTexture, rect, color);
            sb.End();
        }
    }
}
