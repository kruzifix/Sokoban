using Microsoft.Xna.Framework;

namespace SokobanGame.Animation
{
    public class MoveAnimation
    {
        public IntVec Start { get; private set; }
        public IntVec Target { get; private set; }

        public Vector2 Position { get; private set; }

        public bool Finished { get; private set; }

        public float Length { get; private set; }
        public float Time { get; private set; }

        public MovementDir MoveDir { get; set; }
        public int TileId { get; private set; }

        private float animTime = 0f;
        private int animId = 0;

        public MoveAnimation(IntVec start, IntVec target, float length)
        {
            Start = start;
            Target = target;
            Position = start.ToVector2();
            Finished = false;
            Length = length;
            Time = 0f;
            MoveDir = MovementDir.Down;
        }

        public void Update(GameTime gameTime)
        {
            int[] tiles = { 24, 8, 32, 16 };
            
            Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Finished = Time >= Length;

            float k = MathHelper.Clamp(Time / Length, 0f, 1f);
            Position = Vector2.Lerp(Start.ToVector2(), Target.ToVector2(), k);

            if (Finished)
            {
                TileId = tiles[(int)MoveDir];
                return;
            }
            
            animTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            float frameTime = 0.125f / 5;
            if (animTime >= frameTime)
            {
                animTime -= frameTime;
                animId++;
                if (animId > 3)
                    animId = 0;
            }

            int[] anim = { 0, 1, 0, 2 };
            TileId = tiles[(int)MoveDir] + anim[animId];
        }
    }
}
