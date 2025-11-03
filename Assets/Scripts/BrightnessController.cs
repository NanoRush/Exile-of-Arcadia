using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    [SerializeField] Slider brightnessSlider;
    [SerializeField] Image brightnessOverlay;
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    public void ChangeBrightness()
    {
        Color c = brightnessOverlay.color;
        c.a = Mathf.Lerp(0.7f, 0f, brightnessSlider.value);
        brightnessOverlay.color = c;
        Save();
    }

    public void Load()
    {
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
    }

}
