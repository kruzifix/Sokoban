using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame
{
    public enum Align
    {
        TopLeft,
        MidLeft,
        Center,
        MidRight,
        TopMid,
        BotMid
    }

    public static class Utility
    {
        public static void DrawRect(this SpriteBatch sb, float x, float y, float width, float height, Color color, Align align = Align.TopLeft)
        {
            DrawRect(sb, (int)x, (int)y, (int)width, (int)height, color, align);
        }

        public static void DrawRect(this SpriteBatch sb, int x, int y, int width, int height, Color color, Align align = Align.TopLeft)
        {
            DrawRect(sb, new Rectangle(x, y, width, height), color, align);
        }

        public static void DrawRect(this SpriteBatch sb, Rectangle rect, Color color, Align align = Align.TopLeft)
        {
            switch (align)
            {
                case Align.Center:
                    rect.X -= rect.Width / 2;
                    rect.Y -= rect.Height / 2;
                    break;
                case Align.MidLeft:
                    rect.Y -= rect.Height / 2;
                    break;
                case Align.MidRight:
                    rect.X -= rect.Width;
                    rect.Y -= rect.Height / 2;
                    break;
                case Align.TopMid:
                    rect.X -= rect.Width / 2;
                    break;
                case Align.BotMid:
                    rect.X -= rect.Width / 2;
                    rect.Y -= rect.Height;
                    break;
            }

            sb.Begin();
            sb.Draw(Assets.PixelTexture, rect, color);
            sb.End();
        }

        public static void DrawString(this SpriteBatch sb, SpriteFont font, string txt, Vector2 position, Color color, Align align)
        {
            Vector2 txtSize = font.MeasureString(txt);
            Vector2 pos = position;
            switch (align)
            {
                case Align.MidLeft:
                    pos.Y -= txtSize.Y * 0.5f;
                    break;
                case Align.Center:
                    pos.X -= txtSize.X * 0.5f;
                    pos.Y -= txtSize.Y * 0.5f;
                    break;
                case Align.MidRight:
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
