using Editor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scriptables.Player
{
    [ScriptableFactoryElement]
    public class PlayerWalletStats : SerializedScriptableObject
    {
        [SerializeField]
        private int _startingMoney;
        
        public int StartingMoney => _startingMoney;
    }
}