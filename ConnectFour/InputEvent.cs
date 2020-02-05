using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectFour
{
    public delegate void InputEventHandler(object source, InputEventArgs e);

    public class InputEventArgs : EventArgs
    {
        private int EventInfo;
        public InputEventArgs(int Value)
        {
            EventInfo = Value;
        }
        public int GetInfo()
        {
            return EventInfo;
        }
    }

    public class InputEvent
    {
        public event InputEventHandler OnInput;
        public int InputValue
        {
            set
            {
                if (OnInput != null)
                {
                    OnInput(this, new InputEventArgs(value));
                }
            }
        }
    }
}
