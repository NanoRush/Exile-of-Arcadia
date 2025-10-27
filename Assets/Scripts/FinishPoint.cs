using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{

    Collider2D coll;

    AudioSource source;
    public AudioClip clip;

    public GameObject FinishMenu;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coll.enabled = false;
            source.PlayOneShot(clip);
            FinishMenu.SetActive(true);
            Time.timeScale = 0f;
            PauseMenu.isPaused = true;
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        Application.Quit();
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
    }
}
