using UnityEngine;
using Cinemachine;
using System;


public class CameraTriggerScript : MonoBehaviour
{

    public CinemachineVirtualCamera virtualCamera;
    public float yOffsetAmount;
    public float xOffsetAmount;

    private CinemachineFramingTransposer framingTransposer;
    private Vector3 originialOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        originialOffset = framingTransposer.m_TrackedObjectOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            Vector3 newOffset = originialOffset;
            newOffset.y += yOffsetAmount;
            newOffset.x += xOffsetAmount;
            framingTransposer.m_TrackedObjectOffset = newOffset;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            framingTransposer.m_TrackedObjectOffset = originialOffset;
        }
    }

}
