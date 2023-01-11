using Common.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scriptables.Player
{
    [ScriptableFactoryElement]
    public class PlayerHealthStats : SerializedScriptableObject
    {
        [SerializeField]
        private int _maxHealth;
     
        [SerializeField]
        private int _startingHealth;
        
        [SerializeField]
        private int _startingArmor;
        
        public int MaxHealth => _maxHealth;
        public int StartingArmor => _startingArmor;
        
        public int StartingHealth => _startingHealth;
    }
}