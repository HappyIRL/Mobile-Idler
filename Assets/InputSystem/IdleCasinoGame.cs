//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/InputSystem/IdleCasinoGame.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @IdleCasinoGame : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @IdleCasinoGame()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""IdleCasinoGame"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""3fb513d3-eecf-460e-880b-82c7b2f9fb6e"",
            ""actions"": [
                {
                    ""name"": ""Touch0Tap"",
                    ""type"": ""Button"",
                    ""id"": ""7144b8f3-b455-4196-a271-28d1d22324bc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Touch0"",
                    ""type"": ""Button"",
                    ""id"": ""cb35500f-1e08-4db6-b730-b04a134a6d22"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Touch1"",
                    ""type"": ""Button"",
                    ""id"": ""3b3e20c2-5bbb-4027-9457-0b83e8ec80d9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Touch0Position"",
                    ""type"": ""PassThrough"",
                    ""id"": ""da1b69a8-4636-4666-bfdc-c17b51b28d43"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Touch1Position"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3a89f2f1-12a3-4472-805e-31f851be161c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Touch0Delta"",
                    ""type"": ""Value"",
                    ""id"": ""97331bac-adb9-4767-a932-ea636441f01e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Touch1Delta"",
                    ""type"": ""Value"",
                    ""id"": ""f4fdea0a-1de2-4f9e-b7e0-63ec1217165a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""06789d72-38d2-434f-abc3-8f4691fbd014"",
                    ""path"": ""<Touchscreen>/touch0/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Touch0Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca7ae858-b80e-4c19-ac27-d5baa17e6302"",
                    ""path"": ""<Touchscreen>/touch0/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Touch0Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a70c1c68-b7e6-457f-891e-9fa186090635"",
                    ""path"": ""<Touchscreen>/touch1/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Touch1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d763ef7-a78f-4787-b1de-8119a3198f01"",
                    ""path"": ""<Touchscreen>/touch0/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Touch0"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa9909a7-c97f-4cef-a112-30ccd580c3a9"",
                    ""path"": ""<Touchscreen>/touch0/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Touch0Delta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c85b70d-4d50-4c19-9d68-31c1e8f67816"",
                    ""path"": ""<Touchscreen>/touch1/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Touch1Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f36d5d7e-208f-49c2-8b2c-e1a64f6b16bd"",
                    ""path"": ""<Touchscreen>/touch1/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Touch1Delta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Touch0Tap = m_Player.FindAction("Touch0Tap", throwIfNotFound: true);
        m_Player_Touch0 = m_Player.FindAction("Touch0", throwIfNotFound: true);
        m_Player_Touch1 = m_Player.FindAction("Touch1", throwIfNotFound: true);
        m_Player_Touch0Position = m_Player.FindAction("Touch0Position", throwIfNotFound: true);
        m_Player_Touch1Position = m_Player.FindAction("Touch1Position", throwIfNotFound: true);
        m_Player_Touch0Delta = m_Player.FindAction("Touch0Delta", throwIfNotFound: true);
        m_Player_Touch1Delta = m_Player.FindAction("Touch1Delta", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Touch0Tap;
    private readonly InputAction m_Player_Touch0;
    private readonly InputAction m_Player_Touch1;
    private readonly InputAction m_Player_Touch0Position;
    private readonly InputAction m_Player_Touch1Position;
    private readonly InputAction m_Player_Touch0Delta;
    private readonly InputAction m_Player_Touch1Delta;
    public struct PlayerActions
    {
        private @IdleCasinoGame m_Wrapper;
        public PlayerActions(@IdleCasinoGame wrapper) { m_Wrapper = wrapper; }
        public InputAction @Touch0Tap => m_Wrapper.m_Player_Touch0Tap;
        public InputAction @Touch0 => m_Wrapper.m_Player_Touch0;
        public InputAction @Touch1 => m_Wrapper.m_Player_Touch1;
        public InputAction @Touch0Position => m_Wrapper.m_Player_Touch0Position;
        public InputAction @Touch1Position => m_Wrapper.m_Player_Touch1Position;
        public InputAction @Touch0Delta => m_Wrapper.m_Player_Touch0Delta;
        public InputAction @Touch1Delta => m_Wrapper.m_Player_Touch1Delta;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Touch0Tap.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0Tap;
                @Touch0Tap.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0Tap;
                @Touch0Tap.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0Tap;
                @Touch0.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0;
                @Touch0.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0;
                @Touch0.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0;
                @Touch1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch1;
                @Touch1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch1;
                @Touch1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch1;
                @Touch0Position.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0Position;
                @Touch0Position.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0Position;
                @Touch0Position.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0Position;
                @Touch1Position.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch1Position;
                @Touch1Position.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch1Position;
                @Touch1Position.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch1Position;
                @Touch0Delta.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0Delta;
                @Touch0Delta.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0Delta;
                @Touch0Delta.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch0Delta;
                @Touch1Delta.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch1Delta;
                @Touch1Delta.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch1Delta;
                @Touch1Delta.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouch1Delta;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Touch0Tap.started += instance.OnTouch0Tap;
                @Touch0Tap.performed += instance.OnTouch0Tap;
                @Touch0Tap.canceled += instance.OnTouch0Tap;
                @Touch0.started += instance.OnTouch0;
                @Touch0.performed += instance.OnTouch0;
                @Touch0.canceled += instance.OnTouch0;
                @Touch1.started += instance.OnTouch1;
                @Touch1.performed += instance.OnTouch1;
                @Touch1.canceled += instance.OnTouch1;
                @Touch0Position.started += instance.OnTouch0Position;
                @Touch0Position.performed += instance.OnTouch0Position;
                @Touch0Position.canceled += instance.OnTouch0Position;
                @Touch1Position.started += instance.OnTouch1Position;
                @Touch1Position.performed += instance.OnTouch1Position;
                @Touch1Position.canceled += instance.OnTouch1Position;
                @Touch0Delta.started += instance.OnTouch0Delta;
                @Touch0Delta.performed += instance.OnTouch0Delta;
                @Touch0Delta.canceled += instance.OnTouch0Delta;
                @Touch1Delta.started += instance.OnTouch1Delta;
                @Touch1Delta.performed += instance.OnTouch1Delta;
                @Touch1Delta.canceled += instance.OnTouch1Delta;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnTouch0Tap(InputAction.CallbackContext context);
        void OnTouch0(InputAction.CallbackContext context);
        void OnTouch1(InputAction.CallbackContext context);
        void OnTouch0Position(InputAction.CallbackContext context);
        void OnTouch1Position(InputAction.CallbackContext context);
        void OnTouch0Delta(InputAction.CallbackContext context);
        void OnTouch1Delta(InputAction.CallbackContext context);
    }
}