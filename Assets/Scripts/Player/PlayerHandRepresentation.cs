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
        
        [Space]
        [SerializeField]
        private CardWorld _cardPrefab;

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
            var minX = _leftCorner.x;
            var maxX = _rightCorner.x;
            
            var minY = _leftCorner.y;
            var maxY = _rightCorner.y;
            
            var minZ = _leftCorner.z;
            var maxZ = _rightCorner.z;
            
            var x = Mathf.Lerp(minX, maxX, (float) cardIndex / cardAmount);
            var y = Mathf.Lerp(minY, maxY, (float) cardIndex / cardAmount);
            var z = Mathf.Lerp(minZ, maxZ, (float) cardIndex / cardAmount);

            var position = new Vector3(x, y, z);
            
            return position;
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