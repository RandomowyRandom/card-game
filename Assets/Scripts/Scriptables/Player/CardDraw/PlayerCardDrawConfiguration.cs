using Common.Attributes;
using Deck;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Scriptables.Player
{
    [ScriptableFactoryElement]
    public class PlayerCardDrawConfiguration: SerializedScriptableObject
    {
        [OdinSerialize]
        private IPlayerCardAmountProvider _cardAmountProvider;
        
        [SerializeField]
        private CardDatabase _cardDatabase;
        
        public CardDatabase CardDatabase => _cardDatabase;
        
        public int GetCardAmount()
        {
            return _cardAmountProvider.GetCardAmount();
        }
    }
}