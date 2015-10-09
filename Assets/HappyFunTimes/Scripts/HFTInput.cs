using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent (typeof (HFTGamepad))]
public class HFTInput : MonoBehaviour
{
    public class Touch {
        public Vector2 deltaPosition = new Vector2();
        public float deltaTime = 0;
        public int fingerId = 0;
        public TouchPhase phase = TouchPhase.Canceled;
        public Vector2 rawPosition = new Vector2();
        public int tapCount = 0;
    }

    public HFTInput()
    {
        for (int ii = 0; ii < m_touches.Length; ++ii)
        {
            m_touches[ii] = new Touch();
        }

        SpecifyAxisNameToAxisIndex("Horizontal", 0);
        SpecifyAxisNameToAxisIndex("Vertical", 1);
        SpecifyAxisNameToAxisIndex("Horizontal2", 2);
        SpecifyAxisNameToAxisIndex("Vertical3", 3);

        SpecifyButtonNameToButtonIndex("Fire1", 0);
        SpecifyButtonNameToButtonIndex("Fire2", 1);
    }

    void Awake()
    {
        m_gamepad = GetComponent<HFTGamepad>();

        m_buttonState = new bool[m_gamepad.buttons.Length];
        m_lastButtonState = new bool[m_gamepad.buttons.Length];
        m_lastPosition = new float[m_gamepad.axes.Length];
    }

    //Last measured linear acceleration of a device in three-dimensional space. (Read Only)
    public Vector3 acceleration
    {
        get { return m_gyro.userAcceleration; }
    }

    //Number of acceleration measurements which occurred during last frame.
    public int accelerationEventCount
    {
        get { return m_accelerationEventCount; }
    }

    //Returns list of acceleration measurements which occurred during the last frame. (Read Only) (Allocates temporary variables).
    public AccelerationEvent[] accelerationEvents
    {
        get { return null; }// TODO
    }

    //Is any key or mouse button currently held down? (Read Only)
    public bool anyKey
    {
        get { return m_anyKey; }
    }

    //Returns true the first frame the user hits any key or mouse button. (Read Only)
    public bool anyKeyDown
    {
        get { return m_anyKeyDown; }
    }

    //Property for accessing compass (handheld devices only). (Read Only)
    public Compass compass
    {
        get { return m_compass; }
    }

    //This property controls if input sensors should be compensated for screen orientation.
    public bool compensateSensors
    {
        get { return false; }
        set { Debug.Log("compensateSensors not yet implemented"); }
    }

    //The current text input position used by IMEs to open windows.
    public Vector2 compositionCursorPos
    {
        get { return m_compositionCursorPos; }
    }

    //The current IME composition string being typed by the user.
    public string compositionString
    {
        get { return ""; }
    }

    //Device physical orientation as reported by OS. (Read Only)
    public DeviceOrientation deviceOrientation
    {
        get { return m_deviceOrientation; }
    }

    //Returns default gyroscope.
    public HFTGyroscope gyro
    {
        get { return m_gyro; }
    }

    //Controls enabling and disabling of IME input composition.
    [System.NonSerialized]
    public IMECompositionMode imeCompositionMode = IMECompositionMode.Auto;

    //Does the user have an IME keyboard input source selected?
    public bool imeIsSelected
    {
        get { return false; }
    }

    //Returns the keyboard input entered this frame. (Read Only)
    public string inputString
    {
        get { return ""; }
    }

    //Property for accessing device location (handheld devices only). (Read Only)
    public LocationService location
    {
        get { return m_location; }
    }

    //The current mouse position in pixel coordinates. (Read Only)
    public Vector3 mousePosition
    {
        get { return m_mousePosition; }
    }

    //The current mouse scroll delta. (Read Only)
    public Vector2 mouseScrollDelta
    {
        get { return m_mouseScrollDelta; }
    }

    //Property indicating whether the system handles multiple touches.
    public bool multiTouchEnabled
    {
        get { return true; }
    }

    //Enables/Disables mouse simulation with touches. By default this option is enabled.
    public bool simulateMouseWithTouches
    {
        get { return true; }
    }

    //Number of touches. Guaranteed not to change throughout the frame. (Read Only)
    public int touchCount
    {
        get { return m_touches.Length; }
    }

    //Returns list of objects representing status of all touches during last frame. (Read Only) (Allocates temporary variables).
    public Touch[] touches
    {
        get { return m_touches; }
    }

