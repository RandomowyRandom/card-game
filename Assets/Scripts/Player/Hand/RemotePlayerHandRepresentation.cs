using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Helpers.Extensions;
using Mirror;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Player.Hand
{
    public class RemotePlayerHandRepresentation: PlayerHandRepresentation
    {
        [SerializeField]
        private NetworkIdentity _parentNetworkIdentity;

        [OdinSerialize] [ReadOnly]
        private int _cardCount;

        [Space]
        [SerializeField]
        private GameObject _cardPrefab;

        private IPlayerHand _playerHand;
        
        private List<GameObject> _instantiatedCards = new();

        private void Awake()
        {
            if (_parentNetworkIdentity.isOwned)
            {
                Destroy(this);
                return;
            }
            
            _playerHand = _parentNetworkIdentity.GetComponent<IPlayerHand>();
            
            _playerHand.OnCardKeysAdded += OnCardKeysChanged;
            _playerHand.OnCardKeysRemoved += OnCardKeysChanged;
            _playerHand.OnCardKeysCleared += OnCardKeysChanged;

            _playerHand.OnCardKeysAdded += SpawnCard;
            _playerHand.OnCardKeysRemoved += DespawnCard;
            _playerHand.OnCardKeysCleared += ClearCards;
        }

        private void OnDestroy()
        {
            _playerHand.OnCardKeysAdded -= OnCardKeysChanged;
            _playerHand.OnCardKeysRemoved -= OnCardKeysChanged;
            _playerHand.OnCardKeysCleared -= OnCardKeysChanged;

            _playerHand.OnCardKeysAdded -= SpawnCard;
            _playerHand.OnCardKeysRemoved -= DespawnCard;
            _playerHand.OnCardKeysCleared -= ClearCards;
        }

        private void OnCardKeysChanged()
        {
            _cardCount = _playerHand.CardKeysCount;
        }

        protected override Quaternion GetCardRotationBasedOnPosition(Vector3 position)
        {
            var minX = _leftCorner.x;
            var maxX = _rightCorner.x;

            var rotation = Mathf.Lerp(_minRotation, _maxRotation, (position.x - minX) / (maxX - minX));

            if(float.IsNaN(rotation))
                rotation = 0;
            
            return Quaternion.Euler(0, 180, rotation);
        }
        
        private void DespawnCard()
        {
            var cardWorld = _instantiatedCards.Last();
            _instantiatedCards.Remove(cardWorld);
            Destroy(cardWorld.gameObject);
            
            UpdateCardPositionAndRotation(_cardCount);
        }

        private void SpawnCard()
        {
            var cardAmount = _cardCount;
            var spawnPosition = GetMiddleOfHand(cardAmount);
            spawnPosition.z -= 1;
            
            var cardInstance = Instantiate(_cardPrefab, spawnPosition, Quaternion.identity, transform);
            _instantiatedCards.Add(cardInstance);

            UpdateCardPositionAndRotation(cardAmount);
        }
        
        private void ClearCards()
        {
            foreach (var card in _instantiatedCards)
            {
                Destroy(card.gameObject);
            }
            
            _instantiatedCards.Clear();
        }
        
        private void UpdateCardPositionAndRotation(int cardAmount)
        {
            foreach (var cardWorld in _instantiatedCards)
            {
                var position = GetCardPosition(cardAmount, _instantiatedCards.IndexOf(cardWorld));
                var rotation = GetCardRotationBasedOnPosition(position);

                cardWorld.transform.DOLocalMove(position, 0.3f);
                cardWorld.transform.DOLocalRotate(rotation.eulerAngles, 0.3f);
            }
        }
    }
}