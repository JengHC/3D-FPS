using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 7;
    public float mouseSens = 0.01f;
    public Transform CameraTransform;

    float horizontalAngle;
    float verticalAngle;

    InputAction moveAction;
    InputAction lookAction;

    CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputActionAsset inputActions = GetComponent<PlayerInput>().actions; // PlayerInput�� actions�� ������ �ͼ� 

        moveAction = inputActions.FindAction("Move");                        // move�� ���
        lookAction = inputActions.FindAction("Look");

        characterController = GetComponent<CharacterController>();

        verticalAngle = 0;
        horizontalAngle = transform.localEulerAngles.y;

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

        move = move * walkingSpeed * Time.deltaTime;
        move = transform.TransformDirection(move);  // ���� gameObject�� �������� ���͸� ����
        characterController.Move(move);             // character controller componenet ������

        Vector2 look = lookAction.ReadValue<Vector2>();     // look�׼� ���͸� ������

        float turnPlayer = look.x * mouseSens;              // ���콺 ���� ����
        horizontalAngle += turnPlayer;                      //���� ������ ����
        if (horizontalAngle >= 360) horizontalAngle -= 360;
        if (horizontalAngle <0) horizontalAngle += 360;
        
        Vector3 currentAngle = transform.localEulerAngles;  // ���ѵ� ���� ������ �ִ´�.
        currentAngle.y = horizontalAngle;
        transform.localEulerAngles =currentAngle;

        // ���콺 ���� 
        float turnCam = look.y * mouseSens;
        verticalAngle -= turnCam;
        verticalAngle = Mathf.Clamp(verticalAngle, -89f,89f);
        currentAngle = CameraTransform.localEulerAngles;
        currentAngle.x = verticalAngle;
        CameraTransform.localEulerAngles = currentAngle;
    }
}
