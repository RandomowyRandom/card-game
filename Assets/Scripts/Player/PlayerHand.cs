using System.Collections.Generic;
using Mirror;
using Scriptables.Cards.Abstractions;

namespace Player
{
    public class PlayerHand : NetworkBehaviour
    {
        private List<Card> _cards = new();
        
        private readonly SyncList<string> _cardsKeys = new();

        public void AddCard(Card card)
        {
            CmdAddCard(card.name);
            _cards.Add(card);
        }
        
        public void RemoveCard(Card card)
        {
            CmdRemoveCard(card.name);
            _cards.Remove(card);
        }
        
        #region Networking
        
        [Command]
        private void CmdAddCard(string cardKey)
        {
            _cardsKeys.Add(cardKey);
        }

        [Command]
        private void CmdRemoveCard(string cardKey)
        {
            _cardsKeys.Remove(cardKey);
        }

        #endregion
    }
}