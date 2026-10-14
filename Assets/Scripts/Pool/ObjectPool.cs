using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public interface IPool<T>
    {
        T Get();
        void Release(T item);
    }

    public interface IPoolable
    {
        void OnSpawned(Transform parent);
        void OnDespawned();
    }
    
    public class ObjectPool<T> : IPool<T> where T : class, IPoolable
    {
        private readonly Func<T> _factory; //Func<T> - создай конкретно этот объект
        private readonly Action<T> _destroyAction;
        private readonly Stack<T> _available = new(); //Stack<T> (последний пришел, последний ушел)
        private readonly List<T> _allActive = new();
        private readonly int _maxSize;
        private IPool<T> _poolImplementation;

        public ObjectPool(Func<T> factory, Action<T> destroyAction, int prewarmCount = 0, int maxSize = 100)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _destroyAction = destroyAction;
            _maxSize = maxSize;
            Prewarm(prewarmCount);
        }

        /*public T Get(Transform playerSide) 
        {
            T item = _available.Count == 0? _factory() : _available.Pop();
            _allActive.Add(item);
            
            item.OnSpawned(playerSide); //??
            return item;
        }*/
        
        public T Get()
        {
            T item = _available.Count > 0? _available.Pop() : _factory();
            _allActive.Add(item);
            
            //item.OnSpawned(); 
            return item;
        }

        public void Release(T item)
        {
            if (item == null || _allActive.Remove(item))
                return;
            
            item.OnDespawned();
            if(_available.Count < _maxSize)
                _available.Push(item);
            else
                _destroyAction?.Invoke(item);
        }

        public void ReleaseAll()
        {
            for (int i = 0; i < _allActive.Count; i++) 
                Release(_allActive[i]); 
        }
        
        private void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T item = _factory();
                item.OnDespawned();
                _available.Push(item);
            }
        }
    }
}