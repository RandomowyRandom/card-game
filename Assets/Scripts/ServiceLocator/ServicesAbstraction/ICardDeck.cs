using Cards;
using Scriptables.Cards.Abstractions;

namespace ServiceLocator.ServicesAbstraction
{
    public interface ICardDeck : IService
    {
        public Card DrawCard();

        public Card DrawCard(CardRarity rarity);

        public Card DrawCard(CardRarity[] rarities);
        public Card DrawCard(string key);

        public void ReturnCard(Card card);
    }
}