using UnityEngine;

public class AirSlashScript : MonoBehaviour
{
    Rigidbody2D playerRb;

    [SerializeField] private float bounceForce = 12f;
    private PlayerMovement playerMovement;

    private EnemyHealth EnemyHealth;

    void Start()
    {
        playerRb = GetComponentInParent<Rigidbody2D>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            EnemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (EnemyHealth.enemyType == "Warper" && EnemyHealth.health == 1)
            {
                HitStop.Instance.Freeze(0.15f);
            }
            else
            {
                HitStop.Instance.Freeze(0.05f);
            }
            EnemyHealth.TakeDamage(1);
            playerMovement.resetJump();
            // Sharp upward bounce
            playerRb.linearVelocity = new Vector2(
                playerRb.linearVelocity.x,
                bounceForce
            );
        }
    }
}