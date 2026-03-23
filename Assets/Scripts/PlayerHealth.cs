using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int health;
    private SpriteRenderer SpriteRenderer;
    private Collider2D col;
    private Rigidbody2D rb;
    public Canvas canvas;
    Vector2 checkpointPos;
    bool Respawning = false;

    private AudioSource audioSource;
    public AudioClip deathSound;
    TextMeshProUGUI DeathCounter;
    int DeathCount = 0;

    public GameObject deathWave;

    public event Action OnPlayerDied;
    public event Action OnPlayerRespawn;
    public event Action<int> OnDeathCountChanged;

    // Start is called before the first frame update
    void Start()
    {
        health = 1;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        checkpointPos = transform.position;
        DeathCounter = canvas.GetComponentInChildren<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("spike"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!Respawning)
        {
            health -= damage;

            if(health <= 0)
            {
                HandleDeath();
            }
        }
    }

    private void HandleDeath()
    {
        if (!Respawning)
        {
            Respawning = true;

            SpriteRenderer.enabled = false;
            rb.simulated = false;
            col.enabled = false;
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
            canvas.transform.GetChild(0).gameObject.SetActive(true);

            Instantiate(deathWave, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(deathSound);

            DeathCount++;

            OnDeathCountChanged?.Invoke(DeathCount);
            OnPlayerDied?.Invoke();
            Invoke(nameof(Respawn), 0.8f);
            Invoke(nameof(DeathTextAppear), 2.5f);
        }
    }

    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpointPos = pos;
    }

    public void Respawn()
    {
        Respawning = false;
        transform.position = checkpointPos;
        health = 1;
        SpriteRenderer.enabled = true;
        rb.simulated = true;
        rb.linearVelocity = Vector2.zero;
        col.enabled = true;
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
        canvas.transform.GetChild(0).gameObject.SetActive(false);

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("enemy");
        if (enemyObjects.Length > 0)
        {
            foreach (GameObject enemyObject in enemyObjects)
            {
                enemyObject.GetComponent<EnemyHealth>().Respawn();
            }
        }

    }

    public void DeathTextAppear()
    {
        OnPlayerRespawn?.Invoke();
    }

}
