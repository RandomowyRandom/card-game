namespace Scriptables.Cards.Abstractions
{
    public interface ICardEffect
    {
        public ITargetProvider TargetProvider { get; }
        public void OnUse();
    }
}