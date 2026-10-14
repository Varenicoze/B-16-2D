using System.Collections.Generic;
using Inventory.Container;
using UI.SeparatePanel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Zenject;

namespace Inventory
{
    public class DynamicInventoryView : InventoryView
    {
        [Tooltip("Включи, если можно разделять предмет в этом инвентаре")]
        [SerializeField] private bool allowSplitItem;
        [SerializeField] private SplitWindowController splitWindowController;
        
        private InventorySlot _pendingSplitSource;
        private int _pendingSplitAmount;
        
        private InventorySlotPool _pool;
        private readonly List<PooledSlot> _activeSlot = new();
        
        [Inject]
        public void Construct(InventorySlotPool pool)
        {
            _pool = pool;
        }
        
        public override void CreateSlots()
        {
            slots = inventory.container.Items;

            for (int i = 0; i < slots.Length; i++)
            {
                var pooled = _pool.Rent(transform);
                InventorySlotBinder.Bind(pooled.GameObject,
                    pooled.View,
                    slots[i],
                    this,
                    inventory.database);
                _activeSlot.Add(pooled);
            }
        }
        
        public override void ClearSlots()
        {
            foreach (var pooled in _activeSlot)
            {
                _pool.Return(pooled);
            }
            _activeSlot.Clear();
        }

        public override void OnLeftClick(InventorySlot slot)
        {
            if (_pendingSplitSource != null)
            {
                CompleteSplit(slot);
                return;
            }
            
            if (allowSplitItem && slot.ID >= 0 && Keyboard.current.leftShiftKey.isPressed)
            {
                
                splitWindowController.RequestSplit(
                    slot,
                    slot.amount,
                    amount => BeginSplit(slot, amount));
            }
        }

        private void BeginSplit(InventorySlot source, int amount)
        {
            _pendingSplitSource = source;
            _pendingSplitAmount = amount;
            StartManualDrag(source.ID);
        }
        
        private void CompleteSplit(InventorySlot target)
        {
            inventory.SplitItem(_pendingSplitSource, target, _pendingSplitAmount);
            ClearPendingSplit();

        }

        private void CancelSplit()
        {
            ClearPendingSplit();
        }
        
        private void ClearPendingSplit()
        {
            _pendingSplitSource = null;
            _pendingSplitAmount = 0;
            StopManualDrag();
        }
    }
}