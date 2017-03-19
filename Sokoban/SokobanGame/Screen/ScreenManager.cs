﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

/*
 * Reference: https://blogs.msdn.microsoft.com/etayrien/2006/12/12/basic-game-engine-structure/
 */

namespace SokobanGame.Screen
{
    public class ScreenManager
    {
        private static ScreenManager instance = null;

        public static void CreateScreenManager(Screen startupScreen)
        {
            if (instance != null)
            {
                throw new ApplicationException("ScreenManager.CreateScreenManager(Screen): Only one instance of ScreenManager is allowed!");
            }
            instance = new ScreenManager(startupScreen);
        }

        public static ScreenManager Instance { get { return instance; } }
        
        private Stack<Screen> activeScreens;

        private ScreenManager(Screen startupScreen)
        {
            activeScreens = new Stack<Screen>();
            activeScreens.Push(startupScreen);
        }

        public Screen RemoveScreen()
        {
            return activeScreens.Pop();
        }

        public void AddScreen(Screen screen)
        {
            activeScreens.Push(screen);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var screen in activeScreens)
            {
                screen.Update(gameTime);
                if (screen.BlocksUpdate)
                    break;
            }
        }

        public void Draw(GameTime gameTime)
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
    }
}
