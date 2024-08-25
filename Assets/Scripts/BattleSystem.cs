using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private enum BattleState
    {
        Start,
        Selection,
        Battle,
        Won,
        Lost,
        Run
    }

    [Header("Battle State")] [SerializeField]
    private BattleState battleState;

    [Header("Spawn Points")] [SerializeField]
    private Transform[] partySpawnPoints;

    [SerializeField] private Transform[] enemySpawnPoints;

    [Header("Battlers")] [SerializeField] private List<BattleEntity> allBattlers = new();
    [SerializeField] private List<BattleEntity> enemyBattlers = new();
    [SerializeField] private List<BattleEntity> playerBattlers = new();

    [Header("UI")] [SerializeField] private GameObject[] enemySelectionButtons;
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject enemySelectionMenu;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private GameObject bottomTextPopUp;
    [SerializeField] private TextMeshProUGUI bottomText;

    private PartyManager _partyManager;
    private EnemyManager _enemyManager;
    private int _currentPlayer;

    private const string ActionMessage = "'s Actions:";
    private const string WinMessage = "Your party won the battle!";

    // Start is called before the first frame update
    void Start()
    {
        _partyManager = FindFirstObjectByType<PartyManager>();
        _enemyManager = FindFirstObjectByType<EnemyManager>();

        CreatePartyEntities();
        CreateEnemyEntities();
        ShowBattleMenu();
    }

    private IEnumerator BattleRoutine()
    {
        enemySelectionMenu.SetActive(false);
        battleState = BattleState.Battle;
        bottomTextPopUp.SetActive(true);

        for (int i = 0; i < allBattlers.Count; i++)
        {
            var battler = allBattlers[i];
            switch (battler.BattleAction)
            {
                case BattleEntity.BattleActionEnum.Attack:
                    yield return AttackRoutine(battler, allBattlers[battler.Target]);
                    break;
                case BattleEntity.BattleActionEnum.Run:
                    break;
                default:
                    Debug.LogError("Error - invalid battle action for " + battler.Name);
                    break;
            }
        }

        if (battleState == BattleState.Battle)
        {
            bottomTextPopUp.SetActive(false);
            _currentPlayer = 0;
            ShowBattleMenu();
        }
    }

    private IEnumerator AttackRoutine(BattleEntity attacker, BattleEntity target)
    {
        // player's turn
        if (attacker.IsPlayer)
        {
            AttackAction(attacker, target);
            // wait for attack animation to finish
            while (attacker.BattleVisuals.IsAttacking)
            {
                yield return null;
            }

            if (target.CurrHealth <= 0)
            {
                enemyBattlers.Remove(target);
                allBattlers.Remove(target);
                if (enemyBattlers.Count == 0)
                {
                    battleState = BattleState.Won;
                    bottomText.text = WinMessage;
                }
            }
        }

        // enemy's turn
    }

    private void CreatePartyEntities()
    {
        List<PartyMember> currentParty = _partyManager.GetCurrentParty();

        for (int i = 0; i < currentParty.Count; i++)
        {
            PartyMember tempMember = currentParty[i];
            BattleEntity tempEntity = new BattleEntity();
            tempEntity.SetEntityValues(tempMember.MemberName, tempMember.CurrentHealth, tempMember.MaxHealth,
                tempMember.Initiative, tempMember.Strength, tempMember.Level, true);

            BattleVisuals tempBattleVisuals =
                Instantiate(tempMember.BattlePrefab, partySpawnPoints[i].position, Quaternion.identity)
                    .GetComponent<BattleVisuals>();
            tempBattleVisuals.SetStartingValues(tempMember.MaxHealth, tempMember.MaxHealth, tempMember.Level);
            tempEntity.BattleVisuals = tempBattleVisuals;

            allBattlers.Add(tempEntity);
            playerBattlers.Add(tempEntity);
        }
    }

    private void CreateEnemyEntities()
    {
        List<Enemy> currentEnemies = _enemyManager.GetCurrentEnemies();

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            Enemy tempEnemy = currentEnemies[i];
            BattleEntity tempEntity = new BattleEntity();
            tempEntity.SetEntityValues(tempEnemy.enemyName, tempEnemy.currHealth, tempEnemy.maxHealth,
                tempEnemy.initiative, tempEnemy.strength, tempEnemy.level, false);

            BattleVisuals tempBattleVisuals =
                Instantiate(tempEnemy.prefab, enemySpawnPoints[i].position, Quaternion.identity)
                    .GetComponent<BattleVisuals>();
            tempBattleVisuals.SetStartingValues(tempEnemy.maxHealth, tempEnemy.maxHealth, tempEnemy.level);
            tempEntity.BattleVisuals = tempBattleVisuals;

            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);
        }
    }

    private void ShowBattleMenu()
    {
        actionText.text = playerBattlers[_currentPlayer].Name + ActionMessage;
        battleMenu.SetActive(true);
    }

    public void ShowEnemySelectionMenu()
    {
        battleMenu.SetActive(false);
        SetEnemySelectionButtons();
        enemySelectionMenu.SetActive(true);
    }

    private void SetEnemySelectionButtons()
    {
        //disable buttons
        foreach (var enemySelectionButton in enemySelectionButtons)
        {
            enemySelectionButton.SetActive(false);
        }

        for (int i = 0; i < enemyBattlers.Count; i++)
        {
            enemySelectionButtons[i].SetActive(true);
            var enemyText = enemySelectionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (enemyText != null)
            {
                enemyText.text = enemyBattlers[i].Name;
            }
        }
    }

    public void SelectEnemy(int currentEnemy)
    {
        // setting the current members target
        BattleEntity currentPlayerEntity = playerBattlers[_currentPlayer];
        currentPlayerEntity.SetTarget(allBattlers.IndexOf(enemyBattlers[currentEnemy]));

        currentPlayerEntity.BattleAction = BattleEntity.BattleActionEnum.Attack;
        _currentPlayer++;

        if (_currentPlayer >= playerBattlers.Count)
        {
            StartCoroutine(BattleRoutine());
        }
        else
        {
            enemySelectionMenu.SetActive(false);
            ShowBattleMenu();
        }
    }

    private void AttackAction(BattleEntity attacker, BattleEntity target)
    {
        int damage = attacker.Strength;
        attacker.BattleVisuals.PlayAttackAnimation();
        target.CurrHealth -= damage;
        target.BattleVisuals.PlayHitAnimation();
        target.UpdateUI();

        bottomText.text = $"{attacker.Name} attacks {target.Name} for {damage} damage";
    }
}

[System.Serializable]
public class BattleEntity
{
    public enum BattleActionEnum
    {
        Attack,
        Run
    }

    public BattleActionEnum BattleAction;
    public string Name;
    public int CurrHealth;
    public int MaxHealth;
    public int Initiative;
    public int Strength;
    public int Level;
    public bool IsPlayer;
    public BattleVisuals BattleVisuals;
    public int Target;

    public void SetEntityValues(string name, int currHealth, int maxHealth, int initiative, int strength, int level,
        bool isPlayer)
    {
        Name = name;
        CurrHealth = currHealth;
        MaxHealth = maxHealth;
        Initiative = initiative;
        Strength = strength;
        Level = level;
        IsPlayer = isPlayer;
    }

    public void SetTarget(int target)
    {
        Target = target;
    }

    public void UpdateUI()
    {
        BattleVisuals.ChangeHealth(CurrHealth);
    }
}