using UnityEngine;
using UnityEngine.UI;

public class ToggleTriple : MonoBehaviour
{
    public static bool TripleJumpOn;
    [SerializeField] public Toggle toggle;

    public void Start()
    {
        Load();
    }
    public void ToggleJump()
    {
        TripleJumpOn = toggle.isOn;
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("TripleJump", (TripleJumpOn ? 1 : 0));
    }

    public void Load()
    {
        TripleJumpOn = (PlayerPrefs.GetInt("TripleJump") != 0);
        toggle.isOn = TripleJumpOn;
    }

}
