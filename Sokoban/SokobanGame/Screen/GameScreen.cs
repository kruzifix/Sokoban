using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SokobanGame.Animation;
using SokobanGame.Input;
using SokobanGame.Logic;
using SokobanGame.Tiled;
using System;
using System.Collections.Generic;

namespace SokobanGame.Screen
{
    public class GameScreen : Screen
    {
        private TiledMap map;
        private bool debugMode = false;
        private Queue<MoveAnimation> animations;
        MoveAnimation currentAnim = null;
        private Vector2 playerPos;
        private int playerTile = 65;

        private SpriteFont font;

        public int Level { get; private set; }

        private int borderSize = 10;
        private int topPad = 60;
        private int botPad = 60;

        public GameScreen(int level)
            : base(true, true)
        {
            font = Assets.TextFont;

            Level = level;
            map = Assets.Levels[level];
            map.Room.Reset();

            animations = new Queue<MoveAnimation>();
            playerPos = map.Room.CurrentState.PlayerPosition.ToVector2();
            
            CalcPositions();
        }

        private void CalcPositions()
        {
            int width = SokobanGame.Width;
            int height = SokobanGame.Height - (topPad + botPad);

            int tileSize = (int)Math.Min(width / (float)(map.Width + 1), height / (float)(map.Height + 1));

            map.SetTileSize(Math.Min(tileSize, map.Tileset.TileWidth), Math.Min(tileSize, map.Tileset.TileHeight));
            
            map.RenderOffset = new IntVec((width - map.PixelWidth) / 2, topPad + (height - map.PixelHeight) / 2);
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.GraphicsDevice.Clear(Color.LightSlateGray);

            map.Draw();

            sb.Begin();
            var tex = map.Tileset.Texture;
            Rectangle r = new Rectangle((int)(playerPos.X * map.TileWidth + map.RenderOffset.X),
                                        (int)(playerPos.Y * map.TileHeight + map.RenderOffset.Y),
                                        map.TileWidth,
                                        map.TileHeight);
            sb.Draw(tex, r, map.Tileset.GetSourceRect(playerTile), Color.White);
            sb.End();

            if (debugMode)
                map.DrawDebug();

            int width = SokobanGame.Width;
            int height = SokobanGame.Height;
            
            sb.DrawRect(0, 0, width, topPad, Colors.PadBackground);
            sb.DrawRect(0, height - botPad, width, botPad, Colors.PadBackground);

            sb.DrawRect(0, topPad, width, borderSize, Colors.PadBorder);
            sb.DrawRect(0, height - botPad - borderSize, width, borderSize, Colors.PadBorder);

            string lvlName;
            if (!map.Properties.TryGetValue("Name", out lvlName))
                lvlName = "noname";

            sb.Begin();
            float midTopPad = topPad * 0.5f;
            int s = topPad / 2 + 4;

            r.X = 40;
            r.Y = (int)(midTopPad - s);
            r.Width = s * 2;
            r.Height = s * 2;

            sb.Draw(Assets.Keys, r, Assets.SrcEsc, Color.White);
            sb.DrawString(font, "Go Back", new Vector2(r.Right, midTopPad), Colors.PadText, Align.MidLeft);

            sb.DrawString(font, lvlName, new Vector2(width * 0.5f, midTopPad), Colors.PadText, Align.Center);

            float midBotPad = height - botPad * 0.5f;
            s = botPad / 2 + 4;
            
            r.X = width / 2;
            r.Y = (int)(midBotPad - s);
            r.Width = s * 2;
            r.Height = s * 2;
            sb.Draw(Assets.Keys, r, Assets.SrcR, Color.White);
            var tr = sb.DrawString(font, "Reset", new Vector2(r.Right, midBotPad), Colors.PadText, Align.MidLeft);
            var mr = sb.DrawString(font, "Move", new Vector2(r.X - 24, midBotPad), Colors.PadText, Align.MidRight);

            r.X = mr.X - r.Width;
            sb.Draw(Assets.Keys, r, Assets.SrcUp, Color.White);
            r.X = r.X - r.Width + 12;
            sb.Draw(Assets.Keys, r, Assets.SrcRight, Color.White);
            r.X = r.X - r.Width + 12;
            sb.Draw(Assets.Keys, r, Assets.SrcDown, Color.White);
            r.X = r.X - r.Width + 12;
            sb.Draw(Assets.Keys, r, Assets.SrcLeft, Color.White);

            r.X = tr.Right + 24;
            sb.Draw(Assets.Keys, r, Assets.SrcZ, Color.White);
            sb.DrawString(font, "Undo", new Vector2(r.Right, midBotPad), Colors.PadText, Align.MidLeft);
            sb.End();

            if (!debugMode)
                return;
            SokobanGame.Instance.DrawDebugMessage(string.Format("Solved: {0}", map.Room.IsSolved()), new Vector2(300, 60), Color.Black);
            SokobanGame.Instance.DrawDebugMessage(string.Format("Entities: {0}", map.Room.CurrentState.Entities.Count), new Vector2(SokobanGame.Width - 300, 60), Color.Black);
            int i = 0;
            foreach (Entity e in map.Room.CurrentState.Entities)
            {
                string info = string.Format("{0} => {1}", e.Pos, e.GetType().Name);
                SokobanGame.Instance.DrawDebugMessage(info, new Vector2(SokobanGame.Width - 300, 80 + i * 20), Color.Black);
                i++;
            }
            InputManager.DrawDebugThumbStick(100, 100, 200);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Pressed("back"))
            {
                ScreenManager.RemoveScreen();
            }

