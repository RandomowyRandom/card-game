namespace Scriptables.Cards.Abstractions
{
    public interface ICardEffect
    {
        public ITargetProvider TargetProvider { get; }
        public void OnUse(); // TODO: add reference for UserCardHandler or something
    }
}