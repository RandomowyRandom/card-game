using System;

namespace ServiceLocator.ServicesAbstraction
{
    public interface IPlayerHealth: IService
    {
        public event Action<int> OnHealthChanged;
        
        public int CurrentHealth { get; }
        
        public void Heal(int health);
        public void SetHealth(int health);
        public void AddArmor(int armor);
        public void SetArmor(int armor);
        public void RemoveArmor(int armor);
        public void TakeDamage(int damage);
    }
}