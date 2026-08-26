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

    void Start()
    {
        VirtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();

        player = GameObject.Find("Stark");
        AnchorAimLine = transform.GetChild(0).gameObject;

        transposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (anchorState)
        {
            RotateAimLine();
        }

        if (ThrowAction.action.triggered && anchorState)
        {
            aimDirection = GetAimDirection();
            anchorState = false;
            player.GetComponent<PlayerMovement>().Teleport(transform.position, aimDirection * 2f);
        }
    }

    private void RotateAimLine()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldMousePosition =
            Camera.main.ScreenToWorldPoint(mousePosition);

        worldMousePosition.z = 0f;

        // Direction from anchor to mouse
        Vector2 direction = (worldMousePosition - transform.position).normalized;

        // Distance from anchor
        float radius = 1.5f;

        // Move the aim line around the anchor
        AnchorAimLine.transform.localPosition =
            direction * radius;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        AnchorAimLine.transform.rotation =
            Quaternion.Euler(0f, 0f, angle - 90);
    }

    public Vector2 AnchorActivate()
    {
        StartCoroutine(AnchorRoutine());
        return aimDirection;
    }

    private IEnumerator AnchorRoutine()
    {
        anchorState = true;

        Time.timeScale = 0.05f;

        transposer.m_XDamping = 0.05f;
        transposer.m_YDamping = 0.05f;

        VirtualCamera.Follow = transform;
        AnchorAimLine.SetActive(true);



        // Wait until ThrowAction changes anchorState to false
        yield return new WaitUntil(() => anchorState == false);

        Time.timeScale = 1f;

        AnchorAimLine.SetActive(false);
        VirtualCamera.Follow = player.transform;

        transposer.m_XDamping = 1f;
        transposer.m_YDamping = 1f;
    }

    private Vector2 GetAimDirection()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldMousePosition =
            mainCamera.ScreenToWorldPoint(mousePosition);

        worldMousePosition.z = 0f;

        Vector2 direction =
            worldMousePosition - transform.position;

        return direction.normalized;
    }
}