using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy")]
public class EnemyInfo : ScriptableObject
{
    public string enemyName;
    public int baseHealth;
    public int baseStr;
    public int baseInitiative;
    public GameObject enemyPrefab;
}
