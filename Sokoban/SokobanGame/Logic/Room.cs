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
                    if (!TryMoveEntity(ent, dir))
                        return;
                if (ent is StickyBox)
                    if (!TryMoveStickyBox(ent as StickyBox, dir))
                        return;
            }
            
            CurrentState.PlayerPosition = pos;

            history.Push(oldState);
        }
        
        private bool TryMoveEntity(Entity ent, IntVec dir)
        {
            IntVec pos = ent.Pos + dir;
            if (GetWall(pos) > 0)
                return false;
            if (CurrentState.EntityAt(pos) != null)
                return false;
            ent.Pos = pos;
            return true;
        }

        private bool TryMoveStickyBox(StickyBox box, IntVec dir)
        {
            // get all connected sticky boxes
            // TODO: recursively find all sticky boxes!
            var north = CurrentState.EntityAt(box.Pos + IntVec.Up);
            var south= CurrentState.EntityAt(box.Pos + IntVec.Down);
            var west = CurrentState.EntityAt(box.Pos + IntVec.Left);
            var east = CurrentState.EntityAt(box.Pos + IntVec.Right);

            List<StickyBox> others = new List<StickyBox>();
            if (north != null && north is StickyBox)
                others.Add(north as StickyBox);
            if (south != null && south is StickyBox)
                others.Add(south as StickyBox);
            if (west != null && west is StickyBox)
                others.Add(west as StickyBox);
            if (east != null && east is StickyBox)
                others.Add(east as StickyBox);

            // sort by movement direction
            others.Sort(delegate (StickyBox b1, StickyBox b2) {
                // TODO: add comparisons for different movement dirs
                return 0;
            });

            bool canMove = TryMoveEntity(box, dir);
            if (!canMove)
                return false;

            // try to move each box
            foreach (var sbox in others)
                TryMoveEntity(sbox, dir);

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
