using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Handedness {
    Left,
    Right
}

public enum Gesture : int {
    Pick,
    Grab,
    GrabDown,
    GrabUp,
    Point,
    ThumbDown,
    HandFacingForward,
    HandFacingBackward,
    HandFacingUp,
    HandFacingDown,
    HandFacingInward,
    HandFacingOutward
}

public enum Finger {
    Thumb,
    IndexFinger,
    MiddleFinger,
    RingFinger,
    Pinky
}

public class ButtonState {
    public OVRInput.Controller controller;
    public OVRInput.Button button;
    public int burstTapped = 0;
    public bool burstTappedChanged = false;
    public double lastTap = 0.0f;

    public ButtonState (OVRInput.Button button, OVRInput.Controller controller) {
        this.button = button;
        this.controller = controller;
    }
}

public class OVRAdapter : MonoBehaviour {
    #region Variables

    public Handedness handedness = Handedness.Right;
    public OVRCameraRig cameraRig;
    public OvrAvatar avatar;
    public bool autoDiscover = true;

    public float buttonTapSensitivity = 0.2f;

    #endregion

    private Dictionary<OVRInput.Controller, Facing> handFacingCached;
    private bool stale = true;

    private Dictionary<KeyValuePair<OVRInput.Button, OVRInput.Controller>, int> buttonTapStatesIndex = new Dictionary<KeyValuePair<OVRInput.Button, OVRInput.Controller>, int> ();
    private ButtonState [] buttonTapStates;

    #region Setup

    private void Start () {
        if (autoDiscover) {
            AutoDiscover ();
        }
        Array buttons = Enum.GetValues (typeof (OVRInput.Button));
        OVRInput.Controller [] controllers = GetControllers ();
        buttonTapStates = new ButtonState [buttons.Length * controllers.Length];

        int i = 0;
        foreach (OVRInput.Button button in buttons) {
            foreach (OVRInput.Controller controller in controllers) {
                buttonTapStates [i] = new ButtonState (button, controller);
                buttonTapStatesIndex.Add (new KeyValuePair<OVRInput.Button, OVRInput.Controller> (button, controller), i);
                i += 1;
            }
        }
    }

    private void AutoDiscover () {
        if (!cameraRig) {
            GameObject crO = GameObject.Find ("OVRCameraRig");
            if (crO) {
                cameraRig = crO.GetComponent<OVRCameraRig> ();
            }
        }

        if (!avatar) {
            GameObject aO = GameObject.Find ("LocalAvatar");
            if (aO) {
                avatar = aO.GetComponent<OvrAvatar> ();
            }
        }
    }

    #endregion
    
    private void Update () {
        stale = true;

        UpdateButtonTapState ();
    }

    #region Logging

    public void LogGestures () {
        foreach (Gesture gesture in Enum.GetValues (typeof (Gesture))) {
            Debug.Log ("[" + gesture + "][DominantHand]: " + IsGesture (gesture, GetDominantHand ()));
            Debug.Log ("[" + gesture + "][OffHand]: " + IsGesture (gesture, GetOffHand ()));
        }
    }

    public void LogHandFacing () {
        Debug.Log ("DominantHand facing: " + DoGetHandFacing (GetDominantHand ()));
        Debug.Log ("OffHand facing: " + DoGetHandFacing (GetOffHand ()));
    }

    #endregion

    #region Controller Handling

    public OVRInput.Controller GetDominantHand () {
        if (handedness == Handedness.Left) {
            return OVRInput.Controller.LTouch;
        } else {
            return OVRInput.Controller.RTouch;
        }
    }

    public OVRInput.Controller GetOffHand () {
        if (handedness == Handedness.Left) {
            return OVRInput.Controller.RTouch;
        } else {
            return OVRInput.Controller.LTouch;
        }
    }

    public OVRInput.Controller GetLeftHand () {
        return OVRInput.Controller.LTouch;
    }

    public OVRInput.Controller GetRightHand () {
        return OVRInput.Controller.RTouch;
    }

    public OVRInput.Controller [] GetControllers () {
        return new OVRInput.Controller [2] { GetDominantHand (), GetOffHand () };
    }
    
    #endregion

    #region Gesture Handling
    
    public bool IsGesture (params Gesture [] gestures) {
        return IsGesture (gestures, GetDominantHand ());
    }

    public bool IsGesture (OVRInput.Controller controller, params Gesture [] gestures) {
        return IsGesture (gestures, GetDominantHand ());
    }

    public bool IsGesture (Gesture [] gestures, OVRInput.Controller controller) {
        bool isGesture = true;

        foreach (Gesture gesture in gestures) {
            isGesture &= IsGesture (gesture, controller);
        }

        return isGesture;
    }

    public bool IsGesture (Gesture gesture) {
        return IsGesture (gesture, GetDominantHand ());
    }

