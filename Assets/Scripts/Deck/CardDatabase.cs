using System.Collections.Generic;
using Common.Attributes;
using Helpers.Extensions;
using Scriptables.Cards.Abstractions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Deck
{
    [ScriptableFactoryElement]
    public class CardDatabase : SerializedScriptableObject
    {
        [SerializeField] 
        private List<Card> _cards;

        public List<Card> GetCards()
        {
            var cards = new List<Card>();
            cards.AddRange(_cards);
            
            return cards;
        }

        public Card GetRandomCard()
        {
            return _cards.GetRandomElement();
        }
    }
}