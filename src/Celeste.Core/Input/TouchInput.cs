using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

namespace Celeste.Core.Input
{
    /// <summary>
    /// Sistema de entrada por toque para Android
    /// </summary>
    public class TouchInput
    {
        private List<TouchLocation> _currentTouches = new();
        private List<TouchLocation> _previousTouches = new();
        private List<VirtualButton> _buttons = new();

        public const int ScreenPadding = 20;
        public const int ButtonSize = 80;

        public TouchInput()
        {
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            // Botão A (inferior direita)
            _buttons.Add(new VirtualButton 
            { 
                Name = "A", 
                Bounds = new Rectangle(1200 - ButtonSize - ScreenPadding, 720 - ButtonSize - ScreenPadding, ButtonSize, ButtonSize),
                Action = VirtualButtonAction.A
            });

            // Botão B (esquerda de A)
            _buttons.Add(new VirtualButton 
            { 
                Name = "B", 
                Bounds = new Rectangle(1200 - (ButtonSize * 2) - ScreenPadding - 10, 720 - ButtonSize - ScreenPadding, ButtonSize, ButtonSize),
                Action = VirtualButtonAction.B
            });

            // D-Pad (inferior esquerda)
            int dpadLeft = ScreenPadding + 20;
            int dpadTop = 720 - (ButtonSize * 2) - ScreenPadding;

            _buttons.Add(new VirtualButton 
            { 
                Name = "Up", 
                Bounds = new Rectangle(dpadLeft + ButtonSize / 2 - 20, dpadTop, 40, 40),
                Action = VirtualButtonAction.Up
            });

            _buttons.Add(new VirtualButton 
            { 
                Name = "Down", 
                Bounds = new Rectangle(dpadLeft + ButtonSize / 2 - 20, dpadTop + ButtonSize - 40, 40, 40),
                Action = VirtualButtonAction.Down
            });

            _buttons.Add(new VirtualButton 
            { 
                Name = "Left", 
                Bounds = new Rectangle(dpadLeft, dpadTop + ButtonSize / 2 - 20, 40, 40),
                Action = VirtualButtonAction.Left
            });

            _buttons.Add(new VirtualButton 
            { 
                Name = "Right", 
                Bounds = new Rectangle(dpadLeft + ButtonSize - 40, dpadTop + ButtonSize / 2 - 20, 40, 40),
                Action = VirtualButtonAction.Right
            });
        }

        public void Update()
        {
            _previousTouches.Clear();
            _previousTouches.AddRange(_currentTouches);
            _currentTouches.Clear();

            if (TouchPanel.IsGestureAvailable)
            {
                // Processar gestos se necessário
            }

            var touchCollection = TouchPanel.GetState();
            foreach (var touch in touchCollection)
            {
                if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved)
                {
                    _currentTouches.Add(touch);
                }
            }
        }

        public VirtualButtonAction GetTouchInput()
        {
            VirtualButtonAction result = VirtualButtonAction.None;

            foreach (var touch in _currentTouches)
            {
                Vector2 pos = touch.Position;
                
                foreach (var button in _buttons)
                {
                    if (button.Bounds.Contains((int)pos.X, (int)pos.Y))
                    {
                        result |= button.Action;
                    }
                }
            }

            return result;
        }

        public List<VirtualButton> GetButtons() => _buttons;
        public List<TouchLocation> GetCurrentTouches() => _currentTouches;
        public List<TouchLocation> GetPreviousTouches() => _previousTouches;
    }

    [Flags]
    public enum VirtualButtonAction
    {
        None = 0,
        A = 1,
        B = 2,
        Up = 4,
        Down = 8,
        Left = 16,
        Right = 32,
        Start = 64,
        Select = 128
    }

    public class VirtualButton
    {
        public string Name { get; set; }
        public Rectangle Bounds { get; set; }
        public VirtualButtonAction Action { get; set; }
    }
}
