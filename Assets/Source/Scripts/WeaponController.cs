using UnityEngine;

public class WeaponController : MonoBehaviour{
    [Header("Objects to Load")] 
    [SerializeField] private ObjectPooler _bulletPool;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private AudioClip _shootSfx;

    [Header("Ammo")] 
    [SerializeField] public int _maxAmmo;
    [SerializeField] public int _currentAmmo;
    [SerializeField] private bool _isInfiniteAmmo;

    [Header("Bullets")] 
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootRate;
    
    private float _lastShootTime;
    private bool _isPlayer;
    private AudioSource _audioSource;

    private void Awake(){
        CheckForPlayer();
        GetAudioSource();
    }

    private void CheckForPlayer(){
        if (GetComponent<PlayerController>())
            _isPlayer = true;
    }

    private void GetAudioSource(){
        _audioSource = GetComponent<AudioSource>();
    }

    public bool CheckToShoot(){
        if (Time.time - _lastShootTime >= _shootRate && (_currentAmmo > 0 || _isInfiniteAmmo))
            return true;
        
        return false;
    }

    public void Shoot(){
        _lastShootTime = Time.time;
        _currentAmmo--;

        if (_isPlayer)
            GameUIController._instance.UpdateAmmoText(_currentAmmo, _maxAmmo);
        
        _audioSource.PlayOneShot(_shootSfx);
        
        GameObject bullet = _bulletPool.GetObject();
        bullet.transform.position = _muzzle.position;
        bullet.transform.rotation = _muzzle.rotation;
        
        bullet.GetComponent<Rigidbody>().velocity = _muzzle.forward * _bulletSpeed;
    }
}
