using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private PartMemberInfo[] allMembers;
    [SerializeField] private PartMemberInfo defaultPartyMember;

    private List<PartyMember> _currentParty;

    private void Awake()
    {
        _currentParty = new List<PartyMember>();
        AddMemberToPartyByName(defaultPartyMember.memberName);
    }

    public void AddMemberToPartyByName(string memberName)
    {
        foreach (var member in allMembers)
        {
            if (!member.memberName.Equals(memberName)) continue;
            var newPartyMember = new PartyMember
            {
                MemberName = member.name,
                Level = member.startingLevel,
                CurrentHealth = member.baseHealth,
                MaxHealth = member.baseHealth,
                Strength = member.baseStr,
                Initiative = member.baseInitiative,
                BattlePrefab = member.memberBattlePrefab,
                OverworldPrefab = member.memberOverworldPrefab
            };

            _currentParty.Add(newPartyMember);
            break;
        }
    }
}

[System.Serializable]
public class PartyMember
{
    public string MemberName { get; set; }
    public int Level { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public int Strength { get; set; }
    public int Initiative { get; set; }
    public int CurrentExp { get; set; }
    public int MaxExp { get; set; }
    public GameObject BattlePrefab { get; set; }
    public GameObject OverworldPrefab { get; set; }
}