using Scriptables.Cards.Abstractions;
using UnityEngine;

namespace Scriptables.Cards
{
    public class CardAttack : Card, IAttackCard
    {
        [field: SerializeField]
        public int Damage { get; }

        public override void Use()
        {
            Debug.Log($"Card {CardName} is used");
        }
    }
}