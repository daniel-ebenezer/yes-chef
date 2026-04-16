using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public PlayerConfigSO playerConfigSO;

    [SerializeField] private CharacterController controller;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private BoolEventChannelSO gamePausedEvent;

    public bool canMove;

    void OnEnable()
    {
        gamePausedEvent.OnRaised.AddListener(OnGamePausedEventRecieved);
    }

    void OnDisable()
    {
        gamePausedEvent.OnRaised.RemoveListener(OnGamePausedEventRecieved);
    }

    public void OnGamePausedEventRecieved(bool isGamePaused)
    {
        canMove = !isGamePaused;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (canMove)
        {
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
            controller.Move(moveDir * playerConfigSO.moveSpeed * Time.deltaTime);

            if (moveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
            }
        }
    }
}