using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SokobanGame.Animation;
using SokobanGame.Input;
using SokobanGame.Logic;
using SokobanGame.Tiled;
using System;

namespace SokobanGame.Screen
{
    public class GameScreen : Screen
    {
        private TiledMap map;
        private bool debugMode = false;
        private MoveAnimation currentAnim = null;
        private Vector2 playerPos;
        private int playerTile = 65;

        private SpriteFont font;

        public int Level { get; private set; }

        public GameScreen(int level)
            : base(true, true)
        {
            font = Assets.TextFont;

            Level = level;
            map = Assets.Levels[level];
            map.Room.Reset();

            playerPos = map.Room.CurrentState.PlayerPosition.ToVector2();

            CalcPositions();
        }

        private void CalcPositions()
        {
            int width = SokobanGame.Width;
            int height = SokobanGame.Height;

            int tileSize = (int)Math.Min(width / (float)(map.Width + 1), height / (float)(map.Height + 1));

            map.SetTileSize(Math.Min(tileSize, map.Tileset.TileWidth), Math.Min(tileSize, map.Tileset.TileHeight));

            int totalMapWidth = map.Width * map.TileWidth;
            int totalMapHeight = map.Height * map.TileHeight;

            map.RenderOffset = new IntVec((width - totalMapWidth) / 2, (height - totalMapHeight) / 2);
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

            Color ggray = new Color(51, 51, 51, 255);
            sb.DrawRect(0, 0, width, 50, Color.LightGray);
            sb.DrawRect(0, height - 50, width, 50, Color.LightGray);

            sb.DrawRect(0, 0, width, 40, ggray);
            sb.DrawRect(0, height - 40, width, 40, ggray);

            string lvlName;
            if (!map.Properties.TryGetValue("Name", out lvlName))
                lvlName = "noname";

            sb.Begin();
            sb.DrawString(font, lvlName, new Vector2(width * 0.5f, 20), Color.White, Align.Center);
            sb.DrawString(font, "Arrow keys to move", new Vector2(40, height - 20), Color.White, Align.MidLeft);
            sb.DrawString(font, "R to reset", new Vector2(width * 0.5f, height - 20), Color.White, SokobanGame.FullScreened ? Align.Center : Align.MidLeft);
            sb.DrawString(font, "Z to Undo", new Vector2(width - 40, height - 20), Color.White, Align.MidRight);
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
                map.Room.Reset();
                playerPos = map.Room.CurrentState.PlayerPosition.ToVector2();
            }

            if (InputManager.Pressed("undo"))
            {
                map.Room.Undo();
                playerPos = map.Room.CurrentState.PlayerPosition.ToVector2();
            }

            if (currentAnim != null)
            {
                currentAnim.Update(gameTime);
                playerPos = currentAnim.Position;
                playerTile = currentAnim.TileId;
                if (currentAnim.Finished)
                {
                    currentAnim = null;
                }
                else
                {
                    return;
                }
            }

            if (map.Room.IsSolved())
            {
                ScreenManager.AddScreen(new FinishedScreen(Level));
            }

            if (InputManager.Pressed("up"))
            {
                currentAnim = map.Room.Update(new IntVec(0, -1));
                if (currentAnim != null)
                    currentAnim.MoveDir = MovementDir.Up;
            }

            if (InputManager.Pressed("down"))
            {
                currentAnim = map.Room.Update(new IntVec(0, 1));
                if (currentAnim != null)
                    currentAnim.MoveDir = MovementDir.Down;
            }

            if (InputManager.Pressed("left"))
            {
                currentAnim = map.Room.Update(new IntVec(-1, 0));
                if (currentAnim != null)
                    currentAnim.MoveDir = MovementDir.Left;
            }

            if (InputManager.Pressed("right"))
            {
                currentAnim = map.Room.Update(new IntVec(1, 0));
                if (currentAnim != null)
                    currentAnim.MoveDir = MovementDir.Right;
            }
        }

        public override void Resized(int width, int height)
        {
            CalcPositions();
        }
    }
}
