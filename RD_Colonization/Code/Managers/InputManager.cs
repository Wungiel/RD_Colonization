using Microsoft.Xna.Framework.Input;

namespace RD_Colonization.Code
{
    public static class InputManager
    {
        private static KeyboardState oldStateKeyboard;
        private static KeyboardState newStateKeyboard;
        private static MouseState oldStateMouse;
        private static MouseState newStateMouse;

        public static bool isSinglePress(Keys key)
        {
            return newStateKeyboard.IsKeyDown(key) && oldStateKeyboard.IsKeyUp(key);
        }

        public static bool isSingleLeftPress()
        {
            return newStateMouse.LeftButton == ButtonState.Pressed && oldStateMouse.LeftButton == ButtonState.Released;
        }

        public static bool isSingleRighttPress()
        {
            return newStateMouse.RightButton == ButtonState.Pressed && oldStateMouse.RightButton == ButtonState.Released;
        }

        public static bool IsKeyDown(Keys key)
        {
            return newStateKeyboard.IsKeyDown(key);
        }

        public static void updateState(KeyboardState keyboardState, MouseState mouseState)
        {
            oldStateKeyboard = newStateKeyboard;
            newStateKeyboard = keyboardState;
            oldStateMouse = newStateMouse;
            newStateMouse = mouseState;
        }

        public static int isMouseWheel()
        {
            if (newStateMouse.ScrollWheelValue < oldStateMouse.ScrollWheelValue)
                return 1;
            else if (newStateMouse.ScrollWheelValue > oldStateMouse.ScrollWheelValue)
                return -1;
            else return 0;
        }
    }
}