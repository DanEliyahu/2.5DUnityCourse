using UnityEngine;

[CreateAssetMenu(menuName = "New Party Member")]
public class PartMemberInfo : ScriptableObject
{
    public string memberName;
    public int startingLevel;
    public int baseHealth;
    public int baseStr;
    public int baseInitiative;
    public GameObject memberBattlePrefab;
    public GameObject memberOverworldPrefab;
}
