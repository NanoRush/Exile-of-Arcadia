using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    private Transform player;
    private Vector2 directionToPlayer;

    [SerializeField] private Transform Bullet;
    public Transform firePoint;

    public float shootCooldown;
    [HideInInspector] public bool playerInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shootCooldown = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 180);
            if(shootCooldown > 2.5f)
            {
                Instantiate(Bullet, firePoint.position, Quaternion.identity);
                shootCooldown = 0f;
            }
            shootCooldown += Time.deltaTime;
        }
    }
}
