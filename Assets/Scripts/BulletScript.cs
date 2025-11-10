using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    private Rigidbody2D rb;
    private GameObject player;
    public float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Stark");
        rb = GetComponent<Rigidbody2D>();
        Vector3 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        Destroy(gameObject, 3f);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("spike") || collision.gameObject.CompareTag("enemy"))
        {
            Destroy(gameObject);
        }
    }


}
