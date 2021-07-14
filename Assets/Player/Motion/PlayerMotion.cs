using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotion : MonoBehaviour
{
    private static PlayerMotion instance;
    public static PlayerMotion Instance { get { return instance; } }

    public CharacterController CharacterController { get; private set; }

    public bool IsGrounded { get; private set; }

    public Vector3 MotionVector { get; private set; }
    public Vector3 PhysicalMotionVector { get; private set; }

    public float Speed { get; private set; }

    public StateMachine StateMachine { get; private set; }

    private IdleState idleState; // Initial state.
    [SerializeField] private WalkingState walkingState;
    [SerializeField] private RunningState runningState;
    [SerializeField] private JumpingState jumpingState;
    [SerializeField] private FallingState fallingState;

    private void Awake()
    {
        Singleton<PlayerMotion>.InitializeInstance(ref instance, this);

        CharacterController = GetComponent<CharacterController>();

        InitializeStateMachine();
    }

    private void Update()
    {
        StateMachine.Tick();

        RotateWithCamera();
        CharacterController.Move(MotionVector * Time.deltaTime);
        Speed = Vector3.Magnitude(MotionVector);
    }

    private void FixedUpdate()
    {
        StateMachine.FixedTick();

        CharacterController.Move(PhysicalMotionVector * Time.fixedDeltaTime);
        IsGrounded = CharacterController.isGrounded;
    }

    private void LateUpdate() =>
        ClearMovement();

    private void RotateWithCamera() =>
        transform.localRotation = Quaternion.Euler(0, CameraMotion.Instance.VerticalRotation, 0);

    public void AddMotion(Vector3 movementVector) =>
        MotionVector += movementVector;

    public void AddPhysicalMotion(Vector3 physicalMotionVector) =>
        PhysicalMotionVector += physicalMotionVector;

    private void ClearMovement() =>
        MotionVector = PhysicalMotionVector = Vector3.zero;


    private void InitializeStateMachine()
    {
        StateMachine = new StateMachine();

        idleState = new IdleState();
        walkingState = new WalkingState();
        runningState = new RunningState();
        jumpingState = new JumpingState();
        fallingState = new FallingState();

        StateMachine.SetState(idleState); // Setting the initial state.

        StateMachine.AddTransition(idleState, walkingState, IsWalking()); // Idle ---> Walking.
        StateMachine.AddTransition(walkingState, idleState, IsWalking(false)); // Walking ---> Idle.

        StateMachine.AddTransition(idleState, runningState, AndFunctions(ForwardMovementOccurs(), ShiftPressed())); // etc...
        StateMachine.AddTransition(runningState, idleState, AndFunctions(IsWalking(false), ShiftPressed(false)));

        StateMachine.AddTransition(idleState, jumpingState, IsJumping());

        StateMachine.AddTransition(walkingState, jumpingState, IsJumping());

        StateMachine.AddTransition(runningState, jumpingState, IsJumping());

        StateMachine.AddTransition(jumpingState, fallingState, VerticalSpeedIsNotPositive());

        StateMachine.AddTransition(idleState, fallingState, IsGroundedFunc(false));
        StateMachine.AddTransition(fallingState, idleState, IsGroundedFunc());

        StateMachine.AddTransition(walkingState, fallingState, IsGroundedFunc(false));

        StateMachine.AddTransition(runningState, fallingState, IsGroundedFunc(false));

        StateMachine.AddTransition(walkingState, runningState, AndFunctions(ForwardMovementOccurs(), ShiftPressed()));
        StateMachine.AddTransition(runningState, walkingState, OrFunctions(ShiftPressed(false), ForwardMovementOccurs(false)));
    }

    #region State Transition Conditions
    // --------------------------------
    private Func<bool> AndFunctions(Func<bool> func1, Func<bool> func2) => () => func1() && func2();
    private Func<bool> AndFunctions(Func<bool> func1, Func<bool> func2, Func<bool> func3) => () => func1() && func2() && func3();
    private Func<bool> OrFunctions(Func<bool> func1, Func<bool> func2) => () => func1() || func2();
    private Func<bool> OrFunctions(Func<bool> func1, Func<bool> func2, Func<bool> func3) => () => func1() || func2() || func3();

    private Func<bool> IsWalking(bool prediction = true) => () => PlayerControl.Instance.OnWalk.phase == InputActionPhase.Started == prediction;

    private Func<bool> IsJumping(bool prediction = true) => () => PlayerControl.Instance.OnJump.triggered == prediction;

    public Func<bool> IsGroundedFunc(bool prediction = true) => () => IsGrounded == prediction;

    private Func<bool> VerticalSpeedIsNotPositive() => () => AirState.VerticalSpeed <= 0;

    private Func<bool> ShiftPressed(bool prediction = true) => () => PlayerControl.Instance.OnRun.phase == InputActionPhase.Started == prediction;

    private Func<bool> ForwardMovementOccurs(bool prediction = true) => () => PlayerControl.Instance.VerticalAxis > 0 == prediction;
    // --------------------------------
    #endregion
}