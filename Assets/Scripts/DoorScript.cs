using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private float openHeight = 3f;
    [SerializeField] private float moveDuration = 0.5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        closedPosition = door.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    public void SetOpen(bool open)
    {
        door.DOKill();

        if (open)
        {
            door.DOMove(openPosition, moveDuration)
                .SetEase(Ease.OutQuad);
        }
        else
        {
            door.DOMove(closedPosition, moveDuration)
                .SetEase(Ease.InQuad);
        }
    }
}