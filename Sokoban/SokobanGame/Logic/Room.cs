using System;
using System.Collections.Generic;

namespace SokobanGame.Logic
{
    public class Room
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private int[,] walls;

        private RoomState initialState;
        public RoomState CurrentState { get; private set; }
        private Stack<RoomState> history;

        public int Moves { get { return history.Count; } }

        public Room(int width, int height, RoomState initialState)
        {
            Width = width;
            Height = height;
            walls = new int[Width, Height];

            this.initialState = initialState.Copy();
            history = new Stack<RoomState>();
            Reset();
        }

        public void SetWall(int x, int y, int v)
        {
            walls[x, y] = v;
        }

        public int GetWall(int x, int y)
        {
            return walls[x, y];
        }

        public int GetWall(IntVec v)
        {
            return GetWall(v.X, v.Y);
        }

        public void Reset()
        {
            history.Clear();
            CurrentState = initialState.Copy();
        }

        public void Undo()
        {
            if (history.Count > 0)
                CurrentState = history.Pop();
        }

        public void Update(IntVec dir)
        {
            var oldState = CurrentState.Copy();

            IntVec pos = CurrentState.PlayerPosition + dir;
            if (GetWall(pos) > 0)
                return;

            int box = CurrentState.BoxAt(pos);
            if (box >= 0)
            {
                if (!TryMoveBox(box, dir))
                    return;
            }
            
            CurrentState.PlayerPosition = pos;

            history.Push(oldState);
        }

        private bool TryMoveBox(int box, IntVec dir)
        {
            IntVec pos = CurrentState.Boxes[box] + dir;
            if (GetWall(pos) > 0)
                return false;
            if (CurrentState.BoxAt(pos) >= 0)
                return false;
            CurrentState.Boxes[box] = pos;
            return true;
        }
    }
}
