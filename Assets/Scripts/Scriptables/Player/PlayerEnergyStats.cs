using Sirenix.OdinInspector;
using UnityEngine;

namespace Scriptables.Player
{
    public class PlayerEnergyStats : SerializedScriptableObject
    {
        [SerializeField]
        private int _startingEnergy;
        
        [SerializeField]
        private int _maxEnergy;
        
        public int MaxEnergy => _maxEnergy;
        
        public int StartingEnergy => _startingEnergy;
    }
}