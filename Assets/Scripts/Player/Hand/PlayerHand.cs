using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Helpers.Extensions;
using JetBrains.Annotations;
using Mirror;
using QFSW.QC;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Player.Hand
{
    public class PlayerHand : NetworkBehaviour, IPlayerHand
    {
        private List<Card> _cards = new();
        
        private readonly SyncList<string> _cardsKeys = new();
        public event Action<Card> OnCardAdded;
        public event Action<Card, int> OnCardRemoved;
        public event Action OnHandCleared;
        public event Action OnCardKeysAdded;
        public event Action OnCardKeysRemoved;
        public event Action OnCardKeysCleared;
        public List<Card> Cards => _cards;
        public int CardKeysCount => _cardsKeys.Count;

        private void Start()
        {
            _cardsKeys.Callback += CardKeysChanged;
        }

        private void OnDestroy()
        {
            _cardsKeys.Callback -= CardKeysChanged;

            if (!isOwned)
                return;
            
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerHand>();
            QuantumRegistry.DeregisterObject(this);
        }
        
        private void CardKeysChanged(SyncList<string>.Operation op, int itemIndex, string oldItem, string newItem)
        {
            switch (op)
            {
                case SyncList<string>.Operation.OP_ADD:
                    OnCardKeysAdded?.Invoke();
                    break;
                case SyncList<string>.Operation.OP_REMOVEAT:
                    OnCardKeysRemoved?.Invoke();
                    break;
                case SyncList<string>.Operation.OP_CLEAR:
                    OnCardKeysCleared?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }
        
        public override void OnStartAuthority()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerHand>(this);
            QuantumRegistry.RegisterObject(this);
        }

        public void AddCard(Card card)
        {
            CmdAddCard(card.name);
            _cards.Add(card);
            
            if(!isOwned)
                return;
            
            OnCardAdded?.Invoke(card);
        }
        
        public void RemoveCard(Card card)
        {
            var cardIndex = _cards.IndexOf(card);
            
            CmdRemoveCard(card.name);
            _cards.Remove(card);
            
            if(!isOwned)
                return;
            
            OnCardRemoved?.Invoke(card, cardIndex);
        }
        
        public void UpgradeCard(Card card)
        {
            var oldCardIndex = _cards.IndexOf(card);
            var newCard = card.UpgradeCard;

            _cards.Remove(card);
            CmdRemoveCard(card.name);
            _cards.Add(newCard);
            CmdAddCard(newCard.name);
            
            if(!isOwned)
                return;
            
            OnCardRemoved?.Invoke(card, oldCardIndex);
            OnCardAdded?.Invoke(newCard);
        }

        public bool UpgradeDeck()
        {
            var upgradeableCards = _cards.Where(card => card.UpgradeCard != null).ToList();
            
            if(upgradeableCards.Count == 0)
                return false;
            
            var nonUpgradeableCards = _cards.Where(card => card.UpgradeCard == null).ToList();
            
            var upgradedCards = upgradeableCards.Select(card => card.UpgradeCard).ToList();
            var newCards = upgradedCards.Concat(nonUpgradeableCards).ToList();
            
            ClearHand();
            foreach (var newCard in newCards)
            {
                AddCard(newCard);
            }
            
            return true;
        }

        public bool UpgradeRandomCard()
        {
            var upgradeableCards = _cards.Where(card => card.UpgradeCard != null).ToList();
            
            if (upgradeableCards.Count == 0)
                return false;
            
            UpgradeCard(upgradeableCards.GetRandomElement());
            return true;
        }

        public void ClearHand()
        {
            _cards.Clear();
            CmdClearHand();
            
            if(!isOwned)
                return;

            OnHandCleared?.Invoke();
        }
        
        #region Networking
        
        [Mirror.Command]
        private void CmdAddCard(string cardKey)
        {
            _cardsKeys.Add(cardKey);
        }

        [Mirror.Command]
        private void CmdRemoveCard(string cardKey)
        {
            _cardsKeys.Remove(cardKey);
        }
        
        [Mirror.Command]
        private void CmdClearHand()
        {
            _cardsKeys.Clear();
        }

        #endregion

        #region QC

        [QFSW.QC.Command("add-card", MonoTargetType.Registry)] [UsedImplicitly]
        private void CommandAddCardByKeys(string cardKey)
        {
            var cardDeck = ServiceLocator.ServiceLocator.Instance.Get<ICardDeck>();
            var playerHand = FindObjectsOfType<PlayerHand>().Where(h => h.isOwned).ToList();

            var card = cardDeck.DrawCard(cardKey);
            playerHand[0].AddCard(card);
            
            Debug.Log($"Added {card.name} to the player's hand");
        }

        [QFSW.QC.Command("add-random-card", MonoTargetType.Registry)] [UsedImplicitly]
        private void CommandAddRandomCard(int amount = 1)
        {
            var cardDeck = ServiceLocator.ServiceLocator.Instance.Get<ICardDeck>();
            var playerHand = FindObjectsOfType<PlayerHand>().Where(h => h.isOwned).ToList();

            for (var i = 0; i < amount; i++)
            {
                var card = cardDeck.DrawCard();
                playerHand[0].AddCard(card);
            
                Debug.Log($"Added {card.name} to the player's hand");
                
            }
        }

        [QFSW.QC.Command("add-random-card-rarity", MonoTargetType.Registry)] [UsedImplicitly]
        private void CommandAddRandomCardRarity(CardRarity rarity)
        {
            var cardDeck = ServiceLocator.ServiceLocator.Instance.Get<ICardDeck>();

            var playerHand = FindObjectsOfType<PlayerHand>().Where(h => h.isOwned).ToList();
            
            var card = cardDeck.DrawCard(rarity);
            playerHand[0].AddCard(card);
            
            Debug.Log($"Added {card.name} to the player's hand");
        }

        [QFSW.QC.Command("remove-random-card", MonoTargetType.Registry)] [UsedImplicitly]
        private void RemoveRandomCard()
        {
            var randomCard = _cards.GetRandomElement();
            RemoveCard(randomCard);
        }
        
        [QFSW.QC.Command("remove-card", MonoTargetType.Registry)] [UsedImplicitly]
        private void RemoveCard(int index)
        {
            var card = _cards[index];
            RemoveCard(card);
        }

        [QFSW.QC.Command("log-all-cards", MonoTargetType.Registry)] [UsedImplicitly]
        private void CommandLogAllCards()
        {
            Debug.Log("Cards in hand:");
            foreach (var card in _cardsKeys)
            {
                Debug.Log(card);
            }
        }

        [QFSW.QC.Command("clear-hand", MonoTargetType.Registry)] [UsedImplicitly]
        private void CommandClearHand()
        {
            var playerHand = FindObjectsOfType<PlayerHand>().Where(h => h.isOwned).ToList();
            playerHand[0].ClearHand();
        }
        
        #endregion
    }
}