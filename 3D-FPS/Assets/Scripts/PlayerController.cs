using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float WalkingSpeed = 7;

    InputAction moveAction;

    CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputActionAsset inputActions = GetComponent<PlayerInput>().actions;

        moveAction = inputActions.FindAction("Move");

        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move 평행이동
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);

        if(move.magnitude>1)        // 벡터 길이가 1보다 크면, 1로 맞춘다. 정규화 사용
        {
            move.Normalize();
        }

        move = move * WalkingSpeed * Time.deltaTime;
        move = transform.TransformDirection(move);  // 현재 gameObject의 방향으로 벡터를 돌림
        characterController.Move(move);             // character controller componenet 움직여

    }
}
