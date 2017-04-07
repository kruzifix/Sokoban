using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SokobanGame.Input
{
    public class InputManager
    {
        private static KeyboardState lastState;
        private static KeyboardState currentState;

        private static Dictionary<string, InputState> inputStates;

        static InputManager()
        {
            inputStates = new Dictionary<string, InputState>();
            inputStates.Add("up", InputState.Up);
            inputStates.Add("down", InputState.Up);
            inputStates.Add("left", InputState.Up);
            inputStates.Add("right", InputState.Up);
            inputStates.Add("back", InputState.Up);
            inputStates.Add("confirm", InputState.Up);
            inputStates.Add("reset", InputState.Up);
            inputStates.Add("undo", InputState.Up);
        }

        public static void Update()
        {
            lastState = currentState;
            currentState = Keyboard.GetState();

            inputStates["up"] = GetKeyState(Keys.Up);
            inputStates["down"] = GetKeyState(Keys.Down);
            inputStates["left"] = GetKeyState(Keys.Left);
            inputStates["right"] = GetKeyState(Keys.Right);

            inputStates["back"] = GetKeyState(Keys.Escape);
            inputStates["confirm"] = GetKeyState(Keys.Enter);
            inputStates["reset"] = GetKeyState(Keys.R);
            inputStates["undo"] = GetKeyState(Keys.Z);
        }

        private static InputState GetKeyState(Keys key)
        {
            if (currentState.IsKeyDown(key))
            {
                if (lastState.IsKeyDown(key))
                    return InputState.Down;
                else
                    return InputState.Pressed;
            }
            else
            {
                if (lastState.IsKeyUp(key))
                    return InputState.Up;
                else
                    return InputState.Released;
            }
        }

        public static InputState GetInput(string input)
        {
            return inputStates[input];
        }

        public static bool Pressed(string input)
        {
            return inputStates[input] == InputState.Pressed;
        }

        public static bool KeyPressed(Keys key)
        {
            return lastState.IsKeyUp(key) && currentState.IsKeyDown(key);
        }
    }
}
