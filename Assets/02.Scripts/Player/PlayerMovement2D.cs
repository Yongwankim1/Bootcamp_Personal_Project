using UnityEngine;
using UnityEngine.InputSystem;
public enum Direction
{
    None,

    Up,
    Left,
    Right,
    Down,

    LeftUP,
    RightUP,

    LeftDown,
    RightDown,
}
public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] PlayerInputReader inputReader;
    [SerializeField] PlayerStamina playerStamina;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] Direction currentDirection = Direction.None;
    [SerializeField] Vector2 inputDirection = Vector2.zero;
    [SerializeField] bool isRun;
    private float currentSpeed;
    public Direction CurrentDirection => currentDirection;


    private void Awake()
    {
        if(inputReader == null) inputReader = GetComponent<PlayerInputReader>();

        if(rb == null) rb = GetComponent<Rigidbody2D>();

        if(playerStamina == null) playerStamina = GetComponent<PlayerStamina>();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }
    private void Update()
    {
        inputDirection = inputReader != null ? inputReader.MoveDir : Vector2.zero;
        isRun = Keyboard.current.leftShiftKey.IsPressed();

        currentSpeed = isRun ? runSpeed : moveSpeed;

        if (playerStamina.IsStaminaDepleted)
            currentSpeed = moveSpeed * 0.5f;
        UpdateDirection();
    }

    void PlayerMove()
    {
        rb.MovePosition(rb.position + inputReader.MoveDir * currentSpeed * Time.fixedDeltaTime);
    }

    void UpdateDirection()
    {
        const float deadZoneValue = 0.1f;

        if (inputDirection.sqrMagnitude < 0.01f) return;

        float inputX = inputDirection.x;
        float inputY = inputDirection.y;

        if (inputX > deadZoneValue)
        {
            if (inputY > deadZoneValue) currentDirection = Direction.RightUP;
            else if (inputY < -1 * deadZoneValue) currentDirection = Direction.LeftUP;
            else currentDirection = Direction.Right;
        }
        else if (inputX < -1 * deadZoneValue)
        {
            if (inputY > deadZoneValue) currentDirection = Direction.RightDown;
            else if (inputY < -1 * deadZoneValue) currentDirection = Direction.LeftDown;
            else currentDirection = Direction.Left;
        }
        else
        {
            if (inputY > deadZoneValue) currentDirection = Direction.Up;
            else if (inputY < -1 * deadZoneValue) currentDirection = Direction.Down;
        }
    }
}
