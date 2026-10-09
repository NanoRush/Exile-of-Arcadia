using UnityEngine;

public class AirSlashScript : MonoBehaviour
{
    Rigidbody2D playerRb;

    [SerializeField] private float bounceForce = 12f;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerRb = GetComponentInParent<Rigidbody2D>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);
            playerMovement.resetJump();
            // Sharp upward bounce
            playerRb.linearVelocity = new Vector2(
                playerRb.linearVelocity.x,
                bounceForce
            );
        }
    }
}