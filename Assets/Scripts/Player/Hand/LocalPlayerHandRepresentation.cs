using System;
using System.Collections.Generic;
using Cards;
using DG.Tweening;
using Helpers.Extensions;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Player.Hand
{
    public class LocalPlayerHandRepresentation: PlayerHandRepresentation
    {
        [Space]
        [SerializeField]
        private CardWorld _cardPrefab;

        [SerializeField]
        private Vector3 _selectedMove;
        
        [Space]
        
        [OdinSerialize]
        private ICardSelectionHandler _cardSelectionHandler;

        private IPlayerHand _playerHand;
        
        private List<CardWorld> _instantiatedCards = new();

        private IPlayerHand PlayerHand
        {
            get
            {
                _playerHand ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerHand>();
                return _playerHand;
            }
        }

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered += SubscribeToEvents;
            
            _cardSelectionHandler.OnSelected += SelectCard;
            _cardSelectionHandler.OnDeselected += DeselectCard;
        }

        private void OnDestroy()
        {
            PlayerHand.OnCardAdded -= SpawnCard;
            PlayerHand.OnCardRemoved -= DespawnCard;
            PlayerHand.OnHandCleared += ClearCards;
            
            _cardSelectionHandler.OnSelected -= SelectCard;
            _cardSelectionHandler.OnDeselected -= DeselectCard;

            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered -= SubscribeToEvents;
        }
        
        private void ClearCards()
        {
            foreach (var card in _instantiatedCards)
            {
                Destroy(card.gameObject);
            }
            
            _instantiatedCards.Clear();
        }

        protected override Quaternion GetCardRotationBasedOnPosition(Vector3 position)
        {
            var minX = _leftCorner.x;
            var maxX = _rightCorner.x;

            var rotation = Mathf.Lerp(_minRotation, _maxRotation, (position.x - minX) / (maxX - minX));

            if(float.IsNaN(rotation))
                rotation = 0;
            
            return Quaternion.Euler(0, 0, -rotation);
        }
        
        private void SubscribeToEvents(Type type)
        {
            if (type != typeof(IPlayerHand)) 
                return;
            
            PlayerHand.OnCardAdded += SpawnCard;
            PlayerHand.OnHandCleared += ClearCards;
            PlayerHand.OnCardRemoved += DespawnCard;
        }

        private void DespawnCard(Card card, int cardIndex)
        {
            var cardWorld = _instantiatedCards[cardIndex];
            _instantiatedCards.Remove(cardWorld);
            Destroy(cardWorld.gameObject);
            
            UpdateCardPositionAndRotation(PlayerHand.Cards.Count);
        }

        private void SpawnCard(Card card)
        {
            var cardAmount = PlayerHand.Cards.Count;
            var spawnPosition = GetMiddleOfHand(cardAmount);
            spawnPosition.z -= 1;
            
            var cardInstance = Instantiate(_cardPrefab, spawnPosition, Quaternion.identity, transform);
            _instantiatedCards.Add(cardInstance);

            UpdateCardPositionAndRotation(cardAmount);
            
            cardInstance.SetCard(card);
        }

        private void DeselectCard(CardWorld cardWorld)
        {
            cardWorld.CardMesh.DOLocalMove(Vector3.zero, 0.2f);
        }

        private void SelectCard(CardWorld cardWorld)
        {
            cardWorld.CardMesh.DOLocalMove(cardWorld.CardMesh.localPosition + _selectedMove, 0.15f)
                .SetEase(Ease.OutBack);
        }
        
        private void UpdateCardPositionAndRotation(int cardAmount, CardWorld exception = null)
        {
            foreach (var cardWorld in _instantiatedCards)
            {
                if(exception != null && cardWorld == exception)
                    continue;
                
                var position = GetCardPosition(cardAmount, _instantiatedCards.IndexOf(cardWorld));
                var rotation = GetCardRotationBasedOnPosition(position);

                cardWorld.transform.DOLocalMove(position, 0.3f);
                cardWorld.transform.DOLocalRotate(rotation.eulerAngles, 0.3f);
            }
        }
    }
}