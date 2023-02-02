using System;
using Cysharp.Threading.Tasks;
using Mirror;
using Player;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;

namespace Scriptables.Cards.Targets
{
    [Serializable]
    public class EnemyTarget: IAsyncTargetProvider
    {
        private IEnemySelectionManager _enemySelectionManager;
        public async UniTask<NetworkIdentity[]> GetTargets()
        {
            _enemySelectionManager ??= ServiceLocator.ServiceLocator.Instance.Get<IEnemySelectionManager>();

            PlayerData enemy;
            
            do
                enemy = await _enemySelectionManager.GetSelectedEnemy();
            while 
                (enemy == null);

            var enemyNetworkIdentity = enemy.GetComponent<NetworkIdentity>();
            
            return new []{enemyNetworkIdentity};
        }
    }
}