using Pool;
using UnityEngine;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private BulletPool _bulletPool;
        [SerializeField] private Transform _muzzle; // точка, откуда вылетает пуля
        [SerializeField] private float _fireRate = 8f;
        
        private float _nextFireTime;

        public bool TryFire()
        {
            if(Time.time < _nextFireTime)
                return false;
            
            //_nextFireTime = Time.time + 1f / _fireRate;
            _nextFireTime = Time.time + _fireRate;

            Bullet bullet = _bulletPool.Get(); // достаем пульку из пула
            //bullet.transform.SetLocalPositionAndRotation(_muzzle.position, _muzzle.rotation); // перемещаем пульку к дулу оружия
            bullet.Launch(_muzzle, _muzzle.rotation);
            return true;
        }
    }
}