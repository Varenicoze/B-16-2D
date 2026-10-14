using Pool;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Rigidbody2D))]
    
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _speed = 30f;
        [SerializeField] private float _lifeTime = 3f;
        [SerializeField] private int _damage = 10;
        
        private Rigidbody2D _rb;
        private float _lifeTimer;
        private BulletPool _ownerPool;
        
        public BulletPool OwnerPool { get => _ownerPool; set => _ownerPool = value; }
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>(); // _rb - rigidbody
        }

        private void Update()
        {
            _lifeTimer -= Time.deltaTime;
            if (_lifeTimer <= 0f)
            {
                if (_ownerPool)
                    _ownerPool?.Release(this);
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageble damageable)) //компоненты, которые должны разваливаться
                damageable.TakeDamage(_damage); // разваливаются
            
            _ownerPool?.Release(this);
        }

        public void Launch(Transform player, Quaternion rotation)
        {
            transform.SetPositionAndRotation(player.position, rotation);
            OnSpawned(player);
        }

        public void OnSpawned(Transform playerSide)
        {
            _lifeTimer = _lifeTime;
            
            if (playerSide.localScale.x > 0) // игрок смотрит вправо - пуля летит вправо
            {
                _rb.linearVelocity = transform.right * _speed; // запускает пулю
                return;
            }
            _rb.linearVelocity = -transform.right * _speed; // меняет направление 
        }

        public void OnDespawned()
        {
            _rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}