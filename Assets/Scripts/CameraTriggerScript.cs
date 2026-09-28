using UnityEngine;
using Cinemachine;
using System;


public class CameraTriggerScript : MonoBehaviour
{

    public CinemachineVirtualCamera virtualCamera;
    public float yOffsetAmount;
    public float xOffsetAmount;
    public float zoomOutAmount;

    private CinemachineFramingTransposer framingTransposer;
    private float zoomSmoothTime = 0.5f;
    private Vector3 originialOffset;
    private float originalZoom;
    private float targetZoom;
    private float zoomVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        originialOffset = framingTransposer.m_TrackedObjectOffset;
        originalZoom = virtualCamera.m_Lens.OrthographicSize;
        targetZoom = originalZoom;
    }

    private void Update()
    {
        virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(virtualCamera.m_Lens.OrthographicSize, targetZoom, ref zoomVelocity, zoomSmoothTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            Vector3 newOffset = originialOffset;
            newOffset.y += yOffsetAmount;
            newOffset.x += xOffsetAmount;
            framingTransposer.m_TrackedObjectOffset = newOffset;
            targetZoom = originalZoom + zoomOutAmount;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            framingTransposer.m_TrackedObjectOffset = originialOffset;
            targetZoom = originalZoom;
        }
    }

}
