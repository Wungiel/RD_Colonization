using Microsoft.Xna.Framework.Input;

namespace RD_Colonization.Code
{
    public class InputManager : BaseManager<InputManager>
    {
        private KeyboardState oldStateKeyboard;
        private KeyboardState newStateKeyboard;
        private MouseState oldStateMouse;
        private MouseState newStateMouse;

        public bool IsSinglePress(Keys key)
        {
            return newStateKeyboard.IsKeyDown(key) && oldStateKeyboard.IsKeyUp(key);
        }

        public bool IsSingleLeftPress()
        {
            return newStateMouse.LeftButton == ButtonState.Pressed && oldStateMouse.LeftButton == ButtonState.Released;
        }

        public bool IsSingleRightPress()
        {
            return newStateMouse.RightButton == ButtonState.Pressed && oldStateMouse.RightButton == ButtonState.Released;
        }

        public bool IsKeyDown(Keys key)
        {
            return newStateKeyboard.IsKeyDown(key);
        }

        public void UpdateState(KeyboardState keyboardState, MouseState mouseState)
        {
            oldStateKeyboard = newStateKeyboard;
            newStateKeyboard = keyboardState;
            oldStateMouse = newStateMouse;
            newStateMouse = mouseState;
        }
    }
}