using Mirror;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Player
{
    public class PlayerRound: NetworkBehaviour
    {
        private IRoundManager _roundManager;

        private void Start()
        {
            _roundManager = ServiceLocator.ServiceLocator.Instance.Get<IRoundManager>();
            
            if(!isOwned)
                return;
            
            _roundManager.OnRoundStarted += RoundStarted;
            _roundManager.OnRoundEnded += RoundEnded;
        }

        private void OnDestroy()
        {
            if(!isOwned)
                return;
            
            _roundManager.OnRoundStarted -= RoundStarted;
            _roundManager.OnRoundEnded -= RoundEnded;
        }

        private void RoundEnded(PlayerData playerData)
        {
            if(!playerData.isOwned)
                return;
            
            Debug.Log("Round Ended");
        }

        private void RoundStarted(PlayerData playerData)
        {
            if(!playerData.isOwned)
                return;
            
            Debug.Log("Round Started");
        }
    }
}