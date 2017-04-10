using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SokobanGame.Input
{
    public class InputManager
    {
        private static KeyboardState lastState;
        private static KeyboardState currentState;

        private static GamePadState lastPadState;
        private static GamePadState currentPadState;
        private static bool thumbWasCenter = false;
        // TODO: tweak values!
        private static float centerThreshold = 0.3f;
        private static float activeThreshold = 0.7f;
        private static float angleTolerance = 0.35f;

        public static Vector2 LeftThumb
        {
            get
            {
                return (currentPadState.ThumbSticks.Left + lastPadState.ThumbSticks.Left) * 0.5f;
            }
        }

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

            states.Add("up", new List<InputState>() { GetKeyState(Keys.Up), GetKeyState(Keys.W) });
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

                Vector2 v = LeftThumb;
                if (v.Length() < centerThreshold)
                    thumbWasCenter = true;
                
                bool active = v.Length() > activeThreshold;
                float angle = (float)Math.Atan2(v.Y, v.X);

                bool rightActive = active && (Math.Abs(angle) < angleTolerance);
                bool leftActive = active && (Math.Abs(Math.PI - Math.Abs(angle)) < angleTolerance);
                bool upActive = active && (Math.Abs(angle - Math.PI * 0.5f) < angleTolerance);
                bool downActive = active && (Math.Abs(angle + Math.PI * 0.5f) < angleTolerance);

                states["up"].Add(GetButtonState(Buttons.DPadUp));
                if (thumbWasCenter && upActive)
                {
                    states["up"].Add(InputState.Pressed);
                    thumbWasCenter = false;
                }

                states["down"].Add(GetButtonState(Buttons.DPadDown));
                if (thumbWasCenter && downActive)
                {
                    states["down"].Add(InputState.Pressed);
                    thumbWasCenter = false;
                }

                states["left"].Add(GetButtonState(Buttons.DPadLeft));
                if (thumbWasCenter && leftActive)
                {
                    states["left"].Add(InputState.Pressed);
                    thumbWasCenter = false;
                }

                states["right"].Add(GetButtonState(Buttons.DPadRight));
                if (thumbWasCenter && rightActive)
                {
                    states["right"].Add(InputState.Pressed);
                    thumbWasCenter = false;
                }

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

        public static void DrawDebugThumbStick(int x, int y, int size)
        {
            SokobanGame.Instance.SpriteBatch.DrawRect(x, y, size, size, Color.White, Align.Center);
            SokobanGame.Instance.SpriteBatch.DrawRect(x, y, size, size * 0.15f, Color.DodgerBlue, Align.Center);
            SokobanGame.Instance.SpriteBatch.DrawRect(x, y, size * 0.15f, size, Color.DodgerBlue, Align.Center);

            Vector2 v = LeftThumb;

            float angle = (float)Math.Atan2(v.Y, v.X);
            bool rightActive = v.Length() > activeThreshold && (Math.Abs(angle) < angleTolerance);
            bool leftActive = v.Length() > activeThreshold && (Math.Abs(Math.PI - Math.Abs(angle)) < angleTolerance);

            bool upActive = v.Length() > activeThreshold && (Math.Abs(angle - Math.PI * 0.5f) < angleTolerance);
            bool downActive = v.Length() > activeThreshold && (Math.Abs(angle + Math.PI * 0.5f) < angleTolerance);

            float rectSize = (1f - activeThreshold) * size * 0.5f;

            SokobanGame.Instance.SpriteBatch.DrawRect(x + size * 0.5f, y, rectSize, 125, rightActive ? Color.Red : Color.Brown, Align.MidRight);
            SokobanGame.Instance.SpriteBatch.DrawRect(x - size * 0.5f, y, rectSize, 125, leftActive ? Color.Red : Color.Brown, Align.MidLeft);
            SokobanGame.Instance.SpriteBatch.DrawRect(x, y - size * 0.5f, 125, rectSize, upActive ? Color.Red : Color.Brown, Align.TopMid);
            SokobanGame.Instance.SpriteBatch.DrawRect(x, y + size * 0.5f, 125, rectSize, downActive ? Color.Red : Color.Brown, Align.BotMid);

            float centerThreshold = 0.2f;
            bool center = v.Length() < centerThreshold;
            SokobanGame.Instance.SpriteBatch.DrawRect(x, y, size * centerThreshold, size * centerThreshold, center ? Color.LawnGreen : Color.ForestGreen, Align.Center);

            SokobanGame.Instance.SpriteBatch.DrawRect(x + v.X * size * 0.5f, y - v.Y * size * 0.5f, 20, 20, Color.Black, Align.Center);
            SokobanGame.Instance.SpriteBatch.DrawRect(x + v.X * size * 0.5f, y - v.Y * size * 0.5f, 8, 60, Color.Black, Align.Center);
            SokobanGame.Instance.SpriteBatch.DrawRect(x + v.X * size * 0.5f, y - v.Y * size * 0.5f, 60, 8, Color.Black, Align.Center);
        }
    }
}
