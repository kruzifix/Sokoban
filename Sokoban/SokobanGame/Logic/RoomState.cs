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

        private List<Hole> holes;
        public List<Hole> Holes
        {
            get
            {
                if (holes == null)
                    holes = Entities.Where(e => e is Hole).Cast<Hole>().ToList();
                return holes;
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

        public Entity EntityAt(IntVec pos)
        {
            foreach (var e in Entities)
                if (e.Pos == pos)
                    return e;
            return null;
        }

        public T EntityAt<T>(IntVec pos) 
            where T : Entity
        {
            var ent = EntityAt(pos);
            if (ent != null && ent is T)
                return ent as T;
            return null;
        }
    }
}
