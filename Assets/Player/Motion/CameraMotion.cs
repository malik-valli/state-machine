using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    private static CameraMotion instance;
    public static CameraMotion Instance { get { return instance; } }

    [SerializeField] private float viewSensitivity = 1;
    private float viewSensitivityDeceleration = 0.2f;

    private float viewUpLimit = -89;
    private float viewDownLimit = 86;

    public float HorizontalRotation { get; private set; }
    public float VerticalRotation { get; private set; }

    [SerializeField] private bool invertYAxis;
    private float yAxisInverter { get { return invertYAxis ? 1 : -1; } }

    private void Awake()
    {
        Singleton<CameraMotion>.InitializeInstance(ref instance, this);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleInput();
        Rotate();
    }

    private void HandleInput()
    {
        HorizontalRotation += yAxisInverter * PlayerControl.Instance.MouseDelta.y * viewSensitivity * viewSensitivityDeceleration;
        HorizontalRotation = Mathf.Clamp(HorizontalRotation, viewUpLimit, viewDownLimit);

        VerticalRotation += PlayerControl.Instance.MouseDelta.x * viewSensitivity * viewSensitivityDeceleration;
    }

    private void Rotate() =>
        Camera.main.transform.localRotation = Quaternion.Euler(HorizontalRotation, 0, 0);
}
