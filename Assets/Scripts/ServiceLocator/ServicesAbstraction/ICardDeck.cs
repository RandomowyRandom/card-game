using Cards;
using Scriptables.Cards.Abstractions;

namespace ServiceLocator.ServicesAbstraction
{
    public interface ICardDeck : IService
    {
        public Card DrawCard();

        public Card DrawCard(CardRarity rarity);

        public Card DrawCard(CardRarity[] rarity);

        public void ReturnCard(Card card);
    }
}