using Cysharp.Threading.Tasks;
using Scriptables.Cards.Abstractions;
using Sirenix.Serialization;

namespace Scriptables.Cards.Effects
{
    public class ApplyBuffCardEffect: ICardEffect
    {
        [field: OdinSerialize]
        public ITargetProvider TargetProvider { get; }
        
        public bool IsAsync => TargetProvider is IAsyncTargetProvider;
        
        public UniTask OnUse()
        {
            throw new System.NotImplementedException();
        }
    }
}