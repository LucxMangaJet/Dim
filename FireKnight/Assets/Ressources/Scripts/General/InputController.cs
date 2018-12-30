using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim
{

    /////////////////////////////////////////////////
    /// Singleton, translates Keyboard Keys to Ingame Controls, used by other Classes for getting Input.
    /////////////////////////////////////////////////
    public class InputController
    {   
        private static InputController instance;

        private IRangeInput horizontalAxis;
        private IBoolInput jump;
        private IBoolInput sprint;
        private IBoolInput releaseEnergy;
        private IBoolInput crouch;
        private IBoolInput escape;
        private IBoolInput absorbEnergy;

        private InputController()
        {

            horizontalAxis = new PCRangeInput("Horizontal");
            jump = new PCInput(KeyCode.Space);
            sprint = new PCInput(KeyCode.LeftShift);
            releaseEnergy = new PCInput(KeyCode.E);
            crouch = new PCInput(KeyCode.S);
            escape = new PCInput(KeyCode.Escape);
            absorbEnergy = new PCInput(KeyCode.Q);
        }


        public static void Initiate()
        {
            instance = new InputController();
        }

        public static void Update()
        {
            instance.jump.Update();
            instance.sprint.Update();
            instance.releaseEnergy.Update();
            instance.crouch.Update();
            instance.escape.Update();
            instance.absorbEnergy.Update();

        }

        public static float GetHorizontalInput()
        {
            return instance.horizontalAxis.GetState();
        }

        public static bool GetJump(InputStateType type)
        {
            return instance.jump.GetState(type);
        }

        public static bool GetSprint(InputStateType type)
        {
            return instance.sprint.GetState(type);
        }

        public static bool GetReleaseEnergy(InputStateType type)
        {
            return instance.releaseEnergy.GetState(type);
        }

        public static bool GetCrouch(InputStateType type)
        {
            return instance.crouch.GetState(type);
        }

        public static bool GetEscape(InputStateType type)
        {
            return instance.escape.GetState(type);
        }

        public static bool GetAbsorbEnergy(InputStateType type)
        {
            return instance.absorbEnergy.GetState(type);
        }

    }

    /////////////////////////////////////////////////
    /// Used by the InputController, represents a form of Input that has 2 states, like a Keyboard key.
    /////////////////////////////////////////////////
    public interface IBoolInput
    {
        void Update();
        bool GetState(InputStateType type);
    }

    /////////////////////////////////////////////////
    /// Used by the InputController, represents a form of Input that has a range, like a Controllers stick.
    /////////////////////////////////////////////////
    public interface IRangeInput
    {
        float GetState();
    }

    /////////////////////////////////////////////////
    /// Used by the InputController, implements a IRangeInput from a keyboard.
    /////////////////////////////////////////////////
    public class PCRangeInput : IRangeInput
    {
        private string axisName;
        private float value;

        public PCRangeInput(string _axis)
        {
            axisName = _axis;
        }

        private void Update()
        {
            value = Input.GetAxis(axisName);
        }

        public float GetState()
        {
            Update();
            return value;
        }

        
    }

    /////////////////////////////////////////////////
    /// Used by the InputController, implements a IBoolInput from a keyboard.
    /////////////////////////////////////////////////
    public class PCInput : IBoolInput
    {
        private bool isKey;
        private KeyCode key;
        private string inputName;
        private bool pressed, justPressed, justReleased;
        private bool lastState;

        public PCInput(KeyCode _key)
        {
            isKey = true;
            key = _key;
        }

        public PCInput(string _inputName)
        {
            isKey = false;
            inputName = _inputName;
        }

        public void Update()
        {
            bool currentState;

            if (isKey)
            {
                currentState = Input.GetKey(key);
            }
            else
            {
                currentState = Input.GetButton(inputName);
            }

            pressed = currentState;

            if (!lastState && currentState)
            {
                justPressed = true;
            }
            else
            {
                justPressed = false;
            }

            if (lastState && !currentState)
            {
                justReleased = true;
            }
            else
            {
                justReleased = false;
            }

            lastState = currentState;
        }


        public bool GetState(InputStateType type)
        {
            switch (type)
            {
                case InputStateType.PRESSED:
                    return pressed;
                case InputStateType.JUST_PRESSED:
                    return justPressed;
                case InputStateType.JUST_RELESED:
                    return justReleased;
            }

            throw new System.Exception("INPUT ERROR: Unexpected result: " + type.ToString() );
        }

    }

    /////////////////////////////////////////////////
    /// [UNIMPLEMENTED] Used by the InputController, implements a IRangeInput from a touchScreen.
    /////////////////////////////////////////////////
    public class MoblieRangeInput : IRangeInput
    {
        private float value;
        

        public MoblieRangeInput()
        {
        }

        private void Update()
        {
            //value = ???
        }

        public float GetState()
        {
            Update();
            return value;
        }
    }



    /////////////////////////////////////////////////
    /// Used by the InputController, represents the 3 states of Bool inputs.
    /////////////////////////////////////////////////
    public enum InputStateType
    {
        PRESSED,
        JUST_PRESSED,
        JUST_RELESED
    }


}


