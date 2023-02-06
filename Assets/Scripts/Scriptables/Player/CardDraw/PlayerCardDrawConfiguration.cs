using Common.Attributes;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Scriptables.Player
{
    [ScriptableFactoryElement]
    public class PlayerCardDrawConfiguration: SerializedScriptableObject
    {
        [OdinSerialize]
        private IPlayerCardAmountProvider _cardAmountProvider;
        
        public int GetCardAmount()
        {
            return _cardAmountProvider.GetCardAmount();
        }
    }
}