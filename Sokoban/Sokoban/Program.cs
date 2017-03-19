using System;

namespace Sokoban
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SokobanGame.SokobanGame())
            {
                game.Run();
            }
        }
    }
#endif
}
