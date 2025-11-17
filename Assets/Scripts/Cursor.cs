using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    private Camera mainCam;

    private float currAngle = 0f;

    public InputActionReference aim;

    private enum AimMode { Mouse, Gamepad }
    private AimMode currentAimMode = AimMode.Mouse;
    public Vector2 AimDirection {  get; private set; }

    private Vector2 lastMousePos;

    private const float stickThreshold = 0.15f;     // Minimum stick input
    private const float mouseThreshold = 2f;        // Minimum pixel movement

    void Start()
    {
        mainCam = Camera.main;

        if (Mouse.current != null)
            lastMousePos = Mouse.current.position.ReadValue();
    }

    void Update()
    {
        if (PauseMenu.isPaused)
            return;

        // ---------------------------
        //  CONTROLLER DETECTION
        // ---------------------------
        Vector2 stick = Vector2.zero;
        if (Gamepad.current != null)
            stick = Gamepad.current.rightStick.ReadValue();

        if (stick.sqrMagnitude > stickThreshold * stickThreshold)
        {
            currentAimMode = AimMode.Gamepad;
        }

        // ---------------------------
        //  MOUSE DETECTION (stable)
        // ---------------------------
        if (Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            float mouseMove = (mousePos - lastMousePos).sqrMagnitude;

            if (mouseMove > mouseThreshold * mouseThreshold)
            {
                currentAimMode = AimMode.Mouse;
            }

            lastMousePos = mousePos;
        }

        // ---------------------------
        //  APPLY AIMING LOGIC
        // ---------------------------
        if (currentAimMode == AimMode.Gamepad)
        {
            // Keep last valid stick angle
            if (stick.sqrMagnitude > 0.05f)
            {
                currAngle = Mathf.Atan2(stick.y, stick.x) * Mathf.Rad2Deg;
            }

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0f, 0f, currAngle),
                Time.deltaTime * 30f
            );

            AimDirection = new Vector2(Mathf.Cos(currAngle * Mathf.Deg2Rad), Mathf.Sin(currAngle * Mathf.Deg2Rad)).normalized;
            UnityEngine.Cursor.visible = false;
        }
        else // Mouse aiming
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Vector3 world = mainCam.ScreenToWorldPoint(mouseScreen);

            Vector3 diff = world - transform.position;
            float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

            UnityEngine.Cursor.visible = true;
            AimDirection = (world - transform.position).normalized;
        }
    }

    public void Flip()
    {
        Vector3 ls = transform.localScale;
        ls.x *= -1f;
        transform.localScale = ls;
    }
}