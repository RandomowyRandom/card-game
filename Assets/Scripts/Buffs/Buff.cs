using System.Collections.Generic;
using Common.Attributes;
using Cysharp.Threading.Tasks;
using Scriptables.Cards.Abstractions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Buffs
{
    [ScriptableFactoryElement]
    public class Buff: SerializedScriptableObject
    {
        [SerializeField]
        private string _buffName;

        [SerializeField] [TextArea]
        private string _description;
        
        [OdinSerialize]
        private List<ICardEffect> _cardEffects;
        
        [SerializeField]
        private int _roundDuration;

        public int RoundDuration => _roundDuration;

        public virtual async UniTask InvokeEffects()
        {
            foreach (var effect in _cardEffects)
            {
                if(effect.IsAsync)
                    await effect.OnUse();
                else
                    effect.OnUse();
            }
        }
    }
}