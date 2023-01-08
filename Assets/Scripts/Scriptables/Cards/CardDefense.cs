using Scriptables.Cards.Abstractions;
using UnityEngine;

namespace Scriptables.Cards
{
    public class CardDefense : Card, IDefenseCard
    {
        [field: SerializeField]
        public int Defense { get; }
        
        public override void Use()
        {
            Debug.Log($"Used {CardName} card");
        }

    }
}