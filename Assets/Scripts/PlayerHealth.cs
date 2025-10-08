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

    TextMeshProUGUI DeathCounter;
    int DeathCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        health = 1;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        checkpointPos = transform.position;
        DeathCounter = canvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (!Respawning)
            {
                Respawning = true;
                SpriteRenderer.enabled = false;
                rb.simulated = false;
                col.enabled = false;
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
                canvas.transform.GetChild(0).gameObject.SetActive(true);
                DeathCount++;
                DeathCounter.text = "Deaths: " + DeathCount;
                Invoke(nameof(Respawn), 0.8f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("spike"))
        {
            health--;
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
        rb.velocity = Vector2.zero;
        col.enabled = true;
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
        canvas.transform.GetChild(0).gameObject.SetActive(false);
    }

}
