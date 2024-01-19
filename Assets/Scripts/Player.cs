using System;
using UnityEngine;

public class Player : MonoBehaviour {
    public event EventHandler<OnSelectedCounterChangeArgs> OnSelectedCounterChange;

    public class OnSelectedCounterChangeArgs : EventArgs {
        public ClearCounter selectedCounter;
    }
    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private const float PlayerRadius = .7f;
    private const float PlayerHeight = 2f;
    private bool _isWalking;

    private const float InteractDist = 2f;
    private Vector3 _lastDir;

    private ClearCounter _selectedCounter;

    private void Start() {
        gameInput.OnInteraction += GameInputOnInteraction;
    }

    private void GameInputOnInteraction(object sender, EventArgs e) {
        if (_selectedCounter != null) {
            _selectedCounter.Interact();
        }
    }

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalised();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        HandleInteractions(inputVector, moveDir);
        HandleMovement(inputVector, moveDir);
    }

    public bool IsWalking() {
        return _isWalking;
    }

    private void HandleInteractions(Vector2 inputVector, Vector3 moveDir) {
        if (moveDir != Vector3.zero) {
            _lastDir = moveDir;
        }

        if (Physics.Raycast(transform.position, _lastDir, out RaycastHit raycastHit, InteractDist, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                if (clearCounter != _selectedCounter) {
                    SetSelectedCounter(clearCounter);
                }
            }
            else {
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement(Vector2 inputVector, Vector3 moveDir) {
        _isWalking = moveDir != Vector3.zero;

        float moveDist = moveSpeed * Time.deltaTime;

        bool canMove = CanMove(moveDir, moveDist);

        if (!canMove) {
            // Check if movement along the x-axis is possible
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = CanMove(moveDirX, moveDist);

            if (canMove) {
                moveDir = moveDirX;
            }
            else {
                // Check if movement along the z-axis is possible
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = CanMove(moveDirZ, moveDist);

                if (canMove) {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * (moveDist);
        }

        float rotSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotSpeed * Time.deltaTime);
    }

    private bool CanMove(Vector3 moveDir, float moveDist) {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius,
            moveDir, moveDist);
    }

    private void SetSelectedCounter(ClearCounter selectedCounter) {
        this._selectedCounter = selectedCounter;
        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeArgs() {
            selectedCounter = _selectedCounter
        });
    }
}