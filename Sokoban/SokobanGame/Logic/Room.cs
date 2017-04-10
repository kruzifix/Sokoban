using System.Collections.Generic;

namespace SokobanGame.Logic
{
    public class Room
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private FieldObject[,] field;
        public IntVec[] Switches { get; private set; }
        public Teleporter[] Teleporters { get; private set; }

        private RoomState initialState;
        public RoomState CurrentState { get; private set; }
        private Stack<RoomState> history;
        
        public int Moves { get { return history.Count; } }

        public Room(int width, int height, IntVec[] switches, Teleporter[] teleporters, RoomState initialState)
        {
            Width = width;
            Height = height;
            field = new FieldObject[Width, Height];
            Switches = switches;
            Teleporters = teleporters;

            this.initialState = initialState.Copy();
            history = new Stack<RoomState>();
            Reset();
        }

        public void SetObject(int x, int y, FieldObject v)
        {
            field[x, y] = v;
        }

        public FieldObject GetObject(int x, int y)
        {
            return field[x, y];
        }

        public FieldObject GetObject(IntVec v)
        {
            return GetObject(v.X, v.Y);
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
            List<Entity> boxes = new List<Entity>();
            boxes.AddRange(CurrentState.Boxes);
            boxes.AddRange(CurrentState.StickyBoxes);

            for (int i = 0; i < Switches.Length; i++)
            {
                bool boxFound = false;

                foreach (Entity b in boxes)
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
            if (GetObject(pos) == FieldObject.Wall)
                return;

            var ent = CurrentState.EntityAt(pos);
            if (ent != null)
            {
                if (ent is Box && !TryMoveBox(ent as Box, dir))
                    return;
                if (ent is StickyBox && !TryMoveStickyBox(ent as StickyBox, dir))
                    return;
                if (ent is Hole)
                {
                    Hole h = ent as Hole;
                    if (!h.Filled)
                        return;
                }
            } else
            {
                while (GetObject(pos) == FieldObject.IceGround)
                {
                    var next = CurrentState.EntityAt(pos + dir);
                    if (next != null)
                    {
                        if (!(next is Hole) || !(next as Hole).Filled)
                            break;
                    }
                    pos += dir;
                }
            }
            
            CurrentState.PlayerPosition = pos;

            history.Push(oldState);
        }

        private bool CanMoveEntity(Entity ent, IntVec dir)
        {
            IntVec pos = ent.Pos + dir;
            if (GetObject(pos) == FieldObject.Wall)
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

        private bool TryMoveBox(Box box, IntVec dir)
        {
            IntVec pos = box.Pos + dir;
            if (GetObject(pos) == FieldObject.Wall)
                return false;
            var ent = CurrentState.EntityAt(pos);
            if (ent == null)
            {
                var tele = TeleporterAt(pos);
                if (tele != null)
                {
                    // teleporter blocked?
                    if (CurrentState.EntityAt(tele.Target) == null)
                        pos = tele.Target;
                }

                if (GetObject(pos) == FieldObject.IceGround)
                {
                    box.Pos = pos;
                    TryMoveBox(box, dir);
                    return true;
                }

                box.Pos = pos;
                return true;
            }
            else if (ent is Hole)
            {
                Hole h = ent as Hole;
                if (h.Filled)
                {
                    box.Pos = pos;
                }
                else
                {
                    h.Filled = true;
                    CurrentState.RemoveEntity(box);
                }
                return true;
            }
            return false;
        }

        private bool TryMoveStickyBox(StickyBox box, IntVec dir)
        {
            HashSet<StickyBox> visited = new HashSet<StickyBox>();

            return MoveStickyBoxes(visited, box, dir);
        }

        private bool MoveStickyBoxes(HashSet<StickyBox> visited, StickyBox box, IntVec dir)
        {
            bool firstBox = visited.Count == 0;

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
                if (left != null)
                    branches.Add(left);
                var right = CurrentState.EntityAt<StickyBox>(nextBox.Pos + ortho);
                if (right != null)
                    branches.Add(right);

                if (!visited.Add(nextBox))
                    break;
                group.Add(nextBox);
                nextBox = CurrentState.EntityAt<StickyBox>(nextBox.Pos - dir);
            }
            // move forwards
            nextBox = CurrentState.EntityAt<StickyBox>(box.Pos);
            while (nextBox != null)
            {
                var left = CurrentState.EntityAt<StickyBox>(nextBox.Pos - ortho);
                if (left != null)
                    branches.Add(left);
                var right = CurrentState.EntityAt<StickyBox>(nextBox.Pos + ortho);
                if (right != null)
                    branches.Add(right);

                if (!visited.Add(nextBox))
                    break;
                group.Add(nextBox);
                nextBox = CurrentState.EntityAt<StickyBox>(nextBox.Pos + dir);
            }
                        
            // can only move 1 wide groups!
            //if (firstBox && group.Count > 1)
            //    return false;

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

        public Teleporter TeleporterAt(IntVec pos)
        {
            foreach (Teleporter t in Teleporters)
                if (t.Pos == pos)
                    return t;
            return null;
        }
    }
}
