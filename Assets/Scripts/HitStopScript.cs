
using UnityEngine;
using System.Collections;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Freeze(float duration)
    {
        StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
    }
}
