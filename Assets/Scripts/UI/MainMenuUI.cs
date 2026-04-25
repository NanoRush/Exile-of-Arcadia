using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;


public class MainMenuUI : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public CanvasGroup LevelSelectGroup;
    public float defaultDuration;
    public float fadeDuration;
    public Button playButton;

    public GameObject settingsMenu;
    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject controlsMenu;

    private GameObject currentMenu;
    private bool LevelsOn;

    [Header("Level Details")]
    public CanvasGroup levelDetailsContainer;   // Whole panel (slides in/out)

    public CanvasGroup level1Group;
    public CanvasGroup level2Group;

    public RectTransform levelDetailsPanel;     // The thing that moves

    private Vector2 detailsOriginalPos;
    private int currentLevel = -1; // no level selected initially
    private Sequence detailsSequence;
    private bool detailsVisible = false;

    private void Start()
    {
        canvasGroup.DOFade(1f, defaultDuration);
        LevelsOn = false;

        detailsOriginalPos = levelDetailsPanel.anchoredPosition;

        // Start hidden off-screen
        levelDetailsPanel.anchoredPosition = detailsOriginalPos + Vector2.left * 600f;
        levelDetailsContainer.alpha = 0f;

        level1Group.alpha = 0f;
        level2Group.alpha = 0f;
    }

    public void SwitchToSettings()
    {
        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            if (currentMenu == controlsMenu) {
                currentMenu.SetActive(false);
                currentMenu = settingsMenu;
            }
            else
            {
                currentMenu = settingsMenu;
                mainMenu.SetActive(false);
            }

            settingsMenu.SetActive(true);

            // Reassign to new menu's CanvasGroup

            canvasGroup.DOFade(1f, fadeDuration);
        });
    }

    public void SwitchToMain()
    {
        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            currentMenu.SetActive(false);

            mainMenu.SetActive(true);

            // Reassign to new menu's CanvasGroup

            canvasGroup.DOFade(1f, fadeDuration);
        });
    }

    public void SwitchToCredits()
    {
        currentMenu = creditsMenu;
        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            mainMenu.SetActive(false);

            creditsMenu.SetActive(true);

            // Reassign to new menu's CanvasGroup

            canvasGroup.DOFade(1f, fadeDuration);
        });
    }

    public void SwitchToControls()
    {
        currentMenu = controlsMenu;
        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            settingsMenu.SetActive(false);

            controlsMenu.SetActive(true);

            // Reassign to new menu's CanvasGroup

            canvasGroup.DOFade(1f, fadeDuration);
        });
    }

    public void ShowLevels()
    {
        float offsetY = 200f;

        // Kill group fade tween
        LevelSelectGroup.DOKill();

        if (LevelsOn == false)
        {
            LevelsOn = true;

            LevelSelectGroup.DOFade(1f, fadeDuration);

            for (int x = 0; x < LevelSelectGroup.transform.childCount; x++)
            {
                RectTransform child = LevelSelectGroup.transform.GetChild(x).GetComponent<RectTransform>();

                child.DOKill();

                child.gameObject.SetActive(true);

                // Get or add original position
                if (!child.TryGetComponent(out OriginalPos pos))
                {
                    pos = child.gameObject.AddComponent<OriginalPos>();
                    pos.value = child.anchoredPosition;
                }

                // Start above
                child.anchoredPosition = pos.value + new Vector2(0, offsetY);

                // Animate into place
                child.DOAnchorPos(pos.value, fadeDuration)
                     .SetEase(Ease.OutCubic)
                     .SetDelay(x * 0.05f);
            }

            Button firstButton = LevelSelectGroup.transform.GetChild(0).GetComponent<Button>();

            DOVirtual.DelayedCall(fadeDuration * 0.2f, () =>
            {
                firstButton.Select();
            });
        }
        else
        {
            LevelsOn = false;

            LevelSelectGroup.DOFade(0f, fadeDuration);

            for (int x = 0; x < LevelSelectGroup.transform.childCount; x++)
            {
                RectTransform child = LevelSelectGroup.transform.GetChild(x).GetComponent<RectTransform>();

                child.DOKill();

                OriginalPos pos = child.GetComponent<OriginalPos>();

                // Animate downward
                child.DOAnchorPos(pos.value - new Vector2(0, offsetY), fadeDuration)
                     .SetEase(Ease.InCubic)
                     .SetDelay(x * 0.05f)
                     .OnComplete(() =>
                     {
                         // Only disable if we're STILL in "off" state
                         if (!LevelsOn)
                             child.gameObject.SetActive(false);
                     });
            }

            playButton.Select();
        }
    }

    public void ToggleLevelDetails()
    {
        levelDetailsPanel.DOKill();
        levelDetailsContainer.DOKill();

        float slideDistance = 600f;

        if (!detailsVisible)
        {
            // SHOW
            detailsVisible = true;

            levelDetailsContainer.alpha = 1f;

            levelDetailsPanel.DOAnchorPos(detailsOriginalPos, fadeDuration)
                .SetEase(Ease.OutCubic);
        }
        else
        {
            // HIDE
            detailsVisible = false;

            levelDetailsPanel.DOAnchorPos(detailsOriginalPos + Vector2.left * slideDistance, fadeDuration)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    levelDetailsContainer.alpha = 0f;
                });
        }
    }

    public void ShowLevel(int levelIndex)
    {
        // Prevent reselecting same level
        if (currentLevel == levelIndex) return;

        currentLevel = levelIndex;

        CanvasGroup targetGroup = null;
        CanvasGroup previousGroup = null;

        if (levelIndex == 1)
        {
            targetGroup = level1Group;
            previousGroup = level2Group;
        }
        else if (levelIndex == 2)
        {
            targetGroup = level2Group;
            previousGroup = level1Group;
        }

        // Kill previous animations
        if (detailsSequence != null && detailsSequence.IsActive())
            detailsSequence.Kill();

        detailsSequence = DOTween.Sequence();

        // Fade OUT previous
        if (previousGroup != null)
        {
            detailsSequence.Append(
                previousGroup.DOFade(0f, fadeDuration * 0.5f)
            );
        }

        // Fade IN new
        if (targetGroup != null)
        {
            detailsSequence.Append(
                targetGroup.DOFade(1f, fadeDuration * 0.5f)
            );
        }
    }



}
