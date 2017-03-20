using Microsoft.Xna.Framework.Input;

namespace SokobanGame
{
    public class InputManager
    {
        private static InputManager instance = null;

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputManager();
                return instance;
            }
        }

        private KeyboardState lastState;
        private KeyboardState currentState;

        private InputManager() { }

        public void Update()
        {
            lastState = currentState;
            currentState = Keyboard.GetState();
        }

        public bool KeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public bool KeyUp(Keys key)
        {
            return currentState.IsKeyUp(key);
        }

        public bool KeyPress(Keys key)
        {
            return currentState.IsKeyDown(key) && lastState.IsKeyUp(key);
        }

        public bool KeyRelease(Keys key)
        {
            return currentState.IsKeyUp(key) && lastState.IsKeyDown(key);
        }
    }
}
