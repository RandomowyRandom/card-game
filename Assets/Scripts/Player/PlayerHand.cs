using System.Collections.Generic;
using Cards;
using JetBrains.Annotations;
using Mirror;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Player
{
    public class PlayerHand : NetworkBehaviour
    {
        private List<Card> _cards = new();
        
        private readonly SyncList<string> _cardsKeys = new();

        public void AddCard(Card card)
        {
            CmdAddCard(card.name);
            _cards.Add(card);
        }
        
        public void RemoveCard(Card card)
        {
            CmdRemoveCard(card.name);
            _cards.Remove(card);
        }
        
        #region Networking
        
        [Command]
        private void CmdAddCard(string cardKey)
        {
            _cardsKeys.Add(cardKey);
        }

        [Command]
        private void CmdRemoveCard(string cardKey)
        {
            _cardsKeys.Remove(cardKey);
        }

        #endregion

        #region QC

        [QFSW.QC.Command("add-random-card")] [UsedImplicitly]
        private void AddRandomCard()
        {
            var cardDeck = ServiceLocator.ServiceLocator.Instance.Get<ICardDeck>();

            var card = cardDeck.DrawCard();
            AddCard(card);
            
            Debug.Log($"Added {card.name} to the player's hand");
        }

        [QFSW.QC.Command("add-random-card-rarity")]
        [UsedImplicitly]
        private void AddRandomCardRarity(CardRarity rarity)
        {
            var cardDeck = ServiceLocator.ServiceLocator.Instance.Get<ICardDeck>();

            var card = cardDeck.DrawCard(rarity);
            AddCard(card);
            
            Debug.Log($"Added {card.name} to the player's hand");
        }

        
        [QFSW.QC.Command("log-all-cards")] [UsedImplicitly]
        private void LogAllCards()
        {
            Debug.Log("Cards in hand:");
            foreach (var card in _cardsKeys)
            {
                Debug.Log(card);
            }
        }

        
        
        #endregion
    }
}