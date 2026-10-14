using System;
using Inventory.Container;
using UnityEngine;

namespace Inventory
{
    public class StaticInventory : InventoryView
    {
        private InventorySlotView[] _staticSlot;//проверяем неймспейсы, скобочки не забываем
        public event Action<InventorySlot> OnItemSelected;
        
        private void Start()
        {
            CreateSlots();
            SlotEventBinder.BindInventoryUIEvent(gameObject, this);
        }

        public override void CreateSlots()
        {
            var inventorySlots = inventory.container.Items;

            if (inventorySlots.Length != transform.childCount)
            {
                Debug.LogError(
                    $"[{name} Число слотов в инвентаре ({inventorySlots.Length})" +
                    $"не соовпадает с числом дочерних объектов в сцене ({transform.childCount}). Проверьте префаб/инспектор.",
                    this);
                return;
            }

            slots = inventorySlots;
            _staticSlot = new InventorySlotView[inventorySlots.Length];
            
            for (int i = 0; i < _staticSlot.Length; i++) //была ошибка NullReference, ибо нету _staticSlot.Length
            {
                var obj = transform.GetChild(i).gameObject;
                _staticSlot[i] = obj.GetComponent<InventorySlotView>();
                
                InventorySlotBinder.Bind(obj,
                    _staticSlot[i],
                    inventorySlots[i],
                    this,
                    inventory.database);
            }
        }

        public override void OnLeftClick(InventorySlot slot)
        {
            if(slot.item == null)
                return;
            
            OnItemSelected?.Invoke(slot);
        }
    }
}