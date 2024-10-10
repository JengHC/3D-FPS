using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 7;
    public float mouseSens = 0.01f;
    public float jumpSpeed = 6;

    public Transform CameraTransform;
    public Weapon weapon;

    float gravity = 9.81f;         // 중력 가속도
    float terminalSpeed = 20;      // 종단 속도(낙하시, 일정 속도 이상으로 빨라지지 않게 하기 위해서 만든 장치)
    
    float verticalSpeed;
    float horizontalAngle;
    float verticalAngle;

    bool isGrounded;
    float groundedTimer;

    InputAction moveAction;
    InputAction lookAction;
    InputAction attackAction;
    InputAction reloadAction;


    CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputActionAsset inputActions = GetComponent<PlayerInput>().actions; // PlayerInput에 actions를 가지고 와서 

        moveAction = inputActions.FindAction("Move");                        // move를 사용
        lookAction = inputActions.FindAction("Look");
        attackAction = inputActions.FindAction("Attack");
        reloadAction = inputActions.FindAction("Reload");

        characterController = GetComponent<CharacterController>();

        verticalAngle = 0;
        horizontalAngle = transform.localEulerAngles.y;

        isGrounded = true;
        groundedTimer = 0;
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

        // 마우스 좌우 
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
        // 마우스를 올리면 양수값이 들어와야 하는데
        // 게임에서 마우스를 올려서 위를 올려보려면 음수값을 넣어줘야 한다.? 
        verticalAngle -= turnCam;           
        verticalAngle = Mathf.Clamp(verticalAngle, -89f,89f);
        currentAngle = CameraTransform.localEulerAngles;
        currentAngle.x = verticalAngle;
        CameraTransform.localEulerAngles = currentAngle;

        // 중력

        verticalSpeed -= gravity * Time.deltaTime;  // 속도 = 가속도 * 시간
        if (verticalSpeed < -terminalSpeed)         // -terminalSpeed보다 떨어지는 속도가 낮은 값을 가지고 있으면
        {
            verticalSpeed = -terminalSpeed;         // 떨어지는 속도를 -terminalSpeed로 고정
        }
        Vector3 verticalMove = new Vector3(0, verticalSpeed, 0);      // 하강하는 Vector에 delta.Time적용
        verticalMove *= Time.deltaTime;                               

        CollisionFlags flag = characterController.Move(verticalMove); // 캐릭터가 어디에 부딪혔는지 확인하는 부분

        //캐릭터가 바닥에 닿으면(CollisionFlags.Below), verticalSpeed를 0으로 하여 낙하를 멈춥니다.
        if ((flag & (CollisionFlags.Below | CollisionFlags.Above)) != 0)                       
        {
            verticalSpeed = 0;
        }

        if (!characterController.isGrounded) // 땅에서 떨어져있고,
        {
            if (isGrounded)                  // 전에 땅에 붙어있었다면
            {
                groundedTimer += Time.deltaTime;
                if (groundedTimer > 0.3f)      // 떨어져있는 시간이 0.5초 넘어가면
                {
                    isGrounded = false;     // 떨어져있음
                }
            }
        }
        else                                // 땅에 붙어있으면
        {
            isGrounded = true;              // 땅에 붙어있음
            groundedTimer = 0;
        }

        // 총 발사 및 장전
        // 이번 프레임에 눌렸나요?
        if(attackAction.WasPressedThisFrame())
        {
            weapon.FireWeapon();          
        }
        if(reloadAction.WasPerformedThisFrame())
        {
            weapon.ReloadWeapon();
        }
    }

    void OnJump()
    {
        if(isGrounded)
        {
            verticalSpeed = jumpSpeed;
            isGrounded = false;     // 이때 true를 하게되면 0.3초 내에 점프를 하면 다단 점프 가능
        }
        //Debug.Log("OnJump Active");
    }

    
}
