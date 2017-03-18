using System;

namespace Sokoban
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SokobanApp())
            {
                game.Run();
            }
        }
    }
#endif
}
