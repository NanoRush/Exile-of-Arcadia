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
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 start = AimPoint.transform.position;
        start.z = 0;

        Vector2 direction = (mouseWorldPos - start).normalized;

        if (Vector2.Dot(direction, AimPoint.up) < 0f)
        {
            direction = AimPoint.up;  // clamp direction forward
        }

        lr.SetPosition(0, start);

        RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, maxDistance);

        // default = full length
        Vector3 endPoint = start + (Vector3)(direction * maxDistance);

        foreach (RaycastHit2D h in hits)
        {
            if (h.collider.CompareTag("Player")) continue;
            if (h.collider.CompareTag("dagger")) continue;

            // first valid hit
            endPoint = h.point;
            break;
        }

        lr.SetPosition(1, endPoint);
    }


}
