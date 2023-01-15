using System;
using System.Collections.Generic;
using Cards;
using JetBrains.Annotations;
using Mirror;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Player
{
    public class PlayerHand : NetworkBehaviour, IPlayerHand
    {
        private List<Card> _cards = new();
        
        private readonly SyncList<string> _cardsKeys = new();
        public List<Card> Cards => _cards;

        public event Action OnHandChanged;
        
        private void OnDestroy()
        {
            if (!isOwned)
                return;
            
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerHand>();
        }
        
        public override void OnStartAuthority()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerHand>(this);
        }

        public void AddCard(Card card)
        {
            CmdAddCard(card.name);
            _cards.Add(card);
            
            if(!isOwned)
                return;
            
            OnHandChanged?.Invoke();
        }
        
        public void RemoveCard(Card card)
        {
            CmdRemoveCard(card.name);
            _cards.Remove(card);
            
            if(!isOwned)
                return;
            
            OnHandChanged?.Invoke();
        }

        public void ClearHand()
        {
            _cards.Clear();
            CmdClearHand();
            
            OnHandChanged?.Invoke();
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
        
        [Command]
        private void CmdClearHand()
        {
            _cardsKeys.Clear();
        }

        #endregion

        #region QC

        [QFSW.QC.Command("add-random-card")] [UsedImplicitly]
        private void CommandAddRandomCard(int amount = 1)
        {
            var cardDeck = ServiceLocator.ServiceLocator.Instance.Get<ICardDeck>();

            for (var i = 0; i < amount; i++)
            {
                var card = cardDeck.DrawCard();
                AddCard(card);
            
                Debug.Log($"Added {card.name} to the player's hand");
                
            }
        }

        [QFSW.QC.Command("add-random-card-rarity")]
        [UsedImplicitly]
        private void CommandAddRandomCardRarity(CardRarity rarity)
        {
            var cardDeck = ServiceLocator.ServiceLocator.Instance.Get<ICardDeck>();

            var card = cardDeck.DrawCard(rarity);
            AddCard(card);
            
            Debug.Log($"Added {card.name} to the player's hand");
        }

        
        [QFSW.QC.Command("log-all-cards")] [UsedImplicitly]
        private void CommandLogAllCards()
        {
            Debug.Log("Cards in hand:");
            foreach (var card in _cardsKeys)
            {
                Debug.Log(card);
            }
        }

        [QFSW.QC.Command("clear-hand")] [UsedImplicitly]
        private void CommandClearHand()
        {
            ClearHand();
        }
        
        #endregion
    }
}