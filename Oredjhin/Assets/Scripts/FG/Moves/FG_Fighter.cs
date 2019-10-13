using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freethware.Inputs;

public class FG_Fighter : MonoBehaviour
{
    public FG_Move CurrentMove;

    public Vector2 CalibratedCenter;

    public InputManager IM;

    [ReadOnly]
    public ButtonProfile Profile;

    public Controls_DirectionUnit DirectionDownBackUnit;
    public Controls_DirectionUnit DirectionDownUnit;
    public Controls_DirectionUnit DirectionDownForwardUnit;
    public Controls_DirectionUnit DirectionBackUnit;
    public Controls_DirectionUnit DirectionNeutralUnit;
    public Controls_DirectionUnit DirectionForwardUnit;
    public Controls_DirectionUnit DirectionUpBackUnit;
    public Controls_DirectionUnit DirectionUpUnit;
    public Controls_DirectionUnit DirectionUpForwardUnit;

    public List<Controls_ButtonUnit> ButtonUnits;
    [ReadOnly] public List<Controls_ButtonUnit> buttonUnits;

    public int StickCount = 1;
    public int ButtonCount = 8;

    private bool init;
    private void Init()
    {
        if (!init)
        {
            Profile = IM.DefaultProfiles[0];

            int length = ButtonUnits.Count;
            Debug.Log("Length: " + length);    
            for (int i = 0; i < length; i++)
            {
                buttonUnits.Add(Instantiate(ButtonUnits[i]));
            }

            ButtonCount = length;
            StickCount = 1;
            init = true;
        }
    }

    private void OnEnable()
    {
        Init();
        if (Profile == null)
        {
            Debug.LogError("No control scheme setup, disabling the character");
            gameObject.SetActive(false);
            return;
        }

        if(CurrentMove == null)
        {
            Debug.LogError("No initial move setup, disabling the character");
            gameObject.SetActive(false);
            return;
        }


        if (!DirectionUpBackUnit || !DirectionUpUnit || !DirectionUpForwardUnit
            || !DirectionBackUnit || !DirectionNeutralUnit || !DirectionForwardUnit
            || !DirectionDownBackUnit || !DirectionDownUnit || !DirectionDownForwardUnit)
        {
            Debug.LogError("Missing at least one directional unit, disabling the character.");
            gameObject.SetActive(false);
            return;
        }

        /*int length = System.Enum.GetNames(typeof(Button)).Length - 4; // The 4 movement inputs are optional
        if (ButtonUnits.Count < length)
        {
            Debug.LogError("Not enough button entries, disabling the character.");
            gameObject.SetActive(false);
            return;
        }*/

        int length = buttonUnits.Count;
        for (int i = 0; i < length; i++)
        {
            if (!buttonUnits[i])
            {
                Debug.LogError("Found a NULL buttonUnit entry, disabling the character.");
                gameObject.SetActive(false);
                return;
            }
        }
    }


    private void Update()
    {
        Profile.RunInput();
        AddAxisToStream(Profile.GetAxis());
        AddButtonsToStream();
        UpdateTimeStamps();

        if (!LinkMove())
            TimeInCurrentMove++;
    }

    [ReadOnly]
    public List<InputUnit> InputStream = new List<InputUnit>();
    [ReadOnly]
    public List<float> InputTimeStamps = new List<float>();

