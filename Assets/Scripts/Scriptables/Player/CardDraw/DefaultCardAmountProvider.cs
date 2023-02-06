using System;
using Player.Interfaces;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Scriptables.Player
{
    [Serializable]
    public class DefaultCardAmountProvider: IPlayerCardAmountProvider
    {
        [SerializeField]
        private int _maxCardsToDrawAtOnce = 4;
        
        [SerializeField]
        private int _minCardsToDrawAtOnce = 0;
        
        [SerializeField]
        private int _maxCardsInHand = 6;

        private IPlayerHand _playerHand;
        
        private IPlayerHand PlayerHand => _playerHand ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerHand>();

        public int GetCardAmount()
        {
            var amountToFullHand = _maxCardsInHand - PlayerHand.CardKeysCount;
            
            if(amountToFullHand <= 0)
                return 0;
            
            var amountToDraw = Mathf.Clamp(amountToFullHand, _minCardsToDrawAtOnce, _maxCardsToDrawAtOnce);
            
            return amountToDraw;
        }
    }
}