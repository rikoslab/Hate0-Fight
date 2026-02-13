using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction action_move;
    private InputAction action_jump;

    public float force = 20f;
    public float speed = 4f;
    public float gravity = -6.66f;
    public float acceleration = 3f;

    private bool isJumped;
    private bool isGrounded;

    [SerializeField]private Vector3 direction; // suddenly its a 2d game
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 vertical;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        action_move = playerInput.actions["Move"];
        action_jump = playerInput.actions["Jump"];
    }

    void Gravity()
    {
        isGrounded = controller.isGrounded;
        if (controller.isGrounded)
        {
            vertical.y = -2f;
        } else { vertical.y += gravity * Time.deltaTime; }
    }
    void Move()
    {
        Vector2 sudinputs = action_move.ReadValue<Vector2>();
        direction.x = sudinputs.x;
        if (direction != Vector3.zero)
        {
            position = Vector3.SmoothDamp(position, direction, ref direction, speed);
        }
        else { position = Vector3.Lerp(position, Vector3.zero, Time.deltaTime); }
    }
    void Jump()
    {
        bool sudinputs = action_jump.triggered;
        isJumped = sudinputs;
        if (sudinputs && isGrounded)
        {
            vertical += Vector3.Lerp(vertical, Vector2.up, force * Time.deltaTime);
        } else { vertical = Vector3.Lerp(vertical, Vector2.zero, acceleration * Time.deltaTime); }
    }
    public void Update()
    {
        Gravity();
        Jump();
        Move();
        controller.Move(position + vertical);
    }
}
