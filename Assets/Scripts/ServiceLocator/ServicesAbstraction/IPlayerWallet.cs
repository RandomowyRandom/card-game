using System;

namespace ServiceLocator.ServicesAbstraction
{
    public interface IPlayerWallet: IService
    {
        event Action<int> OnMoneyChanged; 

        public int MoneyAmount { get; }
        
        public void AddMoney(int amount);
        public void SubtractMoney(int amount);
        public bool CanAfford(int amount);
        public void SetMoney(int amount);
    }
}