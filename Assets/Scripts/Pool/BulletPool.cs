using Combat;
using UnityEngine;

namespace Pool// err?
{
    public class BulletPool : MonoBehaviour, IPool<Bullet> //создает пули, отвечает за них
    {
        [SerializeField] private Bullet _bulletPrefab; // ссылка на префаб пули, который будет клонироваться для заполнения пула
        [SerializeField] private int _prewarmCount = 20; // количество созданных изначально
        [SerializeField] private int _maxSize = 100; // максимальное кол-во 
        [SerializeField] private Transform _container; // сюда будут складываться пульки
        
        private ObjectPool<Bullet> _pool;

        private void Awake()
        {
            if (!_container)
                _container = transform;
            
            _pool = new ObjectPool<Bullet>(
                factory: CreateBullet,
                destroyAction: bullet => Destroy(bullet.gameObject),
                prewarmCount: _prewarmCount,
                maxSize: _maxSize);
        }

        public Bullet Get() // возвращает пулю активную из пула с помощью вызова _pool.Get()
        {
            Bullet bullet  = _pool.Get();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        // возвращает отработанную пульку обратно в пул 
        public void Release(Bullet bullet) => _pool.Release(bullet);

        private Bullet CreateBullet() // 
        {
            Bullet bullet = Instantiate(_bulletPrefab, _container);
            bullet.OwnerPool = this;
            return bullet;
        }

        private void OnDestroy() => _pool.ReleaseAll();
    }
}