using System;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private bool _isWalking;
    
    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalised();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        _isWalking = moveDir != Vector3.zero;

        transform.position += moveDir * (moveSpeed * Time.deltaTime);

        float rotSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotSpeed * Time.deltaTime);
    }

    public bool IsWalking() {
        return _isWalking;
    }
}