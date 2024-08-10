using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleVisuals : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        SetStartingValues(10, 15, 56);
    }

    public void SetStartingValues(int currentHealth, int maxHealth, int level)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        levelText.text = level.ToString();
    }
}