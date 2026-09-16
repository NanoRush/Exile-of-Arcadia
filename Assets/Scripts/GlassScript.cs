using System.Collections;
using UnityEngine;


public class GlassScript : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip clip;
    public GameObject building;
    public ParticleSystem glassParticles;
    
    private bool isShattered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isShattered)
        {
            building.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            audioSource.PlayOneShot(clip);
            glassParticles.Play();
            isShattered = true;
        }
    }



}
