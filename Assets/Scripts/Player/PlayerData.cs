using System;
using Mirror;
using Player.Username;
using ServiceLocator.ServicesAbstraction;

namespace Player
{
    public class PlayerData : NetworkBehaviour, IPlayerData
    {
    public event Action<string> OnUsernameChanged;

    [SyncVar(hook = nameof(UsernameChanged))]
    private string _username;

    public string Username => _username;

    private void OnDestroy()
    {
        if (!isOwned)
            return;
            
        ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerEnergy>();
    }
    
    public override void OnStartAuthority()
    {
        ServiceLocator.ServiceLocator.Instance.Register<IPlayerData>(this);
        
        var usernameProvider = ServiceLocator.ServiceLocator.Instance.Get<IUsernameProvider>();
        CmdSetUsername(usernameProvider.GetUsername());
    }

    private void UsernameChanged(string oldUsername, string newUsername)
    {
        OnUsernameChanged?.Invoke(newUsername);
    }

    #region Networking

    [Command]
    private void CmdSetUsername(string username)
    {
        _username = username;
    }

    #endregion

    }
}