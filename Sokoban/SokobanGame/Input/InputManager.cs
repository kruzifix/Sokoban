using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace SokobanGame.Input
{
    public class InputManager
    {
        private static KeyboardState lastState;
        private static KeyboardState currentState;

        private static GamePadState lastPadState;
        private static GamePadState currentPadState;

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

            Dictionary<string, List<InputState>> states = new Dictionary<string, List<InputState>>();

            states.Add("up", new List<InputState>(){ GetKeyState(Keys.Up), GetKeyState(Keys.W) });
            states.Add("down", new List<InputState>() { GetKeyState(Keys.Down), GetKeyState(Keys.S) });
            states.Add("left", new List<InputState>() { GetKeyState(Keys.Left), GetKeyState(Keys.A) });
            states.Add("right", new List<InputState>() { GetKeyState(Keys.Right), GetKeyState(Keys.D) });

            states.Add("back", new List<InputState>() { GetKeyState(Keys.Escape) });
            states.Add("confirm", new List<InputState>() { GetKeyState(Keys.Enter) });
            states.Add("reset", new List<InputState>() { GetKeyState(Keys.R) });
            states.Add("undo", new List<InputState>() { GetKeyState(Keys.Z) });

            if (GamePad.GetCapabilities(0).IsConnected)
            {
                lastPadState = currentPadState;
                currentPadState = GamePad.GetState(0);

                states["up"].Add(GetButtonState(Buttons.DPadUp));
                states["up"].Add(GetButtonState(Buttons.LeftThumbstickUp));

                states["down"].Add(GetButtonState(Buttons.DPadDown));
                states["down"].Add(GetButtonState(Buttons.LeftThumbstickDown));

                states["left"].Add(GetButtonState(Buttons.DPadLeft));
                states["left"].Add(GetButtonState(Buttons.LeftThumbstickLeft));

                states["right"].Add(GetButtonState(Buttons.DPadRight));
                states["right"].Add(GetButtonState(Buttons.LeftThumbstickRight));

                states["back"].Add(GetButtonState(Buttons.B));
                states["confirm"].Add(GetButtonState(Buttons.A));
                states["reset"].Add(GetButtonState(Buttons.Y));
                states["undo"].Add(GetButtonState(Buttons.X));
            }

            foreach (var kvp in states)
            {
                inputStates[kvp.Key] = ResolveStates(kvp.Value);
            }
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

        private static InputState GetButtonState(Buttons b)
        {
            if (lastPadState.IsButtonUp(b) && currentPadState.IsButtonDown(b))
                return InputState.Pressed;
            if (lastPadState.IsButtonDown(b) && currentPadState.IsButtonUp(b))
                return InputState.Released;
            if (lastPadState.IsButtonDown(b) && currentPadState.IsButtonDown(b))
                return InputState.Down;
            return InputState.Up;
        }

        private static InputState ResolveStates(IEnumerable<InputState> states)
        {
            if (states.All(s => s == InputState.Down))
                return InputState.Down;
            if (states.All(s => s == InputState.Up))
                return InputState.Up;
            if (states.All(s => s == InputState.Released))
                return InputState.Released;
            if (states.Any(s => s == InputState.Pressed))
                return InputState.Pressed;
            return InputState.Up;
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
