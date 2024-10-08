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
        // Move �����̵�
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);

        if(move.magnitude>1)        // ���� ���̰� 1���� ũ��, 1�� �����. ����ȭ ���
        {
            move.Normalize();
        }

        move = move * WalkingSpeed * Time.deltaTime;
        move = transform.TransformDirection(move);  // ���� gameObject�� �������� ���͸� ����
        characterController.Move(move);             // character controller componenet ������

    }
}
