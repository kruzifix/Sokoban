// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

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

        public int MoveCount { get; set; }

        public RoomState(IntVec playerPosition, IEnumerable<Entity> ents)
        {
            PlayerPosition = playerPosition;
            Entities = new List<Entity>(ents);
            MoveCount = 0;

            HashEntities();
        }

        private void HashEntities()
        {
            Boxes = Entities.OfType<Box>().ToList();
            StickyBoxes = Entities.OfType<StickyBox>().ToList();
            Holes = Entities.OfType<Hole>().ToList();
        }

        public RoomState Copy()
        {
            var rs = new RoomState(PlayerPosition, Entities.ConvertAll(e => e.Copy()));
            rs.MoveCount = MoveCount;
            return rs;
        }

        public Entity EntityAt(IntVec pos)
        {
            // TODO: multiple entities at same position?

            // TODO: kinda hacky fix for box movement on holes
            foreach (var e in Boxes)
                if (e.Pos == pos)
                    return e;
            foreach (var e in StickyBoxes)
                if (e.Pos == pos)
                    return e;
            foreach (var e in Holes)
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
