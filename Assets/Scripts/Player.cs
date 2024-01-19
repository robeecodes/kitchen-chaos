using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour {

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private const float PlayerRadius = .7f;
    private const float PlayerHeight = 2f;

    private bool _isWalking;
    
    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalised();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
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

    public bool IsWalking() {
        return _isWalking;
    }

    private bool CanMove(Vector3 moveDir, float moveDist) {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius,  moveDir, moveDist);
    }
}