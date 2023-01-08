using Sirenix.OdinInspector;
using UnityEngine;

namespace Scriptables.Player
{
    public class PlayerWalletStats : SerializedScriptableObject
    {
        [SerializeField]
        private int _startingMoney;
        
        public int StartingMoney => _startingMoney;
    }
}