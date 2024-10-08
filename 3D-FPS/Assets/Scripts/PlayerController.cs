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

        InputActionAsset inputActions = GetComponent<PlayerInput>().actions; // PlayerInput에 actions를 가지고 와서 

        moveAction = inputActions.FindAction("Move");                        // move를 사용
        lookAction = inputActions.FindAction("Look");

        characterController = GetComponent<CharacterController>();

        verticalAngle = 0;
        horizontalAngle = transform.localEulerAngles.y;

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

        move = move * walkingSpeed * Time.deltaTime;
        move = transform.TransformDirection(move);  // 현재 gameObject의 방향으로 벡터를 돌림
        characterController.Move(move);             // character controller componenet 움직여

        Vector2 look = lookAction.ReadValue<Vector2>();     // look액션 벡터를 가져옴

        float turnPlayer = look.x * mouseSens;              // 마우스 감도 적용
        horizontalAngle += turnPlayer;                      //현재 각도에 더함
        if (horizontalAngle >= 360) horizontalAngle -= 360;
        if (horizontalAngle <0) horizontalAngle += 360;
        
        Vector3 currentAngle = transform.localEulerAngles;  // 변한된 현재 각도를 넣는다.
        currentAngle.y = horizontalAngle;
        transform.localEulerAngles =currentAngle;

        // 마우스 상하 
        float turnCam = look.y * mouseSens;
        verticalAngle -= turnCam;
        verticalAngle = Mathf.Clamp(verticalAngle, -89f,89f);
        currentAngle = CameraTransform.localEulerAngles;
        currentAngle.x = verticalAngle;
        CameraTransform.localEulerAngles = currentAngle;
    }
}
