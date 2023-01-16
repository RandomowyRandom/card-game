using Mirror;

namespace Scriptables.Cards.Abstractions
{
    public interface ITargetProvider
    {
        public NetworkIdentity[] GetTargets();
    }
}