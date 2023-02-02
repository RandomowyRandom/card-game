using Cysharp.Threading.Tasks;
using Mirror;

namespace Scriptables.Cards.Abstractions
{
    public interface IAsyncTargetProvider: ITargetProvider
    {
        public UniTask<NetworkIdentity[]> GetTargets();
    }
}