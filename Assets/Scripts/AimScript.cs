using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
    // Start is called before the first frame update
    private LineRenderer lr;
    public float maxDistance = 100f;
    private Transform player;
    private Transform AimPoint;

    [SerializeField] private Cursor cursorScript;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        player = GameObject.Find("Stark").transform;
        AimPoint = GameObject.Find("DaggerTransform").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = cursorScript.AimDirection;

        Vector3 start = AimPoint.position;
        start.z = 0;

        lr.SetPosition(0, start);

        RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, maxDistance);

        Vector3 endPoint = start + (Vector3)(direction * maxDistance);

        foreach (RaycastHit2D h in hits)
        {
            if (h.collider.CompareTag("Player")) continue;
            if (h.collider.CompareTag("dagger")) continue;
            if (h.collider.CompareTag("trigger")) continue;

            endPoint = h.point;
            break;
        }

        lr.SetPosition(1, endPoint);
    }


}
