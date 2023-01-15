using System;
using System.Collections.Generic;
using Cards;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Player
{
    public class PlayerHandRepresentation: MonoBehaviour
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
        }

        private void OnDestroy()
        {
            PlayerHand.OnHandChanged -= UpdateHand;
            
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered -= SubscribeToEvents;
        }
        
        private void UpdateHand()
        {
            InstantiateCards();          
        }

        private void InstantiateCards()
        {
            foreach (var cardWorld in _instantiatedCards)
            {
                Destroy(cardWorld.gameObject);
            }
            
            _instantiatedCards.Clear();
            
            var cards = PlayerHand.Cards;
            var cardsCount = cards.Count;

            for (var i = 0; i < cardsCount; i++)
            {
                var position = GetCardPosition(cardsCount, i);
                var rotation = GetCardRotationBasedOnPosition(position);
                
                var cardWorld = Instantiate(_cardPrefab, position, rotation, transform);
                cardWorld.SetCard(cards[i]);
                
                _instantiatedCards.Add(cardWorld);
            }
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
            
            return position;
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
            
            return Quaternion.Euler(0, 0, -rotation);
        }
        
        private void SubscribeToEvents(Type type)
        {
            if (type != typeof(IPlayerHand)) 
                return;
            
            PlayerHand.OnHandChanged += UpdateHand;
        }
    }
}