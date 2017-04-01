using System.Collections.Generic;
using System.Linq;

namespace SokobanGame.Logic
{
    public class RoomState
    {
        public IntVec PlayerPosition { get; set; }
        public List<Entity> Entities { get; private set; }

        public List<Box> Boxes { get; private set; }
        public List<StickyBox> StickyBoxes { get; private set; }
        public List<Hole> Holes { get; private set; }

        public RoomState(IntVec playerPosition, IEnumerable<Entity> ents)
        {
            PlayerPosition = playerPosition;
            Entities = new List<Entity>(ents);

            HashEntities();
        }

        private void HashEntities()
        {
            Boxes = Entities.Where(e => e is Box).Cast<Box>().ToList();
            StickyBoxes = Entities.Where(e => e is StickyBox).Cast<StickyBox>().ToList();
            Holes = Entities.Where(e => e is Hole).Cast<Hole>().ToList();
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

        public void RemoveEntity(Entity ent)
        {
            Entities.Remove(ent);
            HashEntities();
        }
    }
}
