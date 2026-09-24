using Unity.VisualScripting;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [SerializeField] private Door door;
    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private Color pressedColor = new Color32(26, 176, 109, 255); // #1AB06D

    private AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private int playersOnButton = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        normalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playersOnButton++;
        spriteRenderer.color = pressedColor;
        audioSource.PlayOneShot(openSound);

        if (playersOnButton == 1)
        {
            door.SetOpen(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playersOnButton--;
        spriteRenderer.color = normalColor;
        audioSource.PlayOneShot(closeSound);

        if (playersOnButton <= 0)
        {
            playersOnButton = 0;
            door.SetOpen(false);
        }
    }
}
