using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using Celeste.Core;

namespace Celeste.Core.Input
{
    /// <summary>
    /// Gerenciador de entrada unificado para Desktop e Android
    /// </summary>
    public class InputManager
    {
        private readonly ILogSystem _logger;
        private KeyboardState _currentKeyboard;
        private KeyboardState _previousKeyboard;
        private GamePadState _currentGamePad;
        private GamePadState _previousGamePad;
        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private TouchInput _touchInput;
        private bool _isAndroid;

        // Estados de botão abstrato
        private ButtonState _aButtonState;
        private ButtonState _bButtonState;
        private ButtonState _upState;
        private ButtonState _downState;
        private ButtonState _leftState;
        private ButtonState _rightState;

        public InputManager(ILogSystem logger, bool isAndroid)
        {
            _logger = logger;
            _isAndroid = isAndroid;

            if (isAndroid)
            {
                _touchInput = new TouchInput();
                _logger.Log("[Input] Inicializado para ANDROID com Touch");
            }
            else
            {
                _logger.Log("[Input] Inicializado para DESKTOP com Teclado");
            }
        }

        public void Update()
        {
            _previousKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();
            _previousGamePad = _currentGamePad;
            _currentGamePad = GamePad.GetState(PlayerIndex.One);
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            if (_isAndroid && _touchInput != null)
            {
                _touchInput.Update();
                ProcessTouchInput();
            }
            else
            {
                ProcessKeyboardInput();
                ProcessGamePadInput();
                ProcessMouseInput();
            }
        }

        private void ProcessKeyboardInput()
        {
            _aButtonState = _currentKeyboard.IsKeyDown(Keys.Z) ? ButtonState.Pressed : ButtonState.Released;
            _bButtonState = _currentKeyboard.IsKeyDown(Keys.X) ? ButtonState.Pressed : ButtonState.Released;
            _upState = _currentKeyboard.IsKeyDown(Keys.Up) || _currentKeyboard.IsKeyDown(Keys.W) ? ButtonState.Pressed : ButtonState.Released;
            _downState = _currentKeyboard.IsKeyDown(Keys.Down) || _currentKeyboard.IsKeyDown(Keys.S) ? ButtonState.Pressed : ButtonState.Released;
            _leftState = _currentKeyboard.IsKeyDown(Keys.Left) || _currentKeyboard.IsKeyDown(Keys.A) ? ButtonState.Pressed : ButtonState.Released;
            _rightState = _currentKeyboard.IsKeyDown(Keys.Right) || _currentKeyboard.IsKeyDown(Keys.D) ? ButtonState.Pressed : ButtonState.Released;
        }

        private void ProcessGamePadInput()
        {
            // GamePad buttons (Xbox/DualShock controller)
            _aButtonState = _currentGamePad.Buttons.A == ButtonState.Pressed ? ButtonState.Pressed : ButtonState.Released;
            _bButtonState = _currentGamePad.Buttons.B == ButtonState.Pressed ? ButtonState.Pressed : ButtonState.Released;

            // DPad ou thumbstick
            var dPadUp = _currentGamePad.DPad.Up == ButtonState.Pressed;
            var dPadDown = _currentGamePad.DPad.Down == ButtonState.Pressed;
            var dPadLeft = _currentGamePad.DPad.Left == ButtonState.Pressed;
            var dPadRight = _currentGamePad.DPad.Right == ButtonState.Pressed;

            var thumbstickUp = _currentGamePad.ThumbSticks.Left.Y > 0.5f;
            var thumbstickDown = _currentGamePad.ThumbSticks.Left.Y < -0.5f;
            var thumbstickLeft = _currentGamePad.ThumbSticks.Left.X < -0.5f;
            var thumbstickRight = _currentGamePad.ThumbSticks.Left.X > 0.5f;

            _upState = (dPadUp || thumbstickUp) ? ButtonState.Pressed : ButtonState.Released;
            _downState = (dPadDown || thumbstickDown) ? ButtonState.Pressed : ButtonState.Released;
            _leftState = (dPadLeft || thumbstickLeft) ? ButtonState.Pressed : ButtonState.Released;
            _rightState = (dPadRight || thumbstickRight) ? ButtonState.Pressed : ButtonState.Released;
        }

        private void ProcessMouseInput()
        {
            // Mouse esquerdo = jump/confirm (A button)
            _aButtonState = _currentMouse.LeftButton == ButtonState.Pressed ? ButtonState.Pressed : ButtonState.Released;
            
            // Mouse direito = dash (B button)
            _bButtonState = _currentMouse.RightButton == ButtonState.Pressed ? ButtonState.Pressed : ButtonState.Released;

            // Movimento do mouse em quadrantes (aproximado para direção)
            // Essa é uma simplificação - pode ser expandida com cursores em áreas específicas
        }

        private void ProcessTouchInput()
        {
            if (_touchInput == null) return;

            VirtualButtonAction actions = _touchInput.GetTouchInput();

            _aButtonState = (actions & VirtualButtonAction.A) != 0 ? ButtonState.Pressed : ButtonState.Released;
            _bButtonState = (actions & VirtualButtonAction.B) != 0 ? ButtonState.Pressed : ButtonState.Released;
            _upState = (actions & VirtualButtonAction.Up) != 0 ? ButtonState.Pressed : ButtonState.Released;
            _downState = (actions & VirtualButtonAction.Down) != 0 ? ButtonState.Pressed : ButtonState.Released;
            _leftState = (actions & VirtualButtonAction.Left) != 0 ? ButtonState.Pressed : ButtonState.Released;
            _rightState = (actions & VirtualButtonAction.Right) != 0 ? ButtonState.Pressed : ButtonState.Released;
        }

        // Propriedades de estado de botão
        public bool IsJumpPressed => _aButtonState == ButtonState.Pressed;
        public bool IsJumpReleased => _previousKeyboard.IsKeyDown(Keys.Z) && _currentKeyboard.IsKeyUp(Keys.Z);

        public bool IsDashPressed => _bButtonState == ButtonState.Pressed;
        public bool IsLeftPressed => _leftState == ButtonState.Pressed;
        public bool IsRightPressed => _rightState == ButtonState.Pressed;
        public bool IsUpPressed => _upState == ButtonState.Pressed;
        public bool IsDownPressed => _downState == ButtonState.Pressed;

        public float GetHorizontalInput()
        {
            float horizontal = 0;
            if (IsLeftPressed) horizontal -= 1;
            if (IsRightPressed) horizontal += 1;
            return horizontal;
        }

        public float GetVerticalInput()
        {
            float vertical = 0;
            if (IsUpPressed) vertical -= 1;
            if (IsDownPressed) vertical += 1;
            return vertical;
        }

        public TouchInput GetTouchInput() => _touchInput;

        /// <summary>
        /// Posição do mouse (Desktop) ou nula (Android)
        /// </summary>
        public Vector2? GetMousePosition() => _isAndroid ? null : new Vector2(_currentMouse.X, _currentMouse.Y);

        /// <summary>
        /// Verificar se mouse esquerdo foi clicado (transição Released → Pressed)
        /// </summary>
        public bool IsMouseLeftClicked() => _previousMouse.LeftButton == ButtonState.Released && _currentMouse.LeftButton == ButtonState.Pressed;

        /// <summary>
        /// Verificar se gamepad está conectado
        /// </summary>
        public bool IsGamePadConnected() => _currentGamePad.IsConnected;

        /// <summary>
        /// Acessar diretamente o estado do gamepad
        /// </summary>
        public GamePadState GetGamePadState() => _currentGamePad;
    }
}
