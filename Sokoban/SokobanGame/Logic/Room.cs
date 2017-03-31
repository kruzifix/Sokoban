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
            List<StickyBox> others = new List<StickyBox>();
            var boxes = GetConnectedStickyBoxes(box);
            boxes.Remove(box);
            foreach (var s in boxes)
                others.Add(s);

            // sort by movement direction
            others.Sort(delegate (StickyBox b1, StickyBox b2) {
                if (dir == IntVec.Up)
                    return b1.Pos.Y - b2.Pos.Y;
                if (dir == IntVec.Down)
                    return b2.Pos.Y - b1.Pos.Y;
                if (dir == IntVec.Left)
                    return b1.Pos.X - b2.Pos.X;
                if (dir == IntVec.Right)
                    return b2.Pos.X - b1.Pos.X;
                return 0;
            });

            bool canMove = TryMoveEntity(box, dir);
            if (!canMove)
                return false;

            // try to move each box
            foreach (var sbox in others)
                // TODO: BUG!!!
                //    b           b
                //    b w   =>  b w   
                // -> b           b
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

        private HashSet<StickyBox> GetConnectedStickyBoxes(StickyBox box, HashSet<StickyBox> sboxes = null)
        {
            if (sboxes == null)
                sboxes = new HashSet<StickyBox>();

            var north = CurrentState.EntityAt(box.Pos + IntVec.Up);
            var south = CurrentState.EntityAt(box.Pos + IntVec.Down);
            var west = CurrentState.EntityAt(box.Pos + IntVec.Left);
            var east = CurrentState.EntityAt(box.Pos + IntVec.Right);

            // TODO: add direction ignore parameter (don't need to add box from before)
            if (north != null && north is StickyBox)
            {
                if (sboxes.Add(north as StickyBox))
                    GetConnectedStickyBoxes(north as StickyBox, sboxes);
            }
            if (south != null && south is StickyBox)
            {
                if (sboxes.Add(south as StickyBox))
                    GetConnectedStickyBoxes(south as StickyBox, sboxes);
            }
            if (west != null && west is StickyBox)
            {
                if (sboxes.Add(west as StickyBox))
                    GetConnectedStickyBoxes(west as StickyBox, sboxes);
            }
            if (east != null && east is StickyBox)
            {
                if (sboxes.Add(east as StickyBox))
                    GetConnectedStickyBoxes(east as StickyBox, sboxes);
            }

            return sboxes;
        }
    }
}
