using System;
using System.Collections.Generic;
using Cards;
using Common.Attributes;
using Cysharp.Threading.Tasks;
using ServiceLocator.ServicesAbstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Scriptables.Cards.Abstractions
{
    [ScriptableFactoryElement]
    public class Card : SerializedScriptableObject
    {
        public static event Action<Card> OnCardUsedLocally;

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
        
        [InfoBox("TestCard button for debug purposes only! (might not work as expected)", InfoMessageType.Warning)]
        [Button]
        private void TestCard()
        {
            if (!Application.isEditor)
            {
                Debug.LogWarning("TestCard button needs the game to be running!");
                return;
            }
            
            Use();
        }

        public string CardName => _cardName;

        public string Description => _description;

        public Sprite CardSprite => _cardSprite;

        public Card UpgradeCard => _upgradeCard;

        public int EnergyCost => _energyCost;

        public bool IsSpecial => _isSpecial;
        
        public CardRarity Rarity => _rarity;

        public virtual async UniTask Use()
        {
            foreach (var effect in _cardEffects)
            {
                if(effect.IsAsync)
                    await effect.OnUse();
                else
                    effect.OnUse();
            }
            
            OnCardUsedLocally?.Invoke(this);
        }
    }
}