using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int health;
    private SpriteRenderer SpriteRenderer;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        health = 1;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            SpriteRenderer.sprite = null;
            rb.simulated = false;
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
            Invoke(nameof(Reload), 0.8f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("spike"))
        {
            health--;
        }
    }

    private void Reload()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
