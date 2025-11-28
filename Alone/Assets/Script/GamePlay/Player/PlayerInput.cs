using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;

    [Header("Scriptable Object")]
    [SerializeField] private PlayerData _playerData;

    [Header ("JoyStick")]
    [SerializeField] private FloatingJoystick floatingJoystick;

    // Local Data
    private Vector2 MoveInput;
    // Jump
    [SerializeField] private float gravity = -20f;
    private Vector3 velocity;
    private float lastGroundedTime;
    public float groundCheckTime = 0.15f;

    //components
    private CharacterController cc;

    [Header("Debug")]
    [SerializeField] private bool PC;

    public void Awake()
    {
        if (instance==null)
        {
       
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);

    }

    private void Start()
    {
        cc=GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!PC)
        {
            JoyStickInput();
        }

        HandleGravity();
        ApplyInput();

    }

    private void ApplyInput()
    {
        //Vector3 move = new Vector3(MoveInput.x, 0, MoveInput.y);

        Vector3 MoveDirection = transform.right * MoveInput.x + transform.forward * MoveInput.y;

        MoveDirection = MoveDirection.normalized;

        cc.Move(MoveDirection * _playerData.MoveSpeed * Time.deltaTime);
        cc.Move(velocity * Time.deltaTime);


    }

    private void HandleGravity()
    {
        if (cc.isGrounded)
        {
            lastGroundedTime = Time.time;

            if (velocity.y < 0)
                velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }

    private void JoyStickInput()
    {
        MoveInput.x = floatingJoystick.Horizontal;
        MoveInput.y = floatingJoystick.Vertical;
    }

    public void MoveInputActionMap(InputAction.CallbackContext ctx)
    {
        Debug.Log("move");
        MoveInput= ctx.ReadValue<Vector2>();

    }


}
