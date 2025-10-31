using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public static bool isPaused;
    [SerializeField] Image brightnessOverlay;
    void Start()
    {
        pauseMenu.SetActive(false);
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        Color c = brightnessOverlay.color;
        c.a = Mathf.Lerp(0.7f, 0f, PlayerPrefs.GetFloat("Brightness"));
        brightnessOverlay.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (settingsMenu.activeInHierarchy)
        {
            settingsMenu.SetActive(false);
        }
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        isPaused = false;
    }

}
