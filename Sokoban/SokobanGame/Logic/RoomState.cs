namespace SokobanGame.Logic
{
    public class RoomState
    {
        public IntVec PlayerPosition { get; private set; }
        public IntVec[] Switches { get; private set; }
        public IntVec[] Boxes { get; private set; }

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
    }
}
