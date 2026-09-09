using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class DynamicInventoryView : InventoryView
    {
        public override void CreateSlots()
        {
            slots = inventory.container.Items;

            for (int i = 0; i < slots.Length; i++)
            {
                var obj = Instantiate(SlotPrefab, transform);
                SlotEventBinder.BindSlotEvent(obj, slots[i], this);
                var view = obj.GetComponent<InventorySlotView>();
                /*view.Bind(slots[i], inventory.database);*/
            }
        }
        
        private InventorySlotPool _pool;

        private readonly List<PooledSlot> _activeSlot = new();

        /*[Inject]
        public void Construct(InventorySlotPool pool)
        {
            _pool = pool;
        }

        public override void CreateSlots()
        {
            slots = inventory.container.Items;

            for (int i = 0; i < slots.Length; i++) ;
            {
                var pooled = _pool.Rent(transform);
                InventorySlotBinder.Bind(pooled.GameObject,
                    pooled.View,
                    slots[i],
                    this,
                    inventory.database);
                _activeSlot.Add(pooled);
            }
        }*/

        /*public override void CreateSlots()
        {
            throw new System.NotImplementedException();
            foreach (var slot in _activeSlot)
            {
                _pool.Return(pooled);
            }
            _activeSlot.Clear();
        }*/
    }
}
