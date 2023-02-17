using System.Collections.Generic;
using System.Linq;
using Buffs;
using Helpers;
using JetBrains.Annotations;
using Mirror;
using ServiceLocator.ServicesAbstraction;
using UnityEngine;

namespace Player
{
    public class PlayerBuffHandler: NetworkBehaviour, IPlayerBuffHandler
    {
        private List<PlayerBuff> _buffs = new();
        
        private readonly SyncList<string> _buffNames = new();
        
        private IRoundManager _roundManager;
        
        private IRoundManager RoundManager => _roundManager ??= ServiceLocator.ServiceLocator.Instance.Get<IRoundManager>();
        
        public override void OnStartAuthority()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerBuffHandler>(this);
            
            RoundManager.OnRoundStarted += InvokeBuffEffects;
            _buffNames.Callback += HandleBuffUpdate;
        }

        private async void InvokeBuffEffects(PlayerData player)
        {
            if(!player.isOwned)
                return;

            List<PlayerBuff> buffsToRemove = new();

            foreach (var playerBuff in _buffs)
            {
                await playerBuff.Buff.InvokeEffects();
                playerBuff.DecreaseRemainingRounds();
                
                if(playerBuff.RemainingRounds <= 0)
                    buffsToRemove.Add(playerBuff);
            }
            
            foreach (var buff in buffsToRemove)
            {
                RemoveBuff(buff);
            }
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerBuffHandler>();
            
            RoundManager.OnRoundStarted -= InvokeBuffEffects;
            
            if(isOwned)
                _buffNames.Callback -= HandleBuffUpdate;
        }

        public void ApplyBuff(Buff buff)
        {
            CmdApplyBuff(buff.name);
            
            Debug.Log($"Applied buff {buff.name}");
        }     
        
        public void RemoveBuff(PlayerBuff buff)
        {
            CmdRemoveBuff(_buffNames.IndexOf(buff.Buff.name));
        }

        private void HandleBuffUpdate(SyncList<string>.Operation op, int itemIndex, string oldItem, string newItem)
        {
            if(!isOwned)
                return;
            
            switch (op)
            {
                case SyncList<string>.Operation.OP_ADD:
                    var buff = NetworkedScriptableLoader.GetScriptable<Buff>(newItem, ScriptablePath.Buffs);
                    _buffs.Add(new PlayerBuff(buff));
                    break;
                case SyncList<string>.Operation.OP_REMOVEAT:
                    _buffs.RemoveAt(itemIndex);
                    break;
            }
        }
        
        #region Networking

        [Command(requiresAuthority = false)]
        private void CmdApplyBuff(string buffName)
        {
            _buffNames.Add(buffName);
        }
        
        [Command(requiresAuthority = false)]
        private void CmdRemoveBuff(int index)
        {
            _buffNames.RemoveAt(index);
        }

        #endregion

        #region QC

        [QFSW.QC.Command("log-all-buffs")] [UsedImplicitly]
        private void CommandLogAllBuffs()
        {
            var localBuffHandler = FindObjectsOfType<PlayerBuffHandler>().Where(p => p.isOwned).ToArray();
            
            foreach (var buff in localBuffHandler[0]._buffs)
            {
                Debug.Log(buff.Buff.name);
            }
        }

        #endregion
    }
}