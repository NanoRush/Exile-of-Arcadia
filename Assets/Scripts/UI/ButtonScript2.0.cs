using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.InputSystem;

public class UIButtonTween : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler,
    ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public float hoverScale = 1.1f;
    public float pressedScale = 0.9f;
    public float duration = 0.2f;

    public ControllerVibration vibrationScript;

    private Vector3 originalScale;
    private Tween currentTween;

    void Awake()
    {
        DOTween.defaultUpdateType = UpdateType.Normal;
        DOTween.defaultTimeScaleIndependent = true;
    }

    void Start()
    {
        originalScale = transform.localScale;
    }

    void KillTween()
    {
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        KillTween();
        currentTween = transform.DOScale(originalScale * hoverScale, duration)
            .SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        KillTween();
        currentTween = transform.DOScale(originalScale, duration)
            .SetEase(Ease.OutQuad);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        KillTween();
        currentTween = transform.DOScale(originalScale * pressedScale, duration)
            .SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        KillTween();

        // Return to hover if still hovering, otherwise normal
        bool isHovering = RectTransformUtility.RectangleContainsScreenPoint(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera
        );

        Vector3 target = isHovering ? originalScale * hoverScale : originalScale;

        currentTween = transform.DOScale(target, duration)
            .SetEase(Ease.OutBack); // nice bounce feel
    }

    public void OnSelect(BaseEventData eventData)
    {
        KillTween();
        currentTween = transform.DOScale(originalScale * hoverScale, duration)
            .SetEase(Ease.OutQuad);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        KillTween();
        currentTween = transform.DOScale(originalScale, duration)
            .SetEase(Ease.OutQuad);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        KillTween();

        // Quick press feedback for controller
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(originalScale * pressedScale, duration * 0.5f));
        seq.Append(transform.DOScale(originalScale * hoverScale, duration * 0.5f));

        currentTween = seq;
        vibrationScript.Vibrate(0f, 0.1f, 0.1f);
    }
}