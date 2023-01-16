using System;

namespace ServiceLocator.ServicesAbstraction
{
    public interface IPlayerEnergy: IService
    {
        public event Action<int> OnEnergyChanged;
        
        public int MaxEnergy { get; }
        public int CurrentEnergy { get; }
        
        public void AddEnergy(int amount);
        public void RemoveEnergy(int amount);
        public void SetEnergy(int amount);
    }
}