using UnityEngine;
using DG.Tweening;

public class NotificationScript : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup canvasGroup;       // Assign in inspector
    public RectTransform panel;           // The UI panel to move
    public AudioClip clip;

    [Header("Settings")]
    public float slideDistance = 500f;    // How far off-screen it starts
    public float duration = 0.5f;
    public float displayTime = 3f;

    [Header("Help Mode")]
    public bool Help;
    public int deathsRequired = 5;

    private Vector2 originalPos;
    private bool isShowing = false;
    private Sequence currentSequence;
    private AudioSource audioSource;

    private bool playerInTrigger = false;
    private int deathCount = 0;
    private bool helpShown = false;

    void Start()
    {
        originalPos = panel.anchoredPosition;

        // Start hidden off-screen
        panel.anchoredPosition = originalPos + Vector2.left * slideDistance;
        canvasGroup.alpha = 0f;

        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag("Player") || isShowing)
            return;

        playerInTrigger = true;

        if(!Help && !isShowing)
        {
            ShowNotification();
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInTrigger = false;
    }

    public void RegisterDeath()
    {
        if (!Help || helpShown || !playerInTrigger)
            return;

        deathCount++;
        
        if (deathCount >= deathsRequired)
        {
            ShowNotification();
            helpShown = true;
        }
    }

    void ShowNotification()
    {
        isShowing = true;

        // Kill previous animation if somehow still running
        if (currentSequence != null && currentSequence.IsActive())
            currentSequence.Kill();

        currentSequence = DOTween.Sequence();

        // Slide in + fade in
        currentSequence.Append(
            panel.DOAnchorPos(originalPos, duration).SetEase(Ease.OutCubic)
        );
        currentSequence.Join(
            canvasGroup.DOFade(1f, duration)
        );

        audioSource.PlayOneShot(clip);

        // Wait
        currentSequence.AppendInterval(displayTime);

        // Slide out + fade out
        currentSequence.Append(
            panel.DOAnchorPos(originalPos + Vector2.left * slideDistance, duration)
                .SetEase(Ease.InCubic)
        );
        currentSequence.Join(
            canvasGroup.DOFade(0f, duration)
        );

        currentSequence.OnComplete(() =>
        {
            isShowing = false;
        });
    }


}