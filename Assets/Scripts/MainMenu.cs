using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(Load(1));
    }

    public void PlayLevel2()
    {
        StartCoroutine(Load(2));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator Load(int level)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(level);
    }

}
