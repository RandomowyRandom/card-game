using System.Collections.Generic;
using System.Linq;
using Cards;
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

        [Button("Get All Cards")]
        private void GetAllCards()
        {
            _cards = Resources.LoadAll<Card>("NetworkedScriptables/Cards/").ToList();
        }
        
        public List<Card> GetCards()
        {
            var cards = new List<Card>();
            cards.AddRange(_cards);
            
            return cards;
        }

        public Card GetCardByKey(string key)
        {
            return _cards.FirstOrDefault(card => card.name == key);
        }
        
        public Card GetRandomCard()
        {
            return _cards.GetRandomElement();
        }
        public Card GetRandomCard(CardRarity rarity)
        {
            var cards = _cards.FindAll(card => card.Rarity == rarity);
            return cards.GetRandomElement();
        }

        public Card GetRandomCard(IEnumerable<CardRarity> rarities)
        {
            var raritiesList = rarities.ToList();
            
            var cards = _cards.FindAll(card => raritiesList.Contains(card.Rarity));
            return cards.GetRandomElement();
        }
    }
}