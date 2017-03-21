namespace SokobanGame.Logic
{
    public class RoomState
    {
        public IntVec PlayerPosition { get; set; }
        public IntVec[] Switches { get; set; }
        public IntVec[] Boxes { get; set; }

        public RoomState(IntVec playerPosition, IntVec[] switches, IntVec[] boxes)
        {
            PlayerPosition = playerPosition;
            Switches = switches;
            Boxes = boxes;
        }

        public RoomState Copy()
        {
            return new RoomState(PlayerPosition, (IntVec[])Switches.Clone(), (IntVec[])Boxes.Clone());
        }

        public bool IsSolved()
        {
            for (int i = 0; i < Switches.Length; i++)
            {
                bool boxFound = false;

                for (int j = 0; j < Boxes.Length; j++)
                {
                    if (Switches[i] == Boxes[j])
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

        public int BoxAt(IntVec pos)
        {
            for (int i = 0; i < Boxes.Length; i++)
            {
                if (Boxes[i] == pos)
                    return i;
            }
            return -1;
        }
    }
}
