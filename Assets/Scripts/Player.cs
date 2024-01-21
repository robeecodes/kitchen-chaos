using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangeArgs> OnSelectedCounterChange;

    public class OnSelectedCounterChangeArgs : EventArgs {
        public BaseCounter SelectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private const float PlayerRadius = .7f;
    private const float PlayerHeight = 2f;
    private bool _isWalking;

    private const float InteractDist = 2f;
    private Vector3 _lastDir;

    private BaseCounter _selectedCounter;

    private KitchenObject _kitchenObject;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Multiple players exist 😱");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteraction += GameInputOnInteraction;
    }

    private void GameInputOnInteraction(object sender, EventArgs e) {
        if (_selectedCounter != null) {
            _selectedCounter.Interact(this);
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
            if (raycastHit.transform.TryGetComponent(out BaseCounter counter)) {
                if (counter != _selectedCounter) {
                    SetSelectedCounter(counter);
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

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotSpeed * Time.deltaTime);
    }

    private bool CanMove(Vector3 moveDir, float moveDist) {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius,
            moveDir, moveDist);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this._selectedCounter = selectedCounter;
        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeArgs() {
            SelectedCounter = _selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this._kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() {
        return _kitchenObject;
    }

    public void ClearKitchenObject() {
        _kitchenObject = null;
    }
    
    public void DestroyKitchenObject() {
        Destroy(_kitchenObject.gameObject);
        ClearKitchenObject();
    }

    public bool HasKitchenObject() {
        return _kitchenObject != null;
    }
}