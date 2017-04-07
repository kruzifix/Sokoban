using System.Collections.Generic;
using Microsoft.Xna.Framework;

/*
 * Reference: https://blogs.msdn.microsoft.com/etayrien/2006/12/12/basic-game-engine-structure/
 */

namespace SokobanGame.Screen
{
    public class ScreenManager
    {
        private static Stack<Screen> activeScreens;

        public static Screen TopScreen { get { return activeScreens.Peek(); } }

        public static void Initialize(Screen startupScreen)
        {
            activeScreens = new Stack<Screen>();
            activeScreens.Push(startupScreen);
        }
        
        public static Screen RemoveScreen()
        {
            if (activeScreens.Count == 0)
                return null;
            Screen top = activeScreens.Pop();

            if (activeScreens.Count > 0)
                activeScreens.Peek().Activated();

            return top;
        }

        public static void AddScreen(Screen screen)
        {
            if (activeScreens.Count > 0)
                activeScreens.Peek().Disabled();

            activeScreens.Push(screen);
        }

        public static void Update(GameTime gameTime)
        {
            foreach (var screen in activeScreens)
            {
                screen.Update(gameTime);
                if (screen.BlocksUpdate)
                    break;
            }
        }

        public static void Draw(GameTime gameTime)
        {
            List<Screen> screensToDraw = new List<Screen>();
            foreach (var screen in activeScreens)
            {
                screensToDraw.Add(screen);
                if (screen.BlocksDraw)
                    break;
            }

            for (int i = screensToDraw.Count - 1; i >= 0; i--)
            {
                screensToDraw[i].Draw(gameTime);
            }
        }

        public static void Resized(int width, int height)
        {
            if (activeScreens == null)
                return;
            foreach (var screen in activeScreens)
            {
                screen.Resized(width, height);
            }
        }
    }
}
