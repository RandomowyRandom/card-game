using Cards;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scriptables.Cards.Abstractions
{
    public abstract class Card : SerializedScriptableObject
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
        private int _energyCost;
        
        [SerializeField]
        private bool _isSpecial;

        public string CardName => _cardName;

        public string Description => _description;

        public Sprite CardSprite => _cardSprite;

        public Card UpgradeCard => _upgradeCard;

        public int EnergyCost => _energyCost;

        public bool IsSpecial => _isSpecial;

        public abstract void Use();
    }
}