using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPreferences : MonoBehaviour
{
    [SerializeField] Image brightnessOverlay;
    public void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        Color c = brightnessOverlay.color;
        c.a = Mathf.Lerp(0.7f, 0f, PlayerPrefs.GetFloat("Brightness"));
        brightnessOverlay.color = c;
    }
}
