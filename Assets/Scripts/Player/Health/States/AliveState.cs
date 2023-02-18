using Scriptables.Player;
using ServiceLocator.ServicesAbstraction;
using StateMachine;

namespace Player.States
{
    public class AliveState: IState
    {
        private readonly IPlayerHand _playerHand;
        private readonly PlayerCardDrawConfiguration _cardDrawConfiguration;

        private ICardDeck _cardDeck;
        private ICardDeck CardDeck => _cardDeck ??= ServiceLocator.ServiceLocator.Instance.Get<ICardDeck>();
        
        public AliveState(IPlayerHand playerHand, PlayerCardDrawConfiguration cardDrawConfiguration)
        {
            _playerHand = playerHand;
            _cardDrawConfiguration = cardDrawConfiguration;
        }
        
        public void Enter()
        {
            _playerHand.SetCardDrawConfiguration(_cardDrawConfiguration);
            CardDeck.SetCardDatabase(_cardDrawConfiguration.CardDatabase);
        }

        public void Exit()
        {
            
        }

        public void Tick()
        {
            
        }
    }
}