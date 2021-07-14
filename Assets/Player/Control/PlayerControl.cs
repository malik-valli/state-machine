using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private static PlayerControl instance;
    public static PlayerControl Instance { get { return instance; } }

    [SerializeField] private InputActionAsset inputActionAsset;

    private InputActionMap playerControlsMap;

    public InputAction OnView { get; private set; }
    public InputAction OnWalk { get; private set; }
    public InputAction OnRun { get; private set; }
    public InputAction OnJump { get; private set; }

    public float HorizontalAxis { get; private set; }
    public float VerticalAxis { get; private set; }

    public Vector2 MouseDelta { get; private set; }

    private void Awake()
    {
        Singleton<PlayerControl>.InitializeInstance(ref instance, this);

        playerControlsMap = inputActionAsset.FindActionMap("Player Controls");

        OnView = playerControlsMap.FindAction("View");
        OnWalk = playerControlsMap.FindAction("Walk");
        OnJump = playerControlsMap.FindAction("Jump");
        OnRun = playerControlsMap.FindAction("Run");

        OnView.started += OnViewHandler;
        OnView.performed += OnViewHandler;
        OnView.canceled += OnViewHandler;

        OnWalk.started += OnWalkHandler;
        OnWalk.performed += OnWalkHandler;
        OnWalk.canceled += OnWalkHandler;

        OnJump.started += OnJumpHandler;
        OnJump.performed += OnJumpHandler;
        OnJump.canceled += OnJumpHandler;

        OnRun.started += OnRunHandler;
        OnRun.performed += OnRunHandler;
        OnRun.canceled += OnRunHandler;
    }

    private void OnViewHandler(InputAction.CallbackContext context) =>
        MouseDelta = context.ReadValue<Vector2>();

    private void OnWalkHandler(InputAction.CallbackContext context)
    {
        HorizontalAxis = context.ReadValue<Vector2>().x;
        VerticalAxis = context.ReadValue<Vector2>().y;
    }

    private void OnRunHandler(InputAction.CallbackContext context) { } // Empty.

    private void OnJumpHandler(InputAction.CallbackContext context) { } // Empty.
}