using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPreferences : MonoBehaviour
{
    [SerializeField] Image brightnessOverlay;
    public void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 0.5f);
        }

        if (!PlayerPrefs.HasKey("Brightness"))
        {
            PlayerPrefs.SetFloat("Brightness", 1f);
        }

        if (!PlayerPrefs.HasKey("AimLine"))
        {
            PlayerPrefs.SetInt("AimLine", 1);
        }

        if (!PlayerPrefs.HasKey("TripleJump"))
        {
            PlayerPrefs.SetInt("TripleJump", 0);
        }

        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        Color c = brightnessOverlay.color;
        c.a = Mathf.Lerp(0.7f, 0f, PlayerPrefs.GetFloat("Brightness"));
        brightnessOverlay.color = c;
        ToggleScript.AimLineOn = PlayerPrefs.GetInt("AimLine") == 1;
        ToggleTriple.TripleJumpOn = PlayerPrefs.GetInt("TripleJump") == 1;
    }
}