    //Returns whether the device on which application is currently running supports touch input.
    public bool touchSupported
    {
        get { return true; }
    }

    // Returns specific acceleration measurement which occurred during last frame. (Does not allocate temporary variables).
    public AccelerationEvent GetAccelerationEvent(int index)
    {
        return new AccelerationEvent(); // TODO
    }

    // Returns the value of the virtual axis identified by axisName.
    public float GetAxis(string axisName)
    {
        int axisIndex = -1;
        float value = 0;
        m_axisMap.TryGetValue(axisName, out axisIndex);
        if (axisIndex >= 0 && axisIndex < m_gamepad.axes.Length) {
            value = m_gamepad.axes[axisIndex];
        } else {
            Debug.LogError("Unknown axis:" + axisName);
        }
        return value;
    }

    // Returns the value of the virtual axis identified by axisName with no smoothing filtering applied.
    public float GetAxisRaw(string axisName)
    {
        return GetAxis(axisName);
    }

    // Returns true while the virtual button identified by buttonName is held down.
    public bool GetButton(string buttonName)
    {
        int buttonIndex = -1;
        bool value = false;
        m_buttonMap.TryGetValue(buttonName, out buttonIndex);
        if (buttonIndex >= 0 && buttonIndex < m_buttonState.Length) {
            value = m_buttonState[buttonIndex];
        } else {
            Debug.LogError("Unknown button:" + buttonName);
        }
        return value;
    }

    // Returns true during the frame the user pressed down the virtual button identified by buttonName.
    public bool GetButtonDown(string buttonName)
    {
        int buttonIndex = -1;
        bool value = false;
        m_buttonMap.TryGetValue(buttonName, out buttonIndex);
        if (buttonIndex >= 0 && buttonIndex < m_buttonState.Length) {
            value = m_buttonState[buttonIndex] && !m_lastButtonState[buttonIndex];
        } else {
            Debug.LogError("Unknown button:" + buttonName);
        }
        return value;
    }

    // Returns true the first frame the user releases the virtual button identified by buttonName.
    public bool GetButtonUp(string buttonName)
    {
        int buttonIndex = -1;
        bool value = false;
        m_buttonMap.TryGetValue(buttonName, out buttonIndex);
        if (buttonIndex >= 0 && buttonIndex < m_buttonState.Length) {
            value = !m_buttonState[buttonIndex] && m_lastButtonState[buttonIndex];
        } else {
            Debug.LogError("Unknown button:" + buttonName);
        }
        return value;
    }

    // Returns an array of strings describing the connected joysticks.
    public string[] GetJoystickNames()
    {
        return m_joystickNames;
    }

    // Returns true while the user holds down the key identified by name. Think auto fire.
    public bool GetKey(string name)
    {
        return Input.GetKey(name);
    }

    // Returns true during the frame the user starts pressing down the key identified by name.
    public bool GetKeyDown(string name)
    {
        return Input.GetKeyDown(name);
    }

    // Returns true during the frame the user releases the key identified by name.
    public bool GetKeyUp(string name)
    {
        return Input.GetKeyUp(name);
    }

    // Returns whether the given mouse button is held down.
    public bool GetMouseButton(int button)
    {
        return Input.GetMouseButton(button);
    }

    // Returns true during the frame the user pressed the given mouse button.
    public bool GetMouseButtonDown(int button)
    {
        return Input.GetMouseButtonDown(button);
    }

    // Returns true during the frame the user releases the given mouse button.
    public bool GetMouseButtonUp(int button)
    {
        return Input.GetMouseButtonUp(button);
    }

    // Returns object representing status of a specific touch. (Does not allocate temporary variables).
    public Touch GetTouch(int index)
    {
        return (index < m_touches.Length) ? m_touches[index] : null;
    }

    // Determine whether a particular joystick model has been preconfigured by Unity. (Linux-only).
    public bool IsJoystickPreconfigured(string joystickName)
    {
        return true;
    }

    // Resets all input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame.
    public void ResetInputAxes()
    {

    }

    public void SpecifyAxisNameToAxisIndex(string axisName, int axisIndex)
    {
        m_axisMap.Remove(axisName);
        m_axisMap.Add(axisName, axisIndex);
    }

    public void SpecifyButtonNameToButtonIndex(string buttonName, int buttonIndex)
    {
        m_buttonMap.Remove(buttonName);
        m_buttonMap.Add(buttonName, buttonIndex);
    }

