using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame.Input;
using SokobanGame.Tiled;
using System;

namespace SokobanGame.Screen
{
    public class TutorialScreen : Screen
    {
        TiledTileset set;
        Texture2D tex;

        public TutorialScreen()
            : base(true, true)
        {
            set = Assets.Levels[0].Tileset;
            tex = set.Texture;
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Colors.TutorialScreenBackground);

            float hw = SokobanGame.Width * 0.5f;

            int ts = 96;
            Rectangle dest = new Rectangle((SokobanGame.Width - ts) / 2, 250, ts, ts);

            Rectangle strip = new Rectangle(0, dest.Top - ts / 2 + 8, SokobanGame.Width, ts * 2);
            strip.Y = strip.Bottom;
            sb.DrawRect(strip, Color.DimGray);

            sb.Begin();
            sb.DrawString(Assets.TitleFont, "How to play", new Vector2(hw, 120), Color.White, Align.Center);

            sb.Draw(tex, dest, set.GetSourceRect(1), Color.White);
            sb.Draw(tex, dest, set.GetSourceRect(3), Color.White);
            sb.DrawString(Assets.TextFont, "Switch", new Vector2(hw, dest.Top - 16), Color.GreenYellow, Align.Center);

            int midX = dest.X;
            dest.X = midX - ts * 3;
            sb.Draw(tex, dest, set.GetSourceRect(11), Color.White);
            sb.DrawString(Assets.TextFont, "Box", new Vector2(dest.X + ts / 2, dest.Top - 16), Color.GreenYellow, Align.Center);

            dest.X = midX + ts * 3;
            sb.Draw(tex, dest, set.GetSourceRect(19), Color.White);
            sb.DrawString(Assets.TextFont, "Sticky Box", new Vector2(dest.X + ts / 2, dest.Top - 16), Color.GreenYellow, Align.Center);

            sb.DrawString(Assets.TextFont, "Cover all switches with boxes to finish a level!", new Vector2(hw, dest.Bottom + 30), Color.White, Align.Center);

            dest.Y += ts * 2;
            dest.X = midX - ts * 4;
            sb.Draw(tex, dest, set.GetSourceRect(35), Color.White);
            sb.DrawString(Assets.TextFont, "In", new Vector2(dest.X + ts / 2, dest.Top - 16), Color.White, Align.Center);

            dest.X = midX - ts * 2;
            sb.Draw(tex, dest, set.GetSourceRect(36), Color.White);
            sb.DrawString(Assets.TextFont, "Out", new Vector2(dest.X + ts / 2, dest.Top - 16), Color.White, Align.Center);
            dest.X = midX - ts * 3;
            sb.DrawString(Assets.TextFont, "Teleporter (only works for boxes)", new Vector2(dest.X + ts / 2, dest.Bottom + 20), Color.Black, Align.Center);

            dest.X = midX + ts * 3;
            sb.Draw(tex, dest, set.GetSourceRect(27), Color.White);
            sb.DrawString(Assets.TextFont, "Hole", new Vector2(dest.X + ts / 2, dest.Top - 16), Color.GreenYellow, Align.Center);
            sb.DrawString(Assets.TextFont, "fill with box to walk over!", new Vector2(dest.X + ts / 2, dest.Bottom + 20), Color.Black, Align.Center);

            dest.Y += ts * 2 + ts / 4;
            dest.X = midX - ts * 2;
            sb.Draw(tex, dest, set.GetSourceRect(30), Color.White);
            dest.X += ts;
            sb.Draw(tex, dest, set.GetSourceRect(5), Color.White);
            dest.X += ts;
            sb.Draw(tex, dest, set.GetSourceRect(6), Color.White);
            sb.DrawString(Assets.TextFont, "Icy Ground", new Vector2(dest.X + ts / 2, dest.Top - 22), Color.GreenYellow, Align.Center);

            dest.Y += ts;
            dest.X = midX - ts * 2;
            sb.Draw(tex, dest, set.GetSourceRect(22), Color.White);
            dest.X += ts;
            sb.Draw(tex, dest, set.GetSourceRect(29), Color.White);
            dest.X += ts;
            sb.Draw(tex, dest, set.GetSourceRect(29), Color.White);
            sb.DrawString(Assets.TextFont, "Player and Boxes slip until they hit a wall or ground", new Vector2(dest.X + ts / 2, dest.Bottom + 20), Color.White, Align.Center);
            dest.X += ts;
            sb.Draw(tex, dest, set.GetSourceRect(37), Color.White);
            dest.X += ts;
            sb.Draw(tex, dest, set.GetSourceRect(15), Color.White);

            sb.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Pressed("back"))
            {
                ScreenManager.RemoveScreen();
            }
        }
    }
}
