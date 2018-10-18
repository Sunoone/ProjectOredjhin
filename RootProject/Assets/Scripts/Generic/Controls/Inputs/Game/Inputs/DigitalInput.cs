using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
    [System.Serializable]
    public class DigitalInput : ICloneable<DigitalInput>
    {
        public virtual DigitalInput Clone()
        {
            DigitalInput newInput = new DigitalInput();
            newInput.InputKey = InputKey;
            return newInput;
        }
        public virtual bool IsSimulated() { return false; }

        protected bool Pressed = false;
        public KeyCode InputKey;
  
        public virtual bool GetInputDown() { return Input.GetKeyDown(InputKey); }
        public virtual bool GetInputUp() { return Input.GetKeyUp(InputKey); }
        public virtual bool GetInput() { return Input.GetKey(InputKey); }

        public virtual void Switch(bool condition)
        {
            Pressed = condition;
            if (Pressed)
            {
                Press();
                return;
            }
            Release();
        }
        public virtual void Press()
        {
            lastPressTime = Time.time;
        }
        public virtual void Release()
        {
            //Debug.Log("elapsed time: " + (Time.time - lastPressTime));
        }
        protected float lastPressTime;
    }
    [System.Serializable]
    public class SimulatedDigitalInput : DigitalInput
    {
        public override DigitalInput Clone()
        {
            DigitalInput newInput = new DigitalInput();
            newInput.InputKey = InputKey;
            return newInput;
        }
        public override bool IsSimulated() { return true; }

        public string InputString;

        public override bool GetInputDown()
        {
            return base.GetInputDown();
        }

        public override bool GetInputUp()
        {
            return base.GetInputUp();
        }

        public override bool GetInput()
        {
            return base.GetInput();
        }
    }
}

