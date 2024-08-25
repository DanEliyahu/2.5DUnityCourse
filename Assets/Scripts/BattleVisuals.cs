using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleVisuals : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private Animator _animator;

    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Die = Animator.StringToHash("Die");


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetStartingValues(int currentHealth, int maxHealth, int level)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        levelText.text = level.ToString();
    }

    public void ChangeHealth(int newValue)
    {
        healthBar.value = newValue;
        if (newValue <= 0)
        {
            PlayDeathAnimation();
            Destroy(gameObject, 1f);
        }
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(Attack);
    }

    public void PlayHitAnimation()
    {
        _animator.SetTrigger(Hit);
    }

    public void PlayDeathAnimation()
    {
        _animator.SetTrigger(Die);
    }
}