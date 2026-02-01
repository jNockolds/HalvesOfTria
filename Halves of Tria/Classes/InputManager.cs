using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace Halves_of_Tria.Input
{
    /// <summary>
    /// Specifies the set of possible input actions recognized by the game.
    /// </summary>
    internal enum InputAction
    {
        SaltWalkLeft,
        SaltWalkRight,
        SaltJump,
        QuickQuit,
        SaltDebugMove
    }

    /// <summary>
    /// Responsible for managing input actions,
    /// provides functionality to bind keys to specific actions and check their states
    /// (i.e. whether an action is being held, was just pressed, or was just released).
    /// </summary>
    internal static class InputManager
    {
        #region Fields
        private static readonly float _gamepadDeadzone = 0.2f;

        private static GamePadState _previousGamepadState;
        private static GamePadState _gamepadState;


        // Current Bindings:

        private static Dictionary<InputAction, List<Keys>> _keyboardBindings = new();
        private static Dictionary<InputAction, List<MouseButton>> _mouseBindings = new();
        private static Dictionary<InputAction, List<Buttons>> _gamepadBindings = new();


        // Default Bindings:

        private static readonly Dictionary<InputAction, List<Keys>> _defaultKeyboardBindings = new()
        {
            { InputAction.SaltWalkLeft, new List<Keys> { Keys.A, Keys.Left } },
            { InputAction.SaltWalkRight, new List<Keys> { Keys.D, Keys.Right } },
            { InputAction.SaltJump, new List<Keys> { Keys.W, Keys.Up, Keys.Space } },
            { InputAction.QuickQuit, new List<Keys> { Keys.X } }
        };

        private static Dictionary<InputAction, List<MouseButton>> _defaultMouseBindings = new()
        {
            { InputAction.SaltWalkLeft, new List<MouseButton> { } },
            { InputAction.SaltDebugMove, new List<MouseButton> { MouseButton.Right } },
            { InputAction.QuickQuit, new List<MouseButton> { MouseButton.Middle } }
        };

        private static Dictionary<InputAction, List<Buttons>> _defaultGamepadBindings = new()
        {
            { InputAction.SaltWalkLeft, new List<Buttons> { Buttons.DPadLeft, Buttons.LeftThumbstickLeft } },
            { InputAction.SaltWalkRight, new List<Buttons> { Buttons.DPadRight, Buttons.LeftThumbstickRight } },
            { InputAction.SaltJump, new List<Buttons> { Buttons.A } },
            { InputAction.QuickQuit, new List<Buttons> { Buttons.B, Buttons.LeftThumbstickDown, Buttons.RightTrigger } }
        };
        #endregion

        #region Game Loop Methods
        public static void Initialize()
        {
            InitializeBindingDictionaries();
            ResetBindingsToDefault();

            _gamepadState = GamePad.GetState(PlayerIndex.One);
            _previousGamepadState = _gamepadState;
        }

        public static void Update()
        {
            KeyboardExtended.Update();
            MouseExtended.Update();

            _previousGamepadState = _gamepadState;
            _gamepadState = GamePad.GetState(PlayerIndex.One);
        }
        #endregion

        #region Binding Methods
        /// <summary>
        /// Binds a keyboard key to the specified input action.
        /// </summary>
        /// <param name="action">The input action to which the keyboard key will be bound.</param>
        /// <param name="button">The keyboard key to bind to the specified action.</param>
        public static void AddActionBinding(InputAction action, Keys button)
        {
            if (!_keyboardBindings[action].Contains(button))
            {
                _keyboardBindings[action].Add(button);
            }
        }

        /// <summary>
        /// Binds a mouse button to the specified input action.
        /// </summary>
        /// <param name="action">The input action to which the mouse button will be bound.</param>
        /// <param name="button">The mouse button to bind to the specified action.</param>
        public static void AddActionBinding(InputAction action, MouseButton button)
        {
            if (!_mouseBindings[action].Contains(button))
            {
                _mouseBindings[action].Add(button);
            }
        }

        /// <summary>
        /// Binds a gamepad button to the specified input action.
        /// </summary>
        /// <remarks>Note that "button" can refer to standard buttons, triggers, or thumbstick directions.</remarks>
        /// <param name="action">The input action to which the gamepad button will be bound.</param>
        /// <param name="button">The gamepad button to bind to the specified action.</param>
        public static void AddActionBinding(InputAction action, Buttons button)
        {
            if (!_gamepadBindings[action].Contains(button))
            {
                _gamepadBindings[action].Add(button);
            }
        }

        /// <summary>
        /// Binds an enumerable of keyboard keys to the specified input action.
        /// </summary>
        /// <param name="action">The input action to which the keyboard keys will be bound.</param>
        /// <param name="buttons">An enumerable of the keyboard keys to bind to the specified action.</param>
        public static void AddActionBinding(InputAction action, IEnumerable<Keys> buttons)
        {
            foreach (Keys button in buttons)
            {
                AddActionBinding(action, button);
            }
        }

        /// <summary>
        /// Binds an enumerable of mouse buttons to the specified input action.
        /// </summary>
        /// <param name="action">The input action to which the mouse buttons will be bound.</param>
        /// <param name="buttons">An enumerable of the mouse buttons to bind to the specified action.</param>
        public static void AddActionBinding(InputAction action, IEnumerable<MouseButton> buttons)
        {
            foreach (MouseButton button in buttons)
            {
                AddActionBinding(action, button);
            }
        }

        /// <summary>
        /// Binds an enumerable of gamepad buttons to the specified input action.
        /// </summary>
        /// <remarks>Note that "button" can refer to standard buttons, triggers, or thumbstick directions.</remarks>
        /// <param name="action">The input action to which the gamepad buttons will be bound.</param>
        /// <param name="buttons">An enumerable of the gamepad buttons to bind to the specified action.</param>
        public static void AddActionBinding(InputAction action, IEnumerable<Buttons> buttons)
        {
            foreach (Buttons button in buttons)
            {
                AddActionBinding(action, button);
            }
        }

        /// <summary>
        /// Binds multiple input actions to keyboard keys using the specified dictionary of actions and their associated keyboard key lists.
        /// </summary>
        /// <param name="bindings">A dictionary that maps each input action to a list of keyboard keys to bind. Each key in the dictionary represents an
        /// action, and its associated list contains the keyboard keys that will trigger that action. Cannot be null.</param>
        public static void AddActionBinding(Dictionary<InputAction, List<Keys>> bindings)
        {
            foreach (InputAction action in bindings.Keys)
            {
                AddActionBinding(action, bindings[action]);
            }
        }

        /// <summary>
        /// Binds multiple input actions to mouse buttons using the specified dictionary of actions and their associated mouse button lists.
        /// </summary>
        /// <param name="bindings">A dictionary that maps each input action to a list of mouse buttons to bind. Each key in the dictionary represents an
        /// action, and its associated list contains the mouse buttons that will trigger that action. Cannot be null.</param>
        public static void AddActionBinding(Dictionary<InputAction, List<MouseButton>> bindings)
        {
            foreach (InputAction action in bindings.Keys)
            {
                AddActionBinding(action, bindings[action]);
            }
        }

        /// <summary>
        /// Binds multiple input actions to gamepad buttons using the specified dictionary of actions and their associated gamepad button lists.
        /// </summary>
        /// <remarks>Note that "button" can refer to standard buttons, triggers, or thumbstick directions.</remarks>
        /// <param name="bindings">A dictionary that maps each input action to a list of gamepad buttons to bind. Each key in the dictionary represents an
        /// action, and its associated list contains the gamepad buttons that will trigger that action. Cannot be null.</param>
        public static void AddActionBinding(Dictionary<InputAction, List<Buttons>> bindings)
        {
            foreach (InputAction action in bindings.Keys)
            {
                AddActionBinding(action, bindings[action]);
            }
        }

        /// <summary>
        /// Clears all current input bindings for keyboard, mouse, and gamepad.
        /// </summary>
        /// <remarks>This clears the dictionaries that map input actions to their associated keys/buttons, 
        /// and then re-initializes them with <see cref="InputAction"/> keys and empty <see cref="System.Collections.Generic.List{T}"/> values.</remarks>
        public static void ClearAllBindings()
        {
            _keyboardBindings.Clear();
            _mouseBindings.Clear();
            _gamepadBindings.Clear();

            InitializeBindingDictionaries();
        }

        /// <summary>
        /// Resets all input bindings for keyboard, mouse, and gamepad to their defaults, removing any non-default bindings.
        /// </summary>
        public static void ResetBindingsToDefault()
        {
            ClearAllBindings();

            AddActionBinding(_defaultKeyboardBindings);
            AddActionBinding(_defaultMouseBindings);
            AddActionBinding(_defaultGamepadBindings);
        }
        #endregion

        #region Action Input Methods
        /// <summary>
        /// Returns whether any key bound to the specified action is down in the current state. 
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns><see langword="true"/> if any key bound to the action is down this state; 
        /// otherwise, <see langword="false"/>.</returns>
        public static bool IsActionDown(InputAction action)
        {
            KeyboardStateExtended keyboardState = KeyboardExtended.GetState();
            MouseStateExtended mouseState = MouseExtended.GetState();

            foreach (Keys key in _keyboardBindings[action])
            {
                if (keyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }

            foreach (MouseButton button in _mouseBindings[action])
            {
                if (mouseState.IsButtonDown(button))
                {
                    return true;
                }
            }

            foreach (Buttons button in _gamepadBindings[action])
            {
                if (IsGamepadInputActivated(_gamepadState, button))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns whether any key bound to the specified action was up during the previous state, but is now down. 
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns><see langword="true"/> if any key bound to the action was pressed this state and not in the previous state; 
        /// otherwise, <see langword="false"/>.</returns>
        public static bool WasActionJustPressed(InputAction action)
        {
            KeyboardStateExtended keyboardState = KeyboardExtended.GetState();
            MouseStateExtended mouseState = MouseExtended.GetState();

            foreach (Keys key in _keyboardBindings[action])
            {
                if (keyboardState.WasKeyPressed(key))
                {
                    return true;
                }
            }

            foreach (MouseButton button in _mouseBindings[action])
            {
                if (mouseState.WasButtonPressed(button))
                {
                    return true;
                }
            }

            foreach (Buttons button in _gamepadBindings[action])
            {
                if (!IsGamepadInputActivated(_previousGamepadState, button)
                    && IsGamepadInputActivated(_gamepadState, button))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns whether any key bound to the specified action was down during the previous state, but is now up. 
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns><see langword="true"/> if any key bound to the action was released this state and not in the previous state; 
        /// otherwise, <see langword="false"/>.</returns>
        public static bool WasActionJustReleased(InputAction action)
        {
            KeyboardStateExtended keyboardState = KeyboardExtended.GetState();
            MouseStateExtended mouseState = MouseExtended.GetState();

            foreach (Keys key in _keyboardBindings[action])
            {
                if (keyboardState.WasKeyReleased(key))
                {
                    return true;
                }
            }

            foreach (MouseButton button in _mouseBindings[action])
            {
                if (mouseState.WasButtonReleased(button))
                {
                    return true;
                }
            }

            foreach (Buttons button in _gamepadBindings[action])
            {
                if (IsGamepadInputActivated(_previousGamepadState, button)
                    &&!IsGamepadInputActivated(_gamepadState, button))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Initializes the action dictionaries with empty lists for each action.
        /// </summary>
        private static void InitializeBindingDictionaries()
        {
            foreach (InputAction action in Enum.GetValues(typeof(InputAction)).Cast<InputAction>())
            {
                _keyboardBindings[action] = new List<Keys>();
                _mouseBindings[action] = new List<MouseButton>();
                _gamepadBindings[action] = new List<Buttons>();
            }
        }

        /// <summary>
        /// Determines whether the specified gamepad button or control is currently activated in the given gamepad
        /// state.
        /// </summary>
        /// <param name="state">The current state of the gamepad to evaluate.</param>
        /// <param name="button">The button or control to check for activation.</param>
        /// <returns>true if the specified button, trigger, or thumbstick direction is activated in the provided gamepad state;
        /// otherwise, false.</returns>
        private static bool IsGamepadInputActivated(GamePadState state, Buttons button)
        {
            return IsGamepadButtonDown(state, button) ||
                   IsGamepadTriggerDown(state, button) ||
                   IsGamepadThumbstickDirection(state, button);
        }

        /// <summary>
        /// Checks if a given gamepad button is being pressed in the given state.
        /// </summary>
        /// <param name="state">The current state of the gamepad.</param>
        /// <param name="button">The gamepad button to check.</param>
        /// <returns>true if the given gamepad button is pressed; otherwise, false.</returns>
        private static bool IsGamepadButtonDown(GamePadState state, Buttons button)
        {
            return button switch
            {
                Buttons.A => state.Buttons.A == ButtonState.Pressed,
                Buttons.B => state.Buttons.B == ButtonState.Pressed,
                Buttons.X => state.Buttons.X == ButtonState.Pressed,
                Buttons.Y => state.Buttons.Y == ButtonState.Pressed,

                Buttons.DPadUp => state.DPad.Up == ButtonState.Pressed,
                Buttons.DPadDown => state.DPad.Down == ButtonState.Pressed,
                Buttons.DPadLeft => state.DPad.Left == ButtonState.Pressed,
                Buttons.DPadRight => state.DPad.Right == ButtonState.Pressed,

                Buttons.LeftShoulder => state.Buttons.LeftShoulder == ButtonState.Pressed,
                Buttons.RightShoulder => state.Buttons.RightShoulder == ButtonState.Pressed,

                Buttons.LeftStick => state.Buttons.LeftStick == ButtonState.Pressed,
                Buttons.RightStick => state.Buttons.RightStick == ButtonState.Pressed,
                _ => false
            };
        }

        /// <summary>
        /// Determines whether the specified trigger button on the gamepad is currently pressed beyond the configured
        /// deadzone.
        /// </summary>
        /// <remarks>This method only evaluates trigger buttons. For other button types, the method always
        /// returns false.</remarks>
        /// <param name="state">The current state of the gamepad to evaluate.</param>
        /// <param name="button">The trigger button to check. Must be either Buttons.LeftTrigger or Buttons.RightTrigger.</param>
        /// <returns>true if the specified trigger is pressed beyond the deadzone threshold; otherwise, false.</returns>
        private static bool IsGamepadTriggerDown(GamePadState state, Buttons button)
        {
            return button switch
            {
                Buttons.LeftTrigger => state.Triggers.Left > _gamepadDeadzone,
                Buttons.RightTrigger => state.Triggers.Right > _gamepadDeadzone,
                _ => false
            };
        }

        /// <summary>
        /// Determines whether the specified thumbstick on a gamepad is moved in the given direction beyond the
        /// configured deadzone.
        /// </summary>
        /// <remarks>This method only evaluates thumbstick direction buttons. For other button values, the
        /// method returns false. The deadzone threshold is used to prevent minor thumbstick movements from being
        /// registered as directional input.</remarks>
        /// <param name="state">The current state of the gamepad, including thumbstick positions.</param>
        /// <param name="button">The thumbstick direction to check. Must be one of the directional thumbstick button values (e.g.,
        /// LeftThumbstickUp, RightThumbstickLeft).</param>
        /// <returns>true if the specified thumbstick is moved in the given direction beyond the deadzone threshold; otherwise,
        /// false.</returns>
        private static bool IsGamepadThumbstickDirection(GamePadState state, Buttons button)
        {
            return button switch
            {
                Buttons.LeftThumbstickUp => state.ThumbSticks.Left.Y > _gamepadDeadzone,
                Buttons.LeftThumbstickDown => state.ThumbSticks.Left.Y < -_gamepadDeadzone,
                Buttons.LeftThumbstickLeft => state.ThumbSticks.Left.X < -_gamepadDeadzone,
                Buttons.LeftThumbstickRight => state.ThumbSticks.Left.X > _gamepadDeadzone,

                Buttons.RightThumbstickUp => state.ThumbSticks.Right.Y > _gamepadDeadzone,
                Buttons.RightThumbstickDown => state.ThumbSticks.Right.Y < -_gamepadDeadzone,
                Buttons.RightThumbstickLeft => state.ThumbSticks.Right.X < -_gamepadDeadzone,
                Buttons.RightThumbstickRight => state.ThumbSticks.Right.X > _gamepadDeadzone,
                _ => false
            };
        }
        #endregion
    }
}
