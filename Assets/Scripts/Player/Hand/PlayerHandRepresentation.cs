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
    public class PlayerHandRepresentation: SerializedMonoBehaviour
    {
        [SerializeField]
        private Vector3 _leftCorner;
        
        [SerializeField]
        private Vector3 _rightCorner;
        
        [SerializeField]
        private float _minRotation;
        
        [SerializeField]
        private float _maxRotation;
        
        [SerializeField]
        private Vector3 _scaleFactor;

        [Space]
        [SerializeField]
        private CardWorld _cardPrefab;

        [SerializeField]
        private AnimationCurve _cardYPositionCurve;
        
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

        private Vector3 GetCardPosition(int cardAmount, int cardIndex)
        {
            var minX = GetMinX(cardAmount);
            var maxX = GetMaxX(cardAmount);
            
            var minY = _leftCorner.y;
            var maxY = GetMaxY(cardAmount);
            
            var minZ = GetMinZ(cardAmount);
            var maxZ = _rightCorner.z;
            
            var x = Mathf.Lerp(minX, maxX, (float) cardIndex / (cardAmount - 1));
            var z = Mathf.Lerp(minZ, maxZ, (float) cardIndex / (cardAmount - 1));
            
            var y = _cardYPositionCurve.Evaluate((float) cardIndex / (cardAmount - 1));
            y = Mathf.Lerp(minY, maxY, y);

            var position = new Vector3(x, y, z);
            
            return position.IsAnyNaN() ? GetMiddleOfHand(cardAmount) : position;
        }

        private Vector3 GetMiddleOfHand(int cardAmount)
        {
            var minX = GetMinX(cardAmount);
            var maxX = GetMaxX(cardAmount);
            
            var minZ = GetMinZ(cardAmount);
            var maxZ = _rightCorner.z;
            
            var x = Mathf.Lerp(minX, maxX, 0.5f);
            var z = Mathf.Lerp(minZ, maxZ, 0.5f);

            return new Vector3(x, _leftCorner.y, z);
        }
        
        private float GetMinX(int cardCount)
        {
            var minX = -(cardCount * _scaleFactor.x);
            minX = Mathf.Clamp(minX, _leftCorner.x, _rightCorner.x);

            return minX;
        }
        
        private float GetMaxX(int cardCount)
        {
            var maxX = cardCount * _scaleFactor.x;
            maxX = Mathf.Clamp(maxX, _leftCorner.x, _rightCorner.x);

            return maxX;
        }

        private float GetMinZ(int cardCount)
        {
            return _leftCorner.z + cardCount * _scaleFactor.z;
        }
        
        private float GetMaxY(int cardCount)
        {
            return _rightCorner.y + cardCount * _scaleFactor.y;
        }

        private Quaternion GetCardRotationBasedOnPosition(Vector3 position)
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