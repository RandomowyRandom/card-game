using Mirror;

namespace Scriptables.Cards.Abstractions
{
    public interface ISyncTargetProvider: ITargetProvider
    {
        public NetworkIdentity[] GetTargets();
    }
}