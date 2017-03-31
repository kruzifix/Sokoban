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
                if (ent is Box && !TryMoveEntity(ent, dir))
                    return;
                if (ent is StickyBox && !TryMoveStickyBox(ent as StickyBox, dir))
                    return;
            }
            
            CurrentState.PlayerPosition = pos;

            history.Push(oldState);
        }

        private bool CanMoveEntity(Entity ent, IntVec dir)
        {
            IntVec pos = ent.Pos + dir;
            if (GetWall(pos) > 0)
                return false;
            return CurrentState.EntityAt(pos) == null;
        }
        
        private bool TryMoveEntity(Entity ent, IntVec dir)
        {
            if (!CanMoveEntity(ent, dir))
                return false;
            ent.Pos += dir;
            return true;
        }

        private bool TryMoveStickyBox(StickyBox box, IntVec dir)
        {
            HashSet<StickyBox> visited = new HashSet<StickyBox>();

            return MoveStickyBoxes(visited, box, dir);
        }

        private bool MoveStickyBoxes(HashSet<StickyBox> visited, StickyBox box, IntVec dir)
        {
            IntVec ortho = new IntVec(-dir.Y, dir.X);

            // all connected sticky boxes, in row/column of move direction
            List<StickyBox> group = new List<StickyBox>();
            // all boxes to the side of row/column, connected to group
            List<StickyBox> branches = new List<StickyBox>();

            // move backwards
            var nextBox = CurrentState.EntityAt<StickyBox>(box.Pos - dir);
            while (nextBox != null)
            {
                var left = CurrentState.EntityAt<StickyBox>(nextBox.Pos - ortho);
                if (left != null && !visited.Contains(left))
                    branches.Add(left);
                var right = CurrentState.EntityAt<StickyBox>(nextBox.Pos + ortho);
                if (right != null)
                    branches.Add(right);

                if (visited.Contains(nextBox))
                    break;
                group.Add(nextBox);
                visited.Add(nextBox);
                nextBox = CurrentState.EntityAt<StickyBox>(nextBox.Pos - dir);
            }
            // move forwards
            nextBox = CurrentState.EntityAt<StickyBox>(box.Pos);
            while (nextBox != null)
            {
                var left = CurrentState.EntityAt<StickyBox>(nextBox.Pos - ortho);
                if (left != null && !visited.Contains(left))
                    branches.Add(left);
                var right = CurrentState.EntityAt<StickyBox>(nextBox.Pos + ortho);
                if (right != null)
                    branches.Add(right);

                if (visited.Contains(nextBox))
                    break;
                group.Add(nextBox);
                visited.Add(nextBox);
                nextBox = CurrentState.EntityAt<StickyBox>(nextBox.Pos + dir);
            }

            bool firstBox = visited.Count == 0;
            // can only move 1 wide groups!
            if (firstBox && group.Count > 1)
                return false;

            StickyBox farthest = group[group.Count - 1];
            if (!CanMoveEntity(farthest, dir))
                return false;

            // move all boxes in group
            foreach (var s in group)
                s.Pos += dir;

            // execute all branches
            foreach (var s in branches)
            {
                // avoid duplicate movement
                if (visited.Contains(s))
                    continue;
                MoveStickyBoxes(visited, s, dir);
            }

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
