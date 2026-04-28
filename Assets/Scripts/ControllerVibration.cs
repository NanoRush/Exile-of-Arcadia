using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ControllerVibration : MonoBehaviour
{
    private Coroutine vibrationCoroutine;

    public void Vibrate(float lowFrequency, float highFrequency, float duration)
    {
        if (Gamepad.current == null)
            return;

        // Stop any existing vibration timer first
        if (vibrationCoroutine != null)
            StopCoroutine(vibrationCoroutine);

        Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);

        // Use realtime seconds (works even when paused)
        vibrationCoroutine = StartCoroutine(StopVibrationAfterDelay(duration));
    }

    private IEnumerator StopVibrationAfterDelay(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        StopVibration();
    }

    public void StopVibration()
    {
        if (Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(0f, 0f);

        vibrationCoroutine = null;
    }

    private void OnDisable()
    {
        StopVibration();
    }

    private void OnDestroy()
    {
        StopVibration();
    }
}