using Cysharp.Threading.Tasks;
using Player;

namespace ServiceLocator.ServicesAbstraction
{
    public interface IEnemySelectionManager: IService
    {
        public UniTask<PlayerData> GetSelectedEnemy();
        public void DeselectEnemy();
    }
}