    public bool IsGesture (Gesture gesture, OVRInput.Controller controller) {
        if (false == OVRInput.IsControllerConnected (controller)) {
            return false;
        }

        bool inverted = false;
        if ((int) gesture < 0) {
            inverted = true;
            gesture = ~gesture;
        }

        bool result = false;

        switch (gesture) {
            case Gesture.Pick:
                result = IsPick (controller);
                break;
            case Gesture.Grab:
                result = IsGrab (controller);
                break;
            case Gesture.GrabDown:
                result = IsGrabDown (controller);
                break;
            case Gesture.GrabUp:
                result = IsGrabUp (controller);
                break;
            case Gesture.Point:
                result = IsPoint (controller);
                break;
            case Gesture.ThumbDown:
                result = IsThumbDown (controller);
                break;
            case Gesture.HandFacingForward:
                result = (GetHandFacing (controller) == Facing.Forwards);
                break;
            case Gesture.HandFacingBackward:
                result = (GetHandFacing (controller) == Facing.Backwards);
                break;
            case Gesture.HandFacingUp:
                result = (GetHandFacing (controller) == Facing.Up);
                break;
            case Gesture.HandFacingDown:
                result = (GetHandFacing (controller) == Facing.Down);
                break;
            case Gesture.HandFacingInward:
                result = (GetHandFacing (controller) == Facing.Inwards);
                break;
            case Gesture.HandFacingOutward:
                result = (GetHandFacing (controller) == Facing.Outwards);
                break;
        }

        if (inverted) {
            result = !result;
        }

        return result;
    }

