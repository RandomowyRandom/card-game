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
    public class RemotePlayerHandRepresentation: SerializedMonoBehaviour
    {
        [SerializeField]
        private NetworkIdentity _parentNetworkIdentity;

        [OdinSerialize] [ReadOnly]
        private int _cardCount;
        
        [Space(5)]
        
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
        private GameObject _cardPrefab;

        [SerializeField]
        private AnimationCurve _cardYPositionCurve;
        
        private IPlayerHand _playerHand;
        
        private List<GameObject> _instantiatedCards = new();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_leftCorner, .3f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_rightCorner, .3f);
        }

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

        private void OnCardKeysChanged(int count)
        {
            _cardCount = count;
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
        
        private Quaternion GetCardRotationBasedOnPosition(Vector3 position)
        {
            var minX = _leftCorner.x;
            var maxX = _rightCorner.x;

            var rotation = Mathf.Lerp(_minRotation, _maxRotation, (position.x - minX) / (maxX - minX));

            if(float.IsNaN(rotation))
                rotation = 0;
            
            return Quaternion.Euler(0, 180, rotation);
        }
        
        private void DespawnCard(int count)
        {
            var cardWorld = _instantiatedCards.Last();
            _instantiatedCards.Remove(cardWorld);
            Destroy(cardWorld.gameObject);
            
            UpdateCardPositionAndRotation(_cardCount);
        }

        private void SpawnCard(int count)
        {
            var cardAmount = _cardCount;
            var spawnPosition = GetMiddleOfHand(cardAmount);
            spawnPosition.z -= 1;
            
            var cardInstance = Instantiate(_cardPrefab, spawnPosition, Quaternion.identity, transform);
            _instantiatedCards.Add(cardInstance);

            UpdateCardPositionAndRotation(cardAmount);
        }
        
        private void ClearCards(int count)
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