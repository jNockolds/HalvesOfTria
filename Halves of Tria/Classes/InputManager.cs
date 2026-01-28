using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System.Diagnostics;

namespace Halves_of_Tria.Classes
{
    public enum InputAction
    {
        SaltWalkLeft,
        SaltWalkRight,
        SaltJump,
        QuickQuit,
        SaltDebugMove
    }

    /// <summary>
    /// Responsible for managing input actions.
    /// It provides functionality to bind keys to specific actions and check their states
    /// (i.e. whether an action is being held, was just pressed, or was just released).
    /// </summary>
    public static class InputManager
    {
        #region Fields
        private static readonly float _gamepadDeadzone = 0.2f;

        private static GamePadState _previousGamepadState;
        private static GamePadState _gamepadState;


        // Current Bindings:

        private static Dictionary<InputAction, List<Keys>> _keyboardBindings = new()
        {
            { InputAction.SaltWalkLeft, new List<Keys> { } },
            { InputAction.SaltWalkRight, new List<Keys> { } },
            { InputAction.SaltJump, new List<Keys> { } },
            { InputAction.QuickQuit, new List<Keys> { } },
            { InputAction.SaltDebugMove, new List<Keys> { } }
        };

        private static Dictionary<InputAction, List<MouseButton>> _mouseBindings = new()
        {
            { InputAction.SaltWalkLeft, new List<MouseButton> { } },
            { InputAction.SaltWalkRight, new List<MouseButton> { } },
            { InputAction.SaltJump, new List<MouseButton> { } },
            { InputAction.QuickQuit, new List<MouseButton> { } },
            { InputAction.SaltDebugMove, new List<MouseButton> { } }
        };

        private static Dictionary<InputAction, List<Buttons>> _gamepadBindings = new()
        {
            { InputAction.SaltWalkLeft, new List<Buttons> { } },
            { InputAction.SaltWalkRight, new List<Buttons> { } },
            { InputAction.SaltJump, new List<Buttons> { } },
            { InputAction.QuickQuit, new List<Buttons> { } },
            { InputAction.SaltDebugMove, new List<Buttons> { } }
        };


        // Default Bindings:

        private static readonly Dictionary<InputAction, List<Keys>> _defaultKeyboardBindings = new()
        {
            { InputAction.SaltWalkLeft, new List<Keys> { Keys.A, Keys.Left } },
            { InputAction.SaltWalkRight, new List<Keys> { Keys.D, Keys.Right } },
            { InputAction.SaltJump, new List<Keys> { Keys.W, Keys.Up, Keys.Space } },
            { InputAction.QuickQuit, new List<Keys> { Keys.Escape } }
        };

        private static Dictionary<InputAction, List<MouseButton>> _defaultMouseBindings = new()
        {
            { InputAction.SaltWalkLeft, new List<MouseButton> { } },
            { InputAction.SaltDebugMove, new List<MouseButton> { MouseButton.Right } },
            { InputAction.QuickQuit, new List<MouseButton> { MouseButton.Right } }
        };

        private static Dictionary<InputAction, List<Buttons>> _defaultGamepadBindings = new()
        {
            { InputAction.SaltWalkLeft, new List<Buttons> { Buttons.DPadLeft, Buttons.LeftThumbstickLeft } },
            { InputAction.SaltWalkRight, new List<Buttons> { Buttons.DPadRight, Buttons.LeftThumbstickRight } },
            { InputAction.SaltJump, new List<Buttons> { Buttons.A } },
            { InputAction.QuickQuit, new List<Buttons> { Buttons.B } }
        };
        #endregion

        #region Game Loop Methods
        public static void Initialize()
        {
            ResetBindingsToDefault();
            _gamepadState = GamePad.GetState(PlayerIndex.One);
            _previousGamepadState = _gamepadState;
        }

        public static void Update()
        {
            KeyboardExtended.Update();
            _previousGamepadState = _gamepadState;
            _gamepadState = GamePad.GetState(PlayerIndex.One);
        }
        #endregion

        #region Add Binding Methods
        public static void AddActionBinding(InputAction action, Keys button)
        {
            if (!_keyboardBindings[action].Contains(button))
            {
                _keyboardBindings[action].Add(button);
            }
        }

        public static void AddActionBinding(InputAction action, MouseButton button)
        {
            if (!_mouseBindings[action].Contains(button))
            {
                _mouseBindings[action].Add(button);
            }
        }

        public static void AddActionBinding(InputAction action, Buttons button)
        {
            if (!_gamepadBindings[action].Contains(button))
            {
                _gamepadBindings[action].Add(button);
            }
        }

        public static void AddActionBinding(InputAction action, IEnumerable<Keys> buttons)
        {
            foreach (Keys button in buttons)
            {
                AddActionBinding(action, button);
            }
        }
        public static void AddActionBinding(InputAction action, IEnumerable<MouseButton> buttons)
        {
            foreach (MouseButton button in buttons)
            {
                AddActionBinding(action, button);
            }
        }

        public static void AddActionBinding(InputAction action, IEnumerable<Buttons> buttons)
        {
            foreach (Buttons button in buttons)
            {
                AddActionBinding(action, button);
            }
        }

        public static void AddActionBinding(Dictionary<InputAction, List<Keys>> bindings)
        {
            foreach (InputAction action in bindings.Keys)
            {
                AddActionBinding(action, bindings[action]);
            }
        }

        public static void AddActionBinding(Dictionary<InputAction, List<MouseButton>> bindings)
        {
            foreach (InputAction action in bindings.Keys)
            {
                AddActionBinding(action, bindings[action]);
            }
        }
        public static void AddActionBinding(Dictionary<InputAction, List<Buttons>> bindings)
        {
            foreach (InputAction action in bindings.Keys)
            {
                AddActionBinding(action, bindings[action]);
            }
        }
        #endregion

        #region Methods

        public static void ResetBindingsToDefault()
        {
            AddActionBinding(_defaultKeyboardBindings);
            AddActionBinding(_defaultMouseBindings);
            AddActionBinding(_defaultGamepadBindings);
        }


        public static bool IsActionHeld(InputAction action)
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

        public static bool WasActionHeld(InputAction action)
        {
            // [todo]


            return false;
        }

        // [todo: add "WasActionJustPressed" and "WasActionJustReleased" methods]


        #endregion


        #region Helper Methods

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
        /// <returns>True if the given gamepad button is pressed; otherwise, false.</returns>
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

        private static bool IsGamepadTriggerDown(GamePadState state, Buttons button)
        {
            return button switch
            {
                Buttons.LeftTrigger => state.Triggers.Left > _gamepadDeadzone,
                Buttons.RightTrigger => state.Triggers.Right > _gamepadDeadzone,
                _ => false
            };
        }

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
