using Helpers.Extensions;
using Scriptables.Cards.Abstractions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.Hand
{
    public abstract class PlayerHandRepresentation: SerializedMonoBehaviour
    {
        [SerializeField]
        protected Vector3 _leftCorner;
        
        [SerializeField]
        protected Vector3 _rightCorner;
        
        [SerializeField]
        protected float _minRotation;
        
        [SerializeField]
        protected float _maxRotation;
        
        [SerializeField]
        protected Vector3 _scaleFactor;
        
        [SerializeField]
        protected AnimationCurve _cardYPositionCurve;

        protected abstract Quaternion GetCardRotationBasedOnPosition(Vector3 position);
        
        protected Vector3 GetCardPosition(int cardAmount, int cardIndex)
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
        
        protected Vector3 GetMiddleOfHand(int cardAmount)
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
    }
}