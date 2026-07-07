using System.Collections.Generic;
using Data.ActionData;
using UnityEngine;

namespace Data.CharacterData
{
    [CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Scriptable Objects/CharacterDataSO")]
    public class CharacterDataSo : ScriptableObject
    {
        public string CharacterName;
        public float MaxHealth;
        public float Damage;
        public float Speed;
        public float Def;
        public GameObject CombatPrefab;
        public List<BattleActionSO> Actions = new();
        public string AttackSFX;
        public string DamagedSFX;
    }
}
