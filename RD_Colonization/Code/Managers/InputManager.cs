using Microsoft.Xna.Framework.Input;

namespace RD_Colonization.Code
{
    internal static class InputManager
    {
        private static KeyboardState oldStateKeyboard;
        private static KeyboardState newStateKeyboard;
        private static MouseState oldStateMouse;
        private static MouseState newStateMouse;

        internal static bool isSinglePress(Keys key)
        {
            return newStateKeyboard.IsKeyDown(key) && oldStateKeyboard.IsKeyUp(key);
        }

        internal static bool isSinglePress()
        {
            return newStateMouse.LeftButton == ButtonState.Pressed && oldStateMouse.LeftButton == ButtonState.Released;
        }

        internal static bool IsKeyDown(Keys key)
        {
            return newStateKeyboard.IsKeyDown(key);
        }

        internal static void updateState(KeyboardState keyboardState, MouseState mouseState)
        {
            oldStateKeyboard = newStateKeyboard;
            newStateKeyboard = keyboardState;
            oldStateMouse = newStateMouse;
            newStateMouse = mouseState;
        }

        internal static int isMouseWheel()
        {
            if (newStateMouse.ScrollWheelValue < oldStateMouse.ScrollWheelValue)
                return 1;
            else if (newStateMouse.ScrollWheelValue > oldStateMouse.ScrollWheelValue)
                return -1;
            else return 0;
        }
    }
}