using System.Linq;
using JetBrains.Annotations;
using Mirror;
using ServiceLocator.ServicesAbstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using StateMachine;
using UnityEngine;

namespace Player
{
    public class PlayerRound: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private IState _roundStartedState;
        
        [OdinSerialize]
        private IState _roundEndedState;
        
        private IRoundManager _roundManager;
        private NetworkIdentity _networkIdentity;
        private IPlayerHandStateMachine _playerHandStateMachine;

        private IPlayerHandStateMachine PlayerHandStateMachine => _playerHandStateMachine ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerHandStateMachine>();
        
        private void Awake()
        {
            _networkIdentity = GetComponent<NetworkIdentity>();
        }

        private void Start()
        {
            _roundManager = ServiceLocator.ServiceLocator.Instance.Get<IRoundManager>();

            if(!_networkIdentity.isOwned)
                return;
            
            _roundManager.OnRoundStarted += RoundStarted;
            _roundManager.OnRoundEnded += RoundEnded;
        }

        private void OnDestroy()
        {
            if(!_networkIdentity.isOwned)
                return;
            
            _roundManager.OnRoundStarted -= RoundStarted;
            _roundManager.OnRoundEnded -= RoundEnded;
        }
        private void RoundStarted(PlayerData playerRound)
        {
            if(!playerRound.GetComponent<NetworkIdentity>().isOwned)
                return;
         
            PlayerHandStateMachine.SetState(_roundStartedState);
            
            Debug.Log("Round Started");
        }
        
        private void RoundEnded(PlayerData playerRound)
        {
            if(!playerRound.GetComponent<NetworkIdentity>().isOwned)
                return;
            
            PlayerHandStateMachine.SetState(_roundEndedState);

            Debug.Log("Round Ended");
        }

        #region QC

        [QFSW.QC.Command("end-turn")] [UsedImplicitly]
        private void CommandEndTurn()
        {
            _roundManager.NextRound();
        }

        #endregion
    }
}