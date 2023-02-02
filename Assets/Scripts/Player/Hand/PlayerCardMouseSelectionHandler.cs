using System;
using Cards;
using UnityEngine;

namespace Player.Hand
{
    public class PlayerCardMouseSelectionHandler: MonoBehaviour, ICardSelectionHandler
    {
        [SerializeField]
        private LayerMask _layerMask;
        
        public event Action<CardWorld> OnSelected; 
        public event Action<CardWorld> OnDeselected;

        private CardWorld _selectedCard;
        
        private Camera _camera;
        
        private bool _isSelectionBlocked;

        public CardWorld SelectedCard => _selectedCard;

        private void Update()
        {
            if (_isSelectionBlocked)
                return;
            
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (!Physics.Raycast(ray, out var hitInfo, float.PositiveInfinity, _layerMask))
            {
                if (_selectedCard == null)
                    return;
                
                OnDeselected?.Invoke(_selectedCard);
                _selectedCard = null;

                return;
            }
            
            var newlySelectedCard = hitInfo.collider.GetComponent<CardWorld>();

            if (newlySelectedCard == _selectedCard) 
                return;
            
            if(_selectedCard != null)
                OnDeselected?.Invoke(_selectedCard);
            
            OnSelected?.Invoke(newlySelectedCard);
            _selectedCard = newlySelectedCard;
        }

        private void Awake()
        {
            _camera = Camera.main;
            
            if(ServiceLocator.ServiceLocator.Instance.IsRegistered<ICardSelectionHandler>())
                return;
            
            ServiceLocator.ServiceLocator.Instance.Register<ICardSelectionHandler>(this);
        }
        
        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<ICardSelectionHandler>();
        }
        
        public void BlockSelection(bool block)
        {
            _isSelectionBlocked = block;
        }
    }
}