using System.Collections.Generic;
using Cards;
using Common.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Scriptables.Cards.Abstractions
{
    [ScriptableFactoryElement]
    public class Card : SerializedScriptableObject
    {
        [SerializeField]
        private string _cardName;
        
        [SerializeField]
        [TextArea]
        private string _description;
        
        [SerializeField]
        private CardRarity _rarity;
        
        [SerializeField]
        private Sprite _cardSprite;
        
        [SerializeField]
        private Card _upgradeCard;
        
        // -1 if the card is player energy dependent (X energy is being used)
        // X = all the player's energy
        [InfoBox("If the card is player energy dependent, set this to -1")]
        [SerializeField]
        [Range(-1, 8)]
        private int _energyCost;
        
        [SerializeField]
        private bool _isSpecial;

        [Space(5)] [OdinSerialize]
        private List<ICardEffect> _cardEffects;
        
        [InfoBox("TestCard button for debug purposes only!", InfoMessageType.Warning)]
        [Button]
        private void TestCard()
        {
            Use();
        }

        
        public string CardName => _cardName;

        public string Description => _description;

        public Sprite CardSprite => _cardSprite;

        public Card UpgradeCard => _upgradeCard;

        public int EnergyCost => _energyCost;

        public bool IsSpecial => _isSpecial;
        
        public CardRarity Rarity => _rarity;

        public virtual void Use()
        {
            foreach (var effect in _cardEffects)
            {
                effect.OnUse();
            }
        }
    }
}