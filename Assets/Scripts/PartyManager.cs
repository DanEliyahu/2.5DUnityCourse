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
        foreach (var memberInfo in allMembers)
        {
            if (!memberInfo.memberName.Equals(memberName)) continue;
            var newPartyMember = new PartyMember
            {
                MemberName = memberInfo.name,
                Level = memberInfo.startingLevel,
                CurrentHealth = memberInfo.baseHealth,
                MaxHealth = memberInfo.baseHealth,
                Strength = memberInfo.baseStr,
                Initiative = memberInfo.baseInitiative,
                BattlePrefab = memberInfo.memberBattlePrefab,
                OverworldPrefab = memberInfo.memberOverworldPrefab
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