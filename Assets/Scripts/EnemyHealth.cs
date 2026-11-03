using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public string enemyType;
    private UnityEngine.Object enemyRef;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    public int health = 1;
    public GameObject explosion; 
    public AudioClip audioClip;
    private AudioSource AudioSource;
    private Vector3 spawnPosition;

    private WarperScript warper;

    // Start is called before the first frame update
    void Start()
    {
        enemyRef = Resources.Load(enemyType);
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        AudioSource = GetComponent<AudioSource>();
        spawnPosition = transform.position;
        warper = GetComponent<WarperScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            AudioSource.PlayOneShot(audioClip);
            sp.enabled = false;
            rb.simulated = false;
            Invoke("Respawn", 5);
            health = 1;
        }
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        if (CameraImpactManager.Instance != null)
        {
            if (health <= 0)
            {
                CameraImpactManager.Instance.HitImpact();
            }
        }
    }

    public void Respawn()
    {
        GameObject enemyClone = (GameObject)Instantiate(enemyRef);
        enemyClone.transform.position = spawnPosition;

        // Only transfer warp positions if this enemy is a Warper
        if (warper != null)
        {
            WarperScript newWarper = enemyClone.GetComponent<WarperScript>();

            if (newWarper != null)
            {
                newWarper.SetWarpPositions(warper.GetWarpPositions());
            }
        }

        Destroy(gameObject);
    }


}
