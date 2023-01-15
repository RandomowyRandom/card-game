using Deck;
using JetBrains.Annotations;
using Scriptables.Cards.Abstractions;
using TMPro;
using UnityEngine;

namespace Cards
{
    public class CardWorld: MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _cardName;
        
        [SerializeField]
        private TMP_Text _cardDescription;
        
        [SerializeField]
        private TMP_Text _cardCost;
        
        [SerializeField]
        private SpriteRenderer _cardIcon;
        
        [SerializeField]
        private Transform _cardMesh;
        
        [Space]
        
        [SerializeField]
        private CardDatabase _cardDatabase;
        
        public Transform CardMesh => _cardMesh;
        
        public void SetCard(Card card)
        {
            _cardName.text = card.CardName;
            _cardDescription.text = card.Description;
            _cardCost.text = card.EnergyCost.ToString();
            _cardIcon.sprite = card.CardSprite;
        }

        #region QC

        [QFSW.QC.Command("dev-set-card")] [UsedImplicitly]
        private void SetCardCommand(string cardKey)
        {
            var card = _cardDatabase.GetCardByKey(cardKey);

            if (card == null)
            {
                Debug.LogError($"Card {cardKey} does not exist!");
                return;
            }
            
            SetCard(card);
        }

        #endregion
    }
}