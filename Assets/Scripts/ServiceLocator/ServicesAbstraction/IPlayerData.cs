using System;

namespace ServiceLocator.ServicesAbstraction
{
    public interface IPlayerData: IService
    {
        public event Action<string> OnUsernameChanged;
        
        public string Username { get; }
    }
}