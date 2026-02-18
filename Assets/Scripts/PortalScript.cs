using System;
using System.Collections;
using UnityEngine;

public class PortalScript : MonoBehaviour
{

    public GameObject Out;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("dagger") && !collision.CompareTag("Player"))
            return;

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Save incoming speed
        float speed = rb.linearVelocity.magnitude;

        // Teleport
        collision.transform.position = Out.transform.position;

        // Get exit direction
        Vector2 exitDirection = Out.transform.right.normalized;

        // Redirect momentum
        rb.linearVelocity = exitDirection * speed;

        // Rotate object to face exit direction
        if (collision.CompareTag("dagger"))
        {
            float angle = Mathf.Atan2(exitDirection.y, exitDirection.x) * Mathf.Rad2Deg;
            collision.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
        StartCoroutine(TeleportCooldown(1f));
    }

    public IEnumerator TeleportCooldown(float seconds)
    {
        Out.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(seconds);
        Out.GetComponent<Collider2D>().enabled = true;
    }
}
