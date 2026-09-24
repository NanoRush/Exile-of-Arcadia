using UnityEngine;
using DG.Tweening;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float moveHeight = 5f;
    [SerializeField] private float moveDuration = 1f;

    private Vector3 bottomPosition;
    private Vector3 topPosition;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private Color normalColor;
    private Color pressedColor = new Color32(26, 176, 109, 255); // #1AB06D

    public AudioClip upSound;
    public AudioClip downSound;


    private int playersOnElevator = 0;

    private void Start()
    {
        bottomPosition = transform.position;
        topPosition = bottomPosition + Vector3.up * moveHeight;

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        normalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playersOnElevator++;

        if (playersOnElevator == 1)
        {
            spriteRenderer.color = pressedColor;
            audioSource.PlayOneShot(upSound);

            MoveUp();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playersOnElevator--;

        if (playersOnElevator <= 0)
        {
            playersOnElevator = 0;

            spriteRenderer.color = normalColor;
            audioSource.PlayOneShot(downSound);

            MoveDown();
        }
    }

    private void MoveUp()
    {
        transform.DOKill();

        transform.DOMove(topPosition, moveDuration)
            .SetEase(Ease.InOutQuad);
    }

    private void MoveDown()
    {
        transform.DOKill();

        transform.DOMove(bottomPosition, moveDuration)
            .SetEase(Ease.InOutQuad);
    }
}