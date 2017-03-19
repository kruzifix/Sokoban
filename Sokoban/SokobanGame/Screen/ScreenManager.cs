using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SokobanGame.Screen
{
    public class ScreenManager
    {
        private static ScreenManager instance = null;

        public static ScreenManager CreateScreenManager(Game game, GameScreen startupScreen)
        {
            if (instance != null)
            {
                instance = new ScreenManager(game, startupScreen);
            }
            return instance;
        }

        public static ScreenManager Instance { get { return instance; } }

        private Game game;
        private Stack<GameScreen> activeScreens;

        private ScreenManager(Game game, GameScreen startupScreen)
        {
            this.game = game;

            activeScreens = new Stack<GameScreen>();
            activeScreens.Push(startupScreen);
        }
    }
}
