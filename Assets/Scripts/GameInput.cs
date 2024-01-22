using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    public event EventHandler OnInteraction;
    public event EventHandler OnAlternateInteraction;
    private PlayerInputActions _playerInputActions;
    
    private void Awake() {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += InteractPerformed;
        _playerInputActions.Player.InteractAlternate.performed += AlternateInteractionPerformed;
    }

    private void AlternateInteractionPerformed(InputAction.CallbackContext obj) {
        OnAlternateInteraction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerformed(InputAction.CallbackContext obj) {
        OnInteraction?.Invoke(this, EventArgs.Empty);
    }


    public Vector2 GetMovementVectorNormalised() {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
