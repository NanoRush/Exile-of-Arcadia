using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class AnchorScript : MonoBehaviour
{
    private CinemachineVirtualCamera VirtualCamera;
    private CinemachineFramingTransposer transposer;

    public bool anchorState = false;

    private GameObject AnchorAimLine;
    private GameObject player;

    public InputActionReference ThrowAction;

    private Camera mainCamera;
    private Vector2 aimDirection;

    // Input method
    private enum AimMode { Mouse, Gamepad }
    private AimMode currentAimMode = AimMode.Mouse;

    private Vector2 lastMousePos;

    private const float stickThreshold = 0.15f;
    private const float mouseThreshold = 2f;

    // Keeps the last valid controller direction
    private Vector2 lastStickDirection = Vector2.right;

    void Start()
    {
        VirtualCamera = GameObject.Find("Virtual Camera")
            .GetComponent<CinemachineVirtualCamera>();

        player = GameObject.Find("Stark");
        AnchorAimLine = transform.GetChild(0).gameObject;

        transposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        mainCamera = Camera.main;

        mainCamera.GetComponent<CinemachineBrain>().m_IgnoreTimeScale = true;

        if (Mouse.current != null)
            lastMousePos = Mouse.current.position.ReadValue();
    }

    private void Update()
    {
        DetectInputMethod();

        if (anchorState)
        {
            RotateAimLine();
        }

        if (ThrowAction.action.triggered && anchorState)
        {
            aimDirection = GetAimDirection();

            anchorState = false;

            player.GetComponent<PlayerMovement>().Teleport(
                transform.position,
                aimDirection * 2f
            );
        }
    }

    private void DetectInputMethod()
    {
        // ---------------------------
        // CONTROLLER DETECTION
        // ---------------------------
        Vector2 stick = Vector2.zero;

        if (Gamepad.current != null)
            stick = Gamepad.current.rightStick.ReadValue();

        if (stick.sqrMagnitude > stickThreshold * stickThreshold)
        {
            currentAimMode = AimMode.Gamepad;

            // Save last valid stick direction
            lastStickDirection = stick.normalized;
        }

        // ---------------------------
        // MOUSE DETECTION
        // ---------------------------
        if (Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            float mouseMove =
                (mousePos - lastMousePos).sqrMagnitude;

            if (mouseMove > mouseThreshold * mouseThreshold)
            {
                currentAimMode = AimMode.Mouse;
            }

            lastMousePos = mousePos;
        }
    }

    private void RotateAimLine()
    {
        Vector2 direction;

        if (currentAimMode == AimMode.Gamepad)
        {
            // Right joystick direction
            Vector2 stick = Gamepad.current.rightStick.ReadValue();

            if (stick.sqrMagnitude > stickThreshold * stickThreshold)
            {
                lastStickDirection = stick.normalized;
            }

            // Keep the last direction when stick is released
            direction = lastStickDirection;
        }
        else
        {
            // ---------------------------
            // MOUSE AIMING
            // ---------------------------
            Vector3 mousePosition =
                Mouse.current.position.ReadValue();

            Vector3 worldMousePosition =
                mainCamera.ScreenToWorldPoint(mousePosition);

            worldMousePosition.z = 0f;

            direction =
                (worldMousePosition - transform.position).normalized;
        }

        // ---------------------------
        // MOVE AIM LINE AROUND ANCHOR
        // ---------------------------

        float radius = 1.5f;

        AnchorAimLine.transform.localPosition =
            direction * radius;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        AnchorAimLine.transform.rotation =
            Quaternion.Euler(0f, 0f, angle - 90f);
    }

    public Vector2 AnchorActivate()
    {
        StartCoroutine(AnchorRoutine());
        return aimDirection;
    }

    private IEnumerator AnchorRoutine()
{
    anchorState = true;

    // Slow down gameplay
    Time.timeScale = 0.05f;

    // Higher damping = smoother/slower camera movement
    transposer.m_XDamping = 0.8f;
    transposer.m_YDamping = 0.8f;

    VirtualCamera.Follow = transform;
    AnchorAimLine.SetActive(true);
    TogglePlayer(true);

    yield return new WaitUntil(() => anchorState == false);

    Time.timeScale = 1f;

    AnchorAimLine.SetActive(false);
    TogglePlayer(false);

    // Smoothly return camera to player
    transposer.m_XDamping = 0.8f;
    transposer.m_YDamping = 0.8f;

    VirtualCamera.Follow = player.transform;

    // Give the camera time to smoothly follow player
    yield return new WaitForSeconds(0.5f);

    transposer.m_XDamping = 1f;
    transposer.m_YDamping = 1f;
}

    private Vector2 GetAimDirection()
    {
        if (currentAimMode == AimMode.Gamepad)
        {
            return lastStickDirection;
        }

        // Mouse
        Vector3 mousePosition =
            Mouse.current.position.ReadValue();

        Vector3 worldMousePosition =
            mainCamera.ScreenToWorldPoint(mousePosition);

        worldMousePosition.z = 0f;

        Vector2 direction =
            worldMousePosition - transform.position;

        return direction.normalized;
    }

    private void TogglePlayer(bool on)
    {
        if (on)
        {
            player.GetComponent<PlayerAttack>().enabled = false;
            player.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            player.GetComponent<PlayerAttack>().enabled = true;
            player.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}