            if (InputManager.KeyPressed(Keys.F1))
            {
                debugMode = !debugMode;
            }

            if (InputManager.Pressed("reset"))
            {
                Reset();
            }

            if (InputManager.Pressed("undo"))
            {
                Undo();
            }

            if (currentAnim != null)
            {
                currentAnim.Update(gameTime);
                playerPos = currentAnim.Position;
                playerTile = currentAnim.TileId;
                if (currentAnim.Finished)
                {
                    currentAnim = null;

                    if (animations.Count == 0 && map.Room.IsSolved())
                    {
                        ScreenManager.AddScreen(new FinishedScreen(Level));
                    }
                }
            }
            if (currentAnim == null && animations.Count > 0)
                currentAnim = animations.Dequeue();

            if (InputManager.Pressed("up"))
            {
                var anim = map.Room.Update(new IntVec(0, -1));
                if (anim != null)
                {
                    anim.MoveDir = MovementDir.Up;
                    animations.Enqueue(anim);
                }
            }
            else if (InputManager.Pressed("down"))
            {
                var anim = map.Room.Update(new IntVec(0, 1));
                if (anim != null)
                {
                    anim.MoveDir = MovementDir.Down;
                    animations.Enqueue(anim);
                }
            }
            else if (InputManager.Pressed("left"))
            {
                var anim = map.Room.Update(new IntVec(-1, 0));
                if (anim != null)
                {
                    anim.MoveDir = MovementDir.Left;
                    animations.Enqueue(anim);
                }
            }
            else if (InputManager.Pressed("right"))
            {
                var anim = map.Room.Update(new IntVec(1, 0));
                if (anim != null)
                {
                    anim.MoveDir = MovementDir.Right;
                    animations.Enqueue(anim);
                }
            }
        }

        public override void Resized(int width, int height)
        {
            CalcPositions();
        }

        public void Undo()
        {
            animations.Clear();
            currentAnim = null;

            map.Room.Undo();
            playerPos = map.Room.CurrentState.PlayerPosition.ToVector2();
        }

        public void Reset()
        {
            animations.Clear();
            currentAnim = null;

            map.Room.Reset();
            playerPos = map.Room.CurrentState.PlayerPosition.ToVector2();
        }
    }
}
