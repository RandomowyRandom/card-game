using System;
using Cysharp.Threading.Tasks;
using Mirror;
using Player.Hand;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Player
{
    public class PlayerCardUser: NetworkBehaviour, IPlayerCardUser
    {
        private ICardSelectionHandler _cardSelectionHandler;
        private IPlayerEnergy _playerEnergy;
        private IPlayerHand _playerHand;

        private ICardSelectionHandler CardSelectionHandler
        {
            get
            {
                _cardSelectionHandler ??= ServiceLocator.ServiceLocator.Instance.Get<ICardSelectionHandler>();
                return _cardSelectionHandler;
            }
        }
        private IPlayerEnergy PlayerEnergy
        {
            get
            {
                _playerEnergy ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerEnergy>();
                return _playerEnergy;
            }
        }
        private IPlayerHand PlayerHand
        {
            get
            {
                _playerHand ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerHand>();
                return _playerHand;
            }
        }

        private bool _useLock;

        public override void OnStartAuthority()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerCardUser>(this);
        }

        private async void Update()
        {
            if(!isOwned)
                return;
            
            if (!Input.GetMouseButtonDown(0))
                return;
            
            await TryUseCard();
        }

        private async UniTask<bool> TryUseCard()
        {
            if (_useLock)
                return false;

            if (CardSelectionHandler.SelectedCard == null)
                return false;
            
            var selectedCard = CardSelectionHandler.SelectedCard.Card;
            
            if(PlayerEnergy.CurrentEnergy < selectedCard.EnergyCost)
                return false;
            
            _useLock = true;
            await selectedCard.Use();
            
            PlayerEnergy.RemoveEnergy(selectedCard.EnergyCost);

            PlayerHand.RemoveCard(selectedCard);
            _useLock = false;
            
            return true;
        }
        
        private void OnDestroy()
        {
            if(!isOwned)
                return;
            
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerCardUser>();
        }
    }
}