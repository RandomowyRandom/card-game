using Cysharp.Threading.Tasks;
using Player;
using Player.Hand;
using Player.Hand.States;
using Scriptables.Cards.Abstractions;
using ServiceLocator.ServicesAbstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using StateMachine;
using UnityEngine;

namespace Common.Managers
{
    public class EnemySelectionManager: SerializedMonoBehaviour, IEnemySelectionManager
    {
        [SerializeField]
        private PlayerHandStateMachine _playerHandStateMachine;
        
        [Space(5)]
        
        [OdinSerialize]
        private IState _activeState;
        
        [OdinSerialize]
        private IState _enemySelectionState;
        
        private Camera _camera;
        private PlayerData _selectedEnemy;
        private ICardSelectionHandler _cardSelectionHandler;

        private void Awake()
        {
            _camera = Camera.main;
            ServiceLocator.ServiceLocator.Instance.Register<IEnemySelectionManager>(this);
            
            Card.OnCardUsedLocally += ResetSelection;
        }

        private void Start()
        {
            _cardSelectionHandler = ServiceLocator.ServiceLocator.Instance.Get<ICardSelectionHandler>();
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IEnemySelectionManager>();
            
            Card.OnCardUsedLocally -= ResetSelection;
        }

        public async UniTask<PlayerData> GetSelectedEnemy()
        {
            if(_selectedEnemy != null)
                return _selectedEnemy;

            _playerHandStateMachine.SetState(_enemySelectionState);
            
            Debug.Log("Waiting for enemy selection");
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
            
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit))
                return null;
            
            var enemy = hit.collider.GetComponent<PlayerData>();
            
            if (enemy == null)
                return null;
            
            if(enemy.isOwned)
                return null;
            
            _selectedEnemy = enemy;
            _playerHandStateMachine.SetState(_activeState);
            
            return _selectedEnemy;
        }

        public void DeselectEnemy()
        {
            _selectedEnemy = null;
        }
        
        private void ResetSelection(Card obj)
        {
            DeselectEnemy();
        }
    }
}