    void Update()
    {
        for (int ii = 0; ii < m_gamepad.buttons.Length; ++ii)
        {
            m_lastButtonState[ii] = m_buttonState[ii];
            m_buttonState[ii] = m_gamepad.buttons[ii].pressed;
        }

        float alpha = m_gamepad.axes[HFTGamepad.AXIS_ORIENTATION_ALPHA]; // Z
        float beta  = m_gamepad.axes[HFTGamepad.AXIS_ORIENTATION_BETA]; // X'
        float gamma = m_gamepad.axes[HFTGamepad.AXIS_ORIENTATION_GAMMA]; // Y''
        float orient = 0;

        m_gyro.SetAttitude( alpha, beta, gamma, orient );

        m_gyro.userAcceleration.x = m_gamepad.axes[HFTGamepad.AXIS_ACCELERATION_X];
        m_gyro.userAcceleration.y = m_gamepad.axes[HFTGamepad.AXIS_ACCELERATION_Y];
        m_gyro.userAcceleration.z = m_gamepad.axes[HFTGamepad.AXIS_ACCELERATION_Z];
        m_gyro.rotationRate.x = m_gamepad.axes[HFTGamepad.AXIS_ROTATION_RATE_ALPHA];
        m_gyro.rotationRate.y = m_gamepad.axes[HFTGamepad.AXIS_ROTATION_RATE_BETA];
        m_gyro.rotationRate.z = m_gamepad.axes[HFTGamepad.AXIS_ROTATION_RATE_GAMMA];
        m_gyro.rotationRateUnbiased.x = m_gamepad.axes[HFTGamepad.AXIS_ROTATION_RATE_ALPHA];
        m_gyro.rotationRateUnbiased.y = m_gamepad.axes[HFTGamepad.AXIS_ROTATION_RATE_BETA];
        m_gyro.rotationRateUnbiased.z = m_gamepad.axes[HFTGamepad.AXIS_ROTATION_RATE_GAMMA];

        for (int ii = 0; ii < m_touches.Length; ++ii)
        {
            Touch touch = m_touches[ii];
            int buttonNdx = HFTGamepad.BUTTON_TOUCH0 + ii;
            int axesNdx = HFTGamepad.AXIS_TOUCH0_X + ii * 2;
            TouchPhase phase = TouchPhase.Ended;
            if (m_buttonState[buttonNdx])
            {
                if (!m_lastButtonState[buttonNdx])
                {
                    phase = TouchPhase.Began;
                }
                else
                {
                    phase = TouchPhase.Moved;
                    if (m_lastPosition[axesNdx + 0] == m_gamepad.axes[axesNdx + 0] &&
                        m_lastPosition[axesNdx + 1] == m_gamepad.axes[axesNdx + 1])
                    {
                        phase = TouchPhase.Stationary;
                    }
                }
            }

            touch.phase = phase;
            float x = Mathf.Floor((m_gamepad.axes[axesNdx + 0] * 0.5f + 0.5f) * Screen.width);
            float y = Mathf.Floor((m_gamepad.axes[axesNdx + 1] * 0.5f + 0.5f) * Screen.height);
            touch.deltaPosition.x = x - touch.rawPosition.x;
            touch.deltaPosition.y = y - touch.rawPosition.y;
            touch.rawPosition.x = x;
            touch.rawPosition.y = y;
            touch.fingerId = ii;
            touch.tapCount = phase == TouchPhase.Began ? 1 : 0;
            touch.deltaTime = Time.deltaTime;
        }

        for (int ii = 0; ii < m_gamepad.axes.Length; ++ii) {
            m_lastPosition[ii] = m_gamepad.axes[ii];
        }
    }

    private HFTGamepad m_gamepad;
    private Dictionary<string, int> m_axisMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, int> m_buttonMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    private int m_accelerationEventCount = 0;
    private bool[] m_buttonState;
    private bool[] m_lastButtonState;
    private float[] m_lastPosition;
    private bool m_anyKey = false;
    private bool m_anyKeyDown = false;
    private Compass m_compass = new Compass();
    private Vector2 m_compositionCursorPos = new Vector2();
    private DeviceOrientation m_deviceOrientation = DeviceOrientation.Unknown;
    private HFTGyroscope m_gyro = new HFTGyroscope();
    private LocationService m_location = new LocationService();
    private Vector3 m_mousePosition = new Vector3();
    private Vector2 m_mouseScrollDelta = new Vector2();
    private Touch[] m_touches = new Touch[10];
    private string[] m_joystickNames = { "HFTGamepad" };
};

