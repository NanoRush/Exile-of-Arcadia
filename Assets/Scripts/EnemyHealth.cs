using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public string enemyType;
    private UnityEngine.Object enemyRef;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    public int health;
    public GameObject explosion; 
    public AudioClip audioClip;
    private AudioSource AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        enemyRef = Resources.Load(enemyType);
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        health = 1;
        AudioSource = GetComponent<AudioSource>();
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
    }

    public void Respawn()
    {
        GameObject enemyClone = (GameObject)Instantiate(enemyRef);
        enemyClone.transform.position = transform.position;
        Destroy(gameObject);
    }


}
