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
    }
}
