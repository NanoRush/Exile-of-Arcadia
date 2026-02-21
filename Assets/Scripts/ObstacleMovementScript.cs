using UnityEngine;

public class ElectricBlockMovement : MonoBehaviour
{
    public float moveDistance = 3f;
    public float speed = 2f;
    public Vector2 direction = Vector2.up; // Change in Inspector

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, moveDistance);
        transform.position = startPosition + (Vector3)direction.normalized * offset;
    }
}