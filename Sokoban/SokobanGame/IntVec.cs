﻿namespace SokobanGame
{
    public struct IntVec
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsZero { get { return X == 0 && Y == 0; } }

        public IntVec(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public void Set(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is IntVec)
            {
                IntVec p = (IntVec)obj;
                return X == p.X && Y == p.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        #region Operators

        public static IntVec operator +(IntVec p, IntVec v)
        {
            return new IntVec(p.X + v.X, p.Y + v.Y);
        }

        public static IntVec operator -(IntVec p, IntVec v)
        {
            return new IntVec(p.X - v.X, p.Y - v.Y);
        }

        public static bool operator ==(IntVec p, IntVec v)
        {
            return p.Equals(v);
        }

        public static bool operator !=(IntVec p, IntVec v)
        {
            return !p.Equals(v);
        }

        #endregion
    }
}