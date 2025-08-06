using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace HalvesOfTria.Classes
{
    public enum InputAction
    {
        WalkLeft,
        WalkRight,
        Jump,
        QuickQuit,
        DebugMove
    }

    public enum MouseButton // c# doesn't have a built-in enum for mouse buttons
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2
    }

    /// <summary>
    /// Responsible for managing input actions.
    /// It provides functionality to bind keys to specific actions and check their states,
    /// (i.e. whether an action is being held, just pressed, or just released).
    /// </summary>
    /// <remarks>
    /// [todo: add controller support]
    /// </remarks>
    public static class InputManager
    {
        #region Properties and Fields
        /// <remarks>
        /// Each action can have multiple keys bound to it.
        /// </remarks>
        private static Dictionary<InputAction, List<Keys>> _keyBindings = new()
        {
            { InputAction.WalkLeft, new() { Keys.A, Keys.Left } },
            { InputAction.WalkRight, new() { Keys.D, Keys.Right } },
            { InputAction.Jump, new() { Keys.W, Keys.Up, Keys.Space } },
            { InputAction.QuickQuit, new() { Keys.Escape } },
            { InputAction.DebugMove, new() { } }
        };

        /// <remarks>
        /// Each action can have multiple keys bound to it.
        /// </remarks>
        private static Dictionary<InputAction, List<MouseButton>> _mouseBindings = new()
        {
            { InputAction.WalkLeft, new() { } },
            { InputAction.WalkRight, new() { } },
            { InputAction.Jump, new() { } },
            { InputAction.QuickQuit, new() { } },
            { InputAction.DebugMove, new() { MouseButton.Right } }
        };

        private static KeyboardState _previousKeyboardState;
        private static KeyboardState _currentKeyboardState;

        private static MouseState _previousMouseState;
        private static MouseState _currentMouseState;

        public static Vector2 MousePosition => new(_currentMouseState.X, _currentMouseState.Y);
        #endregion

        #region Update Method
        /// <summary>
        /// Updates the current and previous keyboard and mouse states.
        /// </summary>
        /// <remarks>
        /// This method should be called once per frame in <see cref="Game1.Update"/> to ensure input states are updated correctly each frame.
        /// </remarks>
        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Checks if a specific action is currently being held down.
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns>True if any key bound to the action is currently pressed; otherwise, false.</returns>
        public static bool IsActionHeld(InputAction action)
        {
            return (_keyBindings.ContainsKey(action) && _keyBindings[action].Any(key => _currentKeyboardState.IsKeyDown(key))) || // checking _keyBindings[action]
                   (_mouseBindings.ContainsKey(action) && _mouseBindings[action].Any(button => IsMouseButtonDown(_currentMouseState, button))); // checking _mouseBindings[action]
        }

        /// <summary>
        /// Checks if a specific action was just pressed during the current frame.
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns>True if any key bound to the action was pressed this frame and not in the previous frame; otherwise, false.</returns>
        public static bool IsActionJustPressed(InputAction action)
        {
            return (_keyBindings.ContainsKey(action) && _keyBindings[action].Any(key => _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key))) || // checking _keyBindings[action]
                   (_mouseBindings.ContainsKey(action) && _mouseBindings[action].Any(button => IsMouseButtonDown(_currentMouseState, button) && IsMouseButtonUp(_currentMouseState, button))); // checking _mouseBindings[action]
        }

        /// <summary>
        /// Checks if a specific action was just released during the current frame.
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns>True if any key bound to the action was released this frame and was pressed in the previous frame; otherwise, false.</returns>
        public static bool IsActionJustReleased(InputAction action)
        {
            return (_keyBindings.ContainsKey(action) && _keyBindings[action].Any(key => _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key))) || // checking _keyBindings[action]
                   (_mouseBindings.ContainsKey(action) && _mouseBindings[action].Any(button => IsMouseButtonUp(_currentMouseState, button) && IsMouseButtonDown(_previousMouseState, button))); // checking _mouseBindings[action]
        }
        #endregion

        #region Helper Methods
        // These are private to ensure that only Actions are referenced by external code, not specific keys or mouse buttons (which could cause hardcoding issues).

        /// <summary>
        /// Checks if a given mouse button is being pressed in the given state.
        /// </summary>
        /// <param name="state">The current state of the mouse.</param>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>True if the given mouse button is pressed; otherwise, false.</returns>
        private static bool IsMouseButtonDown(MouseState state, MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => state.LeftButton == ButtonState.Pressed,
                MouseButton.Right => state.RightButton == ButtonState.Pressed,
                MouseButton.Middle => state.MiddleButton == ButtonState.Pressed,
                MouseButton.XButton1 => state.XButton1 == ButtonState.Pressed,
                MouseButton.XButton2 => state.XButton2 == ButtonState.Pressed,
                _ => false
            };
        }

        /// <summary>
        /// Checks if a given mouse button is not being pressed in the given state.
        /// </summary>
        /// <param name="state">The current state of the mouse.</param>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>True if the given mouse button is not pressed; otherwise, false.</returns>
        private static bool IsMouseButtonUp(MouseState state, MouseButton button)
        {
            return !IsMouseButtonDown(state, button);
        }
        #endregion
    }
}
