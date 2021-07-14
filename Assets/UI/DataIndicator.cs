using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DataIndicator : MonoBehaviour
{
    private StateMachine stateMachine;

    private State previousState;
    private State currentState;
    private float speed;
    private float verticalSpeed;
    private bool isGrounded;

    [SerializeField] private Text previousStateIndicator;
    [SerializeField] private Text currentStateIndicator;
    [SerializeField] private Text speedIndicator;
    [SerializeField] private Text verticalSpeedIndicator;
    [SerializeField] private Text groundedIndicator;

    private void Start() =>
        stateMachine = PlayerMotion.Instance.StateMachine;

    private void Update()
    {
        HandleData();
        DrawIndicator();
    }

    private void HandleData()
    {
        previousState = stateMachine.PreviousState;
        currentState = stateMachine.CurrentState;
        speed = PlayerMotion.Instance.Speed;
        verticalSpeed = AirState.VerticalSpeed;
        isGrounded = PlayerMotion.Instance.IsGrounded;
    }

    private void DrawIndicator()
    {
        previousStateIndicator.text = previousState.GetType().Name.Replace("State", "");
        currentStateIndicator.text = currentState.GetType().Name.Replace("State", "");
        speedIndicator.text = speed.ToString();
        verticalSpeedIndicator.text = verticalSpeed.ToString();
        groundedIndicator.text = isGrounded ? "Yes" : "No";
    }
}