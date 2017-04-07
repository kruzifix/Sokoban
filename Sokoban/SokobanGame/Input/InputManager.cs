using Microsoft.Xna.Framework.Input;

namespace SokobanGame.Input
{
    public class InputManager
    {
        private static KeyboardState lastState;
        private static KeyboardState currentState;

        static InputManager() { }

        public static void Update()
        {
            lastState = currentState;
            currentState = Keyboard.GetState();
        }

        public static bool KeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public static bool KeyUp(Keys key)
        {
            return currentState.IsKeyUp(key);
        }

        public static bool KeyPress(Keys key)
        {
            return currentState.IsKeyDown(key) && lastState.IsKeyUp(key);
        }

        public static bool KeyRelease(Keys key)
        {
            return currentState.IsKeyUp(key) && lastState.IsKeyDown(key);
        }
    }
}
