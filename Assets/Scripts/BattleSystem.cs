using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private PartyManager _partyManager;
    private EnemyManager _enemyManager;

    [SerializeField] private List<BattleEntity> allBattlers = new List<BattleEntity>();
    [SerializeField] private List<BattleEntity> enemyBattlers = new List<BattleEntity>();
    [SerializeField] private List<BattleEntity> playerBattlers = new List<BattleEntity>();

    // Start is called before the first frame update
    void Start()
    {
        _partyManager = FindFirstObjectByType<PartyManager>();
        _enemyManager = FindFirstObjectByType<EnemyManager>();
        
        CreatePartyEntities();
        CreateEnemyEntities();
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

            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);
        }
    }
}

[System.Serializable]
public class BattleEntity
{
    public string Name;
    public int CurrHealth;
    public int MaxHealth;
    public int Initiative;
    public int Strength;
    public int Level;
    public bool IsPlayer;

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
}