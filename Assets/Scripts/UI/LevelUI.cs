using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;


public class LevelUI : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public float defaultDuration;
    public float fadeDuration;

    public GameObject settingsMenu;
    public GameObject mainMenu;

    private GameObject currentMenu;

    private void Start()
    {
        canvasGroup.DOFade(1f, defaultDuration).SetUpdate(true);
    }

    public void SwitchToSettings()
    {
        canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
           
            currentMenu = settingsMenu;
            mainMenu.SetActive(false);

            settingsMenu.SetActive(true);

            // Reassign to new menu's CanvasGroup

            canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);
        });
    }

    public void SwitchToMain()
    {
        canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            currentMenu.SetActive(false);

            mainMenu.SetActive(true);

            // Reassign to new menu's CanvasGroup

            canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);
        });
    }



}
