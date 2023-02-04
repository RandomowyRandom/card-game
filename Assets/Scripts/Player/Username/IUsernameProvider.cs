using ServiceLocator;

namespace Player.Username
{
    public interface IUsernameProvider: IService
    {
        public string GetUsername();
    }
}