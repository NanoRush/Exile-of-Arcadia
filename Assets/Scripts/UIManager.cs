using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerAttack playerAttack;
    public Image cooldownBar;
    public Color readyColor;

    public PlayerHealth playerHealth;
    public TextMeshProUGUI deathText;

    private void Start()
    {
        playerAttack.OnCooldownChanged += UpdateBar;
        playerAttack.OnCooldownComplete += CooldownReady;
        playerAttack.OnCooldownStarted += CooldownStarted;
        playerHealth.OnDeathCountChanged += UpdateDeathText;
        playerHealth.OnPlayerDied += ShowDeathText;
        playerHealth.OnPlayerRespawn += HideDeathText;
    }

    void UpdateBar(float percent)
    {
        cooldownBar.fillAmount = percent;
    }

    void CooldownReady()
    {
        cooldownBar.color = readyColor;
    }

    void CooldownStarted()
    {
        cooldownBar.color = Color.red;
    }

    void UpdateDeathText(int count)
    {
        deathText.text = "Deaths: " + count;
    }

    void ShowDeathText()
    {
        deathText.alpha = 1;
    }

    void HideDeathText()
    {
        deathText.alpha = 0;
    }

    private void OnDestroy()
    {
        playerHealth.OnDeathCountChanged -= UpdateDeathText;
    }

}
