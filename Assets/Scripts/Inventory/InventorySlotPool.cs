using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class PooledSlot
    {
        public GameObject GameObject { get; private set;  }
        public InventorySlotView View { get; private set; }

        public PooledSlot(GameObject gameObject)
        {
            GameObject = gameObject;
            View = gameObject.AddComponent<InventorySlotView>();
        }
    }

    public class InventorySlotPool
    {
        private readonly GameObject _slotPrefab;
        private readonly Transform _poolSlots;
        private readonly Queue<PooledSlot> _pool = new();

        public InventorySlotPool(GameObject slotPrefab, Transform poolSlots, int capacity)
        {
            _slotPrefab = slotPrefab;
            _poolSlots = poolSlots;


            /*for (int i = 0; i < capacity; i++)
            {
                var obj = Object.Instantiate(slotPrefab, poolRoot);
                obj.SetActive(false);
                _pool.Enqueue(new PooledSlot(obj)); //отдаем
            }*/
        }

        public PooledSlot Rent(Transform parent)
        {
            PooledSlot polled = _pool.Count > 0
                ? _pool.Dequeue()
                : new PooledSlot(Object.Instantiate(_slotPrefab));
            return polled;
        }
    }
}