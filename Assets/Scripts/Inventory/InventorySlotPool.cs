using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class PooledSlot
    {
        public GameObject GameObject { get; private set;  }
        public InventorySlotView View { get; private set; }

        public PooledSlot(GameObject gameObject)//конструктор - метод
        {
            GameObject = gameObject;
            View = gameObject.GetComponent<InventorySlotView>();
        }
    }

    public class InventorySlotPool
    {
        private readonly GameObject _slotPrefab;
        private readonly Transform _poolRoot;
        private readonly Queue<PooledSlot> _pool = new();

        public InventorySlotPool(GameObject slotPrefab, Transform poolRoot, int capacity) // создаем массив из какого-то количества ячеек
        {
            _slotPrefab = slotPrefab;
            _poolRoot = poolRoot;


            for (int i = 0; i < capacity; i++)
            {
                var obj = Object.Instantiate(slotPrefab, _poolRoot);
                obj.SetActive(false);
                _pool.Enqueue(new PooledSlot(obj)); //отдаем
            }
        }

        public PooledSlot Rent(Transform parent)// попросят ячейки - отдадим
        {
            PooledSlot pooled = _pool.Count > 0 // если _pool.Count > 0, то мы достаем из pool 1-ый инкремент и засовываем в polled
                ? _pool.Dequeue()
                : new PooledSlot(Object.Instantiate(_slotPrefab)); // если _pool.Count <= 0, то создаем новый элемент мы его сразу оттаем
            
            pooled.GameObject.transform.SetParent(parent, false);
            pooled.GameObject.SetActive(true);
            return pooled;
        }

        public void Return(PooledSlot pooled)
        {
            SlotEventBinder.Unbind(pooled.GameObject);
            pooled.View.Unbind();//когда мы возвращаем предмет в pool, мы должны отписаться
            pooled.GameObject.SetActive(false);
            pooled.GameObject.transform.SetParent(_poolRoot, false);
            _pool.Enqueue(pooled);
        }
    }
}