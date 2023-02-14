using System.Collections.Generic;
using Buffs;
using Helpers;
using Mirror;
using ServiceLocator.ServicesAbstraction;

namespace Player
{
    public class PlayerBuffHandler: NetworkBehaviour, IPlayerBuffHandler
    {
        private List<PlayerBuff> _buffs = new(); // TODO: implement the logic of network sync similar to the one in PlayerHand
        
        private readonly SyncList<PlayerBuff> _buffNames = new(); // TODO: as above
        
        private IRoundManager _roundManager;
        
        private IRoundManager RoundManager => _roundManager ??= ServiceLocator.ServiceLocator.Instance.Get<IRoundManager>();
        
        public override void OnStartAuthority()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerBuffHandler>(this);
            
            RoundManager.OnRoundStarted += InvokeBuffEffects;
        }

        private async void InvokeBuffEffects(PlayerData player)
        {
            if(!player.isOwned)
                return;

            foreach (var playerBuff in _buffNames)
            {
                await playerBuff.Buff.InvokeEffects();
                playerBuff.DecreaseRemainingRounds();
                
                if(playerBuff.RemainingRounds <= 0)
                    RemoveBuff(playerBuff);
            }
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerBuffHandler>();
            
            RoundManager.OnRoundStarted -= InvokeBuffEffects;
        }

        public void ApplyBuff(Buff buff)
        {
            CmdApplyBuff(buff.name);
        }     
        
        public void RemoveBuff(PlayerBuff buff)
        {
            CmdRemoveBuff(_buffNames.IndexOf(buff));
        }

        #region Networking

        [Command(requiresAuthority = false)]
        private void CmdApplyBuff(string buffName)
        {
            var buff = NetworkedScriptableLoader.GetScriptable<Buff>(buffName);
            _buffNames.Add(new PlayerBuff(buff));
        }
        
        [Command(requiresAuthority = false)]
        private void CmdRemoveBuff(int index)
        {
            _buffNames.RemoveAt(index);
        }

        #endregion
    }
}