using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    public static bool AimLineOn;
    [SerializeField] public Toggle toggle;

    public void Start()
    {
        Load();
    }
    public void ToggleAim()
    {
        AimLineOn = toggle.isOn;
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("AimLine", (AimLineOn ? 1 : 0));
    }

    public void Load()
    {
        AimLineOn = (PlayerPrefs.GetInt("AimLine") != 0);
        toggle.isOn = AimLineOn;
    }

}
