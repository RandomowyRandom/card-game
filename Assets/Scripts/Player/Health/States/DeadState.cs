
using Scriptables.Player;
using ServiceLocator.ServicesAbstraction;
using StateMachine;
using UnityEngine;

namespace Player.States
{
    public class DeadState: IState 
    {
        private readonly IPlayerHand _playerHand;
        private readonly PlayerCardDrawConfiguration _cardDrawConfiguration;

        private ICardDeck _cardDeck;
        private ICardDeck CardDeck => _cardDeck ??= ServiceLocator.ServiceLocator.Instance.Get<ICardDeck>();
        
        public DeadState(IPlayerHand playerHand, PlayerCardDrawConfiguration cardDrawConfiguration)
        {
            _playerHand = playerHand;
            _cardDrawConfiguration = cardDrawConfiguration;
        }
        
        public void Enter()
        {
            _playerHand.SetCardDrawConfiguration(_cardDrawConfiguration);
            CardDeck.SetCardDatabase(_cardDrawConfiguration.CardDatabase);
            Debug.Log("Player is dead");
        }

        public void Exit()
        {
            
        }

        public void Tick()
        {
            
        }
    }
}