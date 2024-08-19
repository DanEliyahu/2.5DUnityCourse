using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] allEnemies;

    private List<Enemy> _currentEnemies;

    private void Awake()
    {
        _currentEnemies = new List<Enemy>();
        GenerateEnemyByName("Slime", 1);
    }

    private void GenerateEnemyByName(string enemyName, int level)
    {
        foreach (var enemyInfo in allEnemies)
        {
            if (!enemyInfo.enemyName.Equals(enemyName)) continue;
            var newEnemy = new Enemy(enemyInfo, level);
            _currentEnemies.Add(newEnemy);
        }
    }

    public List<Enemy> GetCurrentEnemies()
    {
        return _currentEnemies;
    }
}

[System.Serializable]
public class Enemy
{
    public string enemyName;
    public int level;
    public int currHealth;
    public int maxHealth;
    public int strength;
    public int initiative;
    public GameObject prefab;

    private const float LevelModifier = 0.5f;

    public Enemy(EnemyInfo info, int level)
    {
        this.level = level;
        this.enemyName = info.enemyName;
        var levelModifier = LevelModifier * this.level;

        this.maxHealth = Mathf.RoundToInt(info.baseHealth + (info.baseHealth * levelModifier));
        this.currHealth = this.maxHealth;
        this.strength = Mathf.RoundToInt(info.baseStr + (info.baseStr * levelModifier));
        this.initiative = Mathf.RoundToInt(info.baseInitiative + (info.baseInitiative * levelModifier));
        this.prefab = info.enemyPrefab;
    }
}