    public bool IsPick (OVRInput.Controller controller) {
        return (IsThumbDown (controller) && OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger, controller) > 0.99f);
    }

    public bool IsGrab (OVRInput.Controller controller) {
        return (OVRInput.Get (OVRInput.Button.PrimaryHandTrigger, controller));
    }
    
    public bool IsGrabDown (OVRInput.Controller controller) {
        return (OVRInput.GetDown (OVRInput.Button.PrimaryHandTrigger, controller));
    }
    
    public bool IsGrabUp (OVRInput.Controller controller) {
        return (OVRInput.GetUp (OVRInput.Button.PrimaryHandTrigger, controller));
    }

    public bool IsPoint (OVRInput.Controller controller) {
        return (false == OVRInput.Get (OVRInput.NearTouch.PrimaryIndexTrigger, controller));
    }

    public bool IsThumbDown (OVRInput.Controller controller) {
        return (
            OVRInput.Get (OVRInput.Touch.One, controller)
            || OVRInput.Get (OVRInput.Touch.Two, controller)
            || OVRInput.Get (OVRInput.Touch.PrimaryThumbRest, controller)
        );
    }

    #endregion

    #region Buttontaps

    public void UpdateButtonTapState () {
        foreach (ButtonState buttonTapState in buttonTapStates) {
            buttonTapState.burstTappedChanged = false;
            if (OVRInput.GetDown (buttonTapState.button, buttonTapState.controller)) {
                buttonTapState.burstTapped += 1;
                buttonTapState.burstTappedChanged = true;
                buttonTapState.lastTap = Time.time;
            } else if (Time.time - buttonTapState.lastTap > buttonTapSensitivity) {
                buttonTapState.burstTapped = 0;
            }
        }
    }
    
    public int GetButtonTaps (OVRInput.Button button) {
        return GetButtonTaps (button, true);
    }

    public int GetButtonTaps (OVRInput.Button button, OVRInput.Controller controller) {
        return GetButtonTaps (button, true, controller);
    }

    public int GetButtonTaps (OVRInput.Button button, bool current) {
        return GetButtonTaps (button, current, GetDominantHand ());
    }

    public int GetButtonTaps (OVRInput.Button button, bool current, OVRInput.Controller controller) {
        ButtonState buttonState = GetButtonState (button, controller);

        if (buttonState != null && (current == false || buttonState.burstTappedChanged)) {
            return buttonState.burstTapped;
        }

        return 0;
    }

    public ButtonState GetButtonState (OVRInput.Button button, OVRInput.Controller controller) {
        int index;
        if (buttonTapStatesIndex.TryGetValue (new KeyValuePair<OVRInput.Button, OVRInput.Controller> (button, controller), out index)) {
            return buttonTapStates [index];
        }

        return null as ButtonState;
    }

    #endregion
    
    /**
     * Returns in which direction the inside of the hand is pointing
     */
    public Facing GetHandFacing (OVRInput.Controller controller) {
        if (stale || handFacingCached.ContainsKey (controller) == false) {
            handFacingCached [controller] = DoGetHandFacing (controller);
        }
        return handFacingCached [controller];
    }

    public Facing DoGetHandFacing (OVRInput.Controller controller) {
        if (false == OVRInput.IsControllerConnected (controller) || false == OVRInput.GetControllerOrientationTracked (controller)) {
            return Facing.Unknown;
        }

        return Helper.GetFacing (GetInwardVector (controller), controller == OVRInput.Controller.LTouch);
    }

    #region Controller Position, Rotation and Velocity

    public Vector3 GetForwardVector () {
        return GetForwardVector (GetDominantHand ());
    }

    public Vector3 GetForwardVector (OVRInput.Controller controller) {
        return GetRotatedVector3 (Vector3.forward, controller);
    }

    public Vector3 GetInwardVector () {
        return GetInwardVector (GetDominantHand ());
    }

    public Vector3 GetInwardVector (OVRInput.Controller controller) {
        switch (controller) {
            case OVRInput.Controller.LTouch:
                return GetRotatedVector3 (Vector3.right, controller);
            case OVRInput.Controller.RTouch:
                return GetRotatedVector3 (Vector3.left, controller);
        }
        return Vector3.zero;
    }

    public Vector3 GetPosition (bool worldSpace) {
        return GetPosition (worldSpace, GetDominantHand ());
    }

    public Vector3 GetPosition (bool worldSpace, OVRInput.Controller controller) {
        Vector3 position = GetPosition (controller);

        if (worldSpace && cameraRig) {
            position = cameraRig.trackingSpace.TransformPoint (position);
        }

        return position;
    }

    public Vector3 GetPosition () {
        return GetPosition (GetDominantHand ());
    }

    public Vector3 GetPosition (OVRInput.Controller controller) {
        return OVRInput.GetLocalControllerPosition (controller);
    }

    public Quaternion GetRotation (bool worldSpace) {
        return GetRotation (worldSpace, GetDominantHand ());
    }

    public Quaternion GetRotation (bool worldSpace, OVRInput.Controller controller) {
        Quaternion rotation = GetRotation (controller);

        if (worldSpace && cameraRig) {
            rotation = cameraRig.trackingSpace.rotation * rotation;
        }

        return rotation;
    }

    public Quaternion GetRotation () {
        return GetRotation (GetDominantHand ());
    }

    public Quaternion GetRotation (OVRInput.Controller controller) {
        return OVRInput.GetLocalControllerRotation (controller);
    }

    public Vector3 GetRotatedVector3 (Vector3 vector) {
        return GetRotatedVector3 (vector, GetDominantHand ());
    }

    public Vector3 GetRotatedVector3 (Vector3 vector, OVRInput.Controller controller) {
        return GetRotation (true, controller) * vector;
    }

    public Vector3 GetVelocity (bool local = false) {
        return GetVelocity (GetDominantHand (), local);
    }

    public Vector3 GetVelocity (OVRInput.Controller controller, bool local = false) {
        Vector3 velocity = OVRInput.GetLocalControllerVelocity (controller);

        if (false == local) {
            velocity = cameraRig.trackingSpace.TransformDirection (velocity);
        }

        return velocity;
    }

    public Vector3 GetAcceleration () {
        return GetAcceleration (GetDominantHand ());
    }

    public Vector3 GetAcceleration (OVRInput.Controller controller) {
        return OVRInput.GetLocalControllerAcceleration (controller);
    }

    public Vector3 GetAngularVelocity () {
        return GetAngularVelocity (GetDominantHand ());
    }

    public Vector3 GetAngularVelocity (OVRInput.Controller controller) {
        return OVRInput.GetLocalControllerAngularVelocity (controller);
    }

    public Vector3 GetAngularAcceleration () {
        return GetAngularAcceleration (GetDominantHand ());
    }

    public Vector3 GetAngularAcceleration (OVRInput.Controller controller) {
        return OVRInput.GetLocalControllerAngularAcceleration (controller);
    }
    
    #endregion

    #region OVRAvatar Finger Handling

    public Transform GetFingerPart (Finger finger, int joint) {
        return GetFingerPart (finger, joint, GetDominantHand ());
    }

    public Transform GetFingerPart (Finger finger) {
        return GetFingerPart (finger, 4);
    }

    public Transform GetFingerPart (Finger finger, OVRInput.Controller controller) {
        return GetFingerPart (finger, 4, controller);
    }

    public Transform GetFingerPart (Finger finger, int joint, OVRInput.Controller controller) {
        bool valid = (
            (
                avatar
            ) && (
                controller == OVRInput.Controller.LTouch
                || controller == OVRInput.Controller.RTouch
            ) && (
                (joint > 0 && joint <= 4)
                || (finger == Finger.Pinky && joint == 0)
            )
        );

        if (valid) {
            string fingerName = FingerToName (finger);
            string jointName = JointToName (joint);

            if (fingerName != null && jointName != null) {
                string hand = "r";
                if (controller == OVRInput.Controller.RTouch) {
                    hand = "r";
                } else if (controller == OVRInput.Controller.LTouch) {
                    hand = "l";
                }

                GameObject partGo = GameObject.Find (String.Format ("hands:b_{0}_{1}{2}", hand, fingerName, jointName));
                if (partGo) {
                    return partGo.transform;
                }
            }

        }

        return null as Transform;
    }

    private string FingerToName (Finger finger) {
        switch (finger) {
            case Finger.Thumb:
                return "thumb";
            case Finger.IndexFinger:
                return "index";
            case Finger.MiddleFinger:
                return "middle";
            case Finger.RingFinger:
                return "ring";
            case Finger.Pinky:
                return "pinky";
        }
        return null as string;
    }

    private string JointToName (int joint) {
        if (joint == 4) {
            return "_ignore";
        } else if (joint >= 0 && joint < 4) {
            return joint.ToString ();
        }
        return null as string;
    }

    #endregion
}
