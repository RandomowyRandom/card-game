using Buffs;
using Cysharp.Threading.Tasks;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.Serialization;

namespace Scriptables.Cards.Effects
{
    public class ApplyBuffCardEffect: ICardEffect
    {
        [field: OdinSerialize]
        public ITargetProvider TargetProvider { get; }
        
        [OdinSerialize]
        private Buff _buff;
        
        public bool IsAsync => TargetProvider is IAsyncTargetProvider;
        
        public async UniTask OnUse()
        {
            var targets = TargetProvider switch
            {
                ISyncTargetProvider syncTargetProvider => syncTargetProvider.GetTargets(),
                IAsyncTargetProvider asyncTargetProvider => await asyncTargetProvider.GetTargets(),
                _ => null
            };

            foreach (var target in targets!)
            {
                target.GetComponent<IPlayerBuffHandler>()?.ApplyBuff(_buff);
            }
        }
    }
}