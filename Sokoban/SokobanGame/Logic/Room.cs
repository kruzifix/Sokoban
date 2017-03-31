using System;
using System.Collections.Generic;

namespace SokobanGame.Logic
{
    public class Room
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private int[,] walls;
        public IntVec[] Switches { get; private set; }

        private RoomState initialState;
        public RoomState CurrentState { get; private set; }
        private Stack<RoomState> history;
        
        public int Moves { get { return history.Count; } }

        public Room(int width, int height, IntVec[] switches, RoomState initialState)
        {
            Width = width;
            Height = height;
            walls = new int[Width, Height];
            Switches = switches;

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
        
        public bool IsSolved()
        {
            List<Box> boxes = CurrentState.Boxes;

            for (int i = 0; i < Switches.Length; i++)
            {
                bool boxFound = false;

                foreach (Box b in boxes)
                {
                    if (Switches[i] == b.Pos)
                    {
                        boxFound = true;
                        break;
                    }
                }

                if (!boxFound)
                    return false;
            }

            return true;
        }

        public void Update(IntVec dir)
        {
            var oldState = CurrentState.Copy();

            IntVec pos = CurrentState.PlayerPosition + dir;
            if (GetWall(pos) > 0)
                return;

            var ent = CurrentState.EntityAt(pos);
            if (ent != null)
            {
                if (ent is Box)
                    if (!TryMoveBox(ent as Box, dir))
                        return;
                if (ent is StickyBox)
                    if (!TryMoveStickyBox(ent as StickyBox, dir))
                        return;
            }
            
            CurrentState.PlayerPosition = pos;

            history.Push(oldState);
        }
        
        private bool TryMoveBox(Box box, IntVec dir)
        {
            IntVec pos = box.Pos + dir;
            if (GetWall(pos) > 0)
                return false;
            if (CurrentState.EntityAt(pos) != null)
                return false;
            box.Pos = pos;
            return true;
        }

        private bool TryMoveStickyBox(StickyBox box, IntVec dir)
        {
            // TODO: add sticky box movement logic (currently same as normal Box)
            IntVec pos = box.Pos + dir;
            if (GetWall(pos) > 0)
                return false;
            if (CurrentState.EntityAt(pos) != null)
                return false;
            box.Pos = pos;
            return true;
        }

        public bool OnSwitch(IntVec pos)
        {
            foreach (IntVec s in Switches)
                if (s == pos)
                    return true;
            return false;
        }
    }
}
