//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Samples/Input System/1.0.2/Custom Device Usages/TwoPlayerControls.inputactions
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

public partial class @TwoPlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TwoPlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TwoPlayerControls"",
    ""maps"": [
        {
            ""name"": ""TwoPlayers"",
            ""id"": ""0e22eb1c-8c6e-4cec-8364-2a9f0e3ef769"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""d35725fa-073a-4e1c-9052-2c45b8ef0b4c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Button"",
                    ""id"": ""249187b5-59c4-459f-84d0-731cb510c536"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""TwoPlayers"",
            ""bindingGroup"": ""TwoPlayers"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>{Player1}"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Gamepad>{Player2}"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // TwoPlayers
        m_TwoPlayers = asset.FindActionMap("TwoPlayers", throwIfNotFound: true);
        m_TwoPlayers_Move = m_TwoPlayers.FindAction("Move", throwIfNotFound: true);
        m_TwoPlayers_Look = m_TwoPlayers.FindAction("Look", throwIfNotFound: true);
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

    // TwoPlayers
    private readonly InputActionMap m_TwoPlayers;
    private List<ITwoPlayersActions> m_TwoPlayersActionsCallbackInterfaces = new List<ITwoPlayersActions>();
    private readonly InputAction m_TwoPlayers_Move;
    private readonly InputAction m_TwoPlayers_Look;
    public struct TwoPlayersActions
    {
        private @TwoPlayerControls m_Wrapper;
        public TwoPlayersActions(@TwoPlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_TwoPlayers_Move;
        public InputAction @Look => m_Wrapper.m_TwoPlayers_Look;
        public InputActionMap Get() { return m_Wrapper.m_TwoPlayers; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TwoPlayersActions set) { return set.Get(); }
        public void AddCallbacks(ITwoPlayersActions instance)
        {
            if (instance == null || m_Wrapper.m_TwoPlayersActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TwoPlayersActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
        }

        private void UnregisterCallbacks(ITwoPlayersActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
        }

        public void RemoveCallbacks(ITwoPlayersActions instance)
        {
            if (m_Wrapper.m_TwoPlayersActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITwoPlayersActions instance)
        {
            foreach (var item in m_Wrapper.m_TwoPlayersActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TwoPlayersActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TwoPlayersActions @TwoPlayers => new TwoPlayersActions(this);
    private int m_TwoPlayersSchemeIndex = -1;
    public InputControlScheme TwoPlayersScheme
    {
        get
        {
            if (m_TwoPlayersSchemeIndex == -1) m_TwoPlayersSchemeIndex = asset.FindControlSchemeIndex("TwoPlayers");
            return asset.controlSchemes[m_TwoPlayersSchemeIndex];
        }
    }
    public interface ITwoPlayersActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
}