    public float WalkSpeed = 1;
    public const float DirectionThreshold = 0f; // Should be 0 if you're using the Unity build-in deadzone for the axis.
    private void AddAxisToStream(Vector2 directionInput)
    {
        Controls_DirectionUnit directionUnit = GetDirectionUnitForAxis(Profile.GetAxis());
        InputStream.Add(directionUnit);
    }
    private Controls_DirectionUnit GetDirectionUnitForAxis(Vector2 directionInput)
    {
        Controls_DirectionUnit directionUnit = null;

        if (directionInput.x < -DirectionThreshold)
        {
            if (directionInput.y < -DirectionThreshold)
                directionUnit = DirectionUpBackUnit;
            else if (directionInput.y > DirectionThreshold)
                directionUnit = DirectionDownBackUnit;
            else
                directionUnit = DirectionBackUnit;
        }
        else if (directionInput.x > DirectionThreshold)
        {
            if (directionInput.y < -DirectionThreshold)
                directionUnit = DirectionUpForwardUnit;
            else if (directionInput.y > DirectionThreshold)
                directionUnit = DirectionDownForwardUnit;
            else
                directionUnit = DirectionForwardUnit;
        }
        else
        {
            if (directionInput.y < -DirectionThreshold)
                directionUnit = DirectionUpUnit;
            else if (directionInput.y > DirectionThreshold)
                directionUnit = DirectionDownUnit;
            else
                directionUnit = DirectionNeutralUnit;
        }
        //Debug.Log("Direction: " + directionUnit.ToString());
        return directionUnit;
    }

    private void AddButtonsToStream()
    {
        for (int i = 0; i < ButtonCount; i++)
        {
            DigitalButton button = Profile.GetButton(ButtonUnits[i]);
            InputState buttonState = button.GetInputState();
            buttonUnits[i].InputState = buttonState;
            InputStream.Add(buttonUnits[i]);
        }
    }

    private void UpdateTimeStamps()
    {
        float CurrentTime = Time.frameCount;
        InputTimeStamps.Add(CurrentTime);

        // Something something, ring buffer, look it up
        int length = InputTimeStamps.Count;
        for (int i = 0; i < length; i++)
        {
            if ((InputTimeStamps[i] + InputExpirationTime < CurrentTime))
            {
                if (i > 0)
                {
                    InputTimeStamps.RemoveRange(0, i);
                    InputStream.RemoveRange(0, i * ButtonCount + StickCount);
                    break;
                }             
            }          
        }
    }

    // Restructure to read last input first. Inverse the system.
    private bool LinkMove()
    {
        FG_MoveLinkToFollow MoveLinkToFollow = CurrentMove.TryLinks(this, InputStream);
        //if (MoveLinkToFollow != null) 
        //    Debug.Log("MoveLinkToFollow: " + MoveLinkToFollow.Link.name);
        if (MoveLinkToFollow != null && MoveLinkToFollow.SMR.CompletionType == StateMachine.EStateMachineCompletionType.Accepted)
        {
            //Debug.Log("Switch to state: " + MoveLinkToFollow.Link.Move.MoveName);
            if (MoveLinkToFollow.Link.ClearInput || MoveLinkToFollow.Link.Move.ClearInputOnEntry || CurrentMove.ClearInputOnExit)
            {
                InputTimeStamps.Clear();
                InputStream.Clear();
            }
            else if (MoveLinkToFollow.SMR.DataIndex > 0)
            {
                // FG_State.StickCount can be calculated using the Profile.
                int inputOptions = ButtonCount + StickCount;
                if (MoveLinkToFollow.SMR.DataIndex % inputOptions == 0)
                {
                    InputTimeStamps.RemoveRange(0, MoveLinkToFollow.SMR.DataIndex / inputOptions);
                    InputStream.RemoveRange(0, MoveLinkToFollow.SMR.DataIndex);
                }
            }
            CurrentMove = MoveLinkToFollow.Link.Move;
            TimeInCurrentMove = 0.0f;
            //Debug.LogWarning("Attempting to do move: " + ((CurrentMove == null) ? "NULL" :  CurrentMove.MoveName.ToString()));
            DoMove(CurrentMove);
            return true;
        }
        return false;
    }

    private void DoMove(FG_Move move)
    {
        if (move == null)
        {
            Debug.LogError("No move found");
            return;
        }
        Debug.Log("Trying to execute: " + move.MoveName);
    }


    float TimeInCurrentMove = 0;
    public float GetTimeInMove()
    {
        return TimeInCurrentMove;
    }



    public float InputExpirationTime = 60f;
}
