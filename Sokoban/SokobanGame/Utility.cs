using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame
{
    public enum TAlign
    {
        TopLeft,
        MidLeft,
        Center,
        MidRight
    }

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

        public static void DrawString(this SpriteBatch sb, SpriteFont font, string txt, Vector2 position, Color color, TAlign align)
        {
            Vector2 txtSize = font.MeasureString(txt);
            Vector2 pos = position;
            switch (align)
            {
                case TAlign.MidLeft:
                    pos.Y -= txtSize.Y * 0.5f;
                    break;
                case TAlign.Center:
                    pos.X -= txtSize.X * 0.5f;
                    pos.Y -= txtSize.Y * 0.5f;
                    break;
                case TAlign.MidRight:
                    pos.X -= txtSize.X;
                    pos.Y -= txtSize.Y * 0.5f;
                    break;
            }
            pos.Round();
            sb.DrawString(font, txt, pos, color);
        }

        public static void Round(this Vector2 vec)
        {
            vec.X = (int)vec.X;
            vec.Y = (int)vec.Y;
        }
    }
}
