using Cysharp.Threading.Tasks;

namespace Scriptables.Cards.Abstractions
{
    public interface ICardEffect
    {
        public ITargetProvider TargetProvider { get; }
        public bool IsAsync { get; }
        public UniTask OnUse();
    }
}