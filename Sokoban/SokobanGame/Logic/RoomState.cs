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

        private List<StickyBox> stickyBoxes;
        public List<StickyBox> StickyBoxes
        {
            get
            {
                if (stickyBoxes == null)
                    stickyBoxes = Entities.Where(e => e is StickyBox).Cast<StickyBox>().ToList();
                return stickyBoxes;
            }
        }

        public RoomState(IntVec playerPosition, IEnumerable<Entity> ents)
        {
            PlayerPosition = playerPosition;
            Entities = new List<Entity>(ents);
        }

        public RoomState Copy()
        {
            return new RoomState(PlayerPosition, Entities.ConvertAll(e => e.Copy()));
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
