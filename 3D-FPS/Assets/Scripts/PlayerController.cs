using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 7;
    public float mouseSens = 0.01f;
    public float jumpSpeed = 6;

    public Transform CameraTransform;
    public Weapon weapon;

    float gravity = 9.81f;         // �߷� ���ӵ�
    float terminalSpeed = 20;      // ���� �ӵ�(���Ͻ�, ���� �ӵ� �̻����� �������� �ʰ� �ϱ� ���ؼ� ���� ��ġ)
    
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

        InputActionAsset inputActions = GetComponent<PlayerInput>().actions; // PlayerInput�� actions�� ������ �ͼ� 

        moveAction = inputActions.FindAction("Move");                        // move�� ���
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

        // ���콺 �¿� 
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
        // ���콺�� �ø��� ������� ���;� �ϴµ�
        // ���ӿ��� ���콺�� �÷��� ���� �÷������� �������� �־���� �Ѵ�.? 
        verticalAngle -= turnCam;           
        verticalAngle = Mathf.Clamp(verticalAngle, -89f,89f);
        currentAngle = CameraTransform.localEulerAngles;
        currentAngle.x = verticalAngle;
        CameraTransform.localEulerAngles = currentAngle;

        // �߷�

        verticalSpeed -= gravity * Time.deltaTime;  // �ӵ� = ���ӵ� * �ð�
        if (verticalSpeed < -terminalSpeed)         // -terminalSpeed���� �������� �ӵ��� ���� ���� ������ ������
        {
            verticalSpeed = -terminalSpeed;         // �������� �ӵ��� -terminalSpeed�� ����
        }
        Vector3 verticalMove = new Vector3(0, verticalSpeed, 0);      // �ϰ��ϴ� Vector�� delta.Time����
        verticalMove *= Time.deltaTime;                               

        CollisionFlags flag = characterController.Move(verticalMove); // ĳ���Ͱ� ��� �ε������� Ȯ���ϴ� �κ�

        //ĳ���Ͱ� �ٴڿ� ������(CollisionFlags.Below), verticalSpeed�� 0���� �Ͽ� ���ϸ� ����ϴ�.
        if ((flag & (CollisionFlags.Below | CollisionFlags.Above)) != 0)                       
        {
            verticalSpeed = 0;
        }

        if (!characterController.isGrounded) // ������ �������ְ�,
        {
            if (isGrounded)                  // ���� ���� �پ��־��ٸ�
            {
                groundedTimer += Time.deltaTime;
                if (groundedTimer > 0.3f)      // �������ִ� �ð��� 0.5�� �Ѿ��
                {
                    isGrounded = false;     // ����������
                }
            }
        }
        else                                // ���� �پ�������
        {
            isGrounded = true;              // ���� �پ�����
            groundedTimer = 0;
        }

        // �� �߻� �� ����
        // �̹� �����ӿ� ���ȳ���?
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
            isGrounded = false;     // �̶� true�� �ϰԵǸ� 0.3�� ���� ������ �ϸ� �ٴ� ���� ����
        }
        //Debug.Log("OnJump Active");
    }

    
}
