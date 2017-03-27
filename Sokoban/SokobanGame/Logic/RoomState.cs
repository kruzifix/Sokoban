using System.Collections.Generic;
using System.Linq;

namespace SokobanGame.Logic
{
    public class RoomState
    {
        public IntVec PlayerPosition { get; set; }
        public List<Entity> Entities { get; private set; }

        private List<Box> boxes;
        public List<Box> Boxes
        {
            get
            {
                if (boxes == null)
                    boxes = Entities.Where(e => e is Box).Cast<Box>().ToList();
                return boxes;
            }
        }

        public RoomState(IntVec playerPosition, IEnumerable<Entity> ents)
        {
            PlayerPosition = playerPosition;
            Entities = new List<Entity>(ents);
        }

        public RoomState Copy()
        {
            var copy = new List<Entity>();
            foreach (var e in Entities)
                copy.Add(e.Copy());
            return new RoomState(PlayerPosition, copy);
        }

        public Box BoxAt(IntVec pos)
        {
            foreach (var b in Boxes)
                if (b.Pos == pos)
                    return b;
            return null;
        }
    }
}
