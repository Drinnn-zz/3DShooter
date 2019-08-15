using UnityEngine;

public class BulletController : MonoBehaviour{
    [SerializeField] private int _damageToGive;
    [SerializeField] private float _lifetime;
    [SerializeField] private GameObject _hitParticle;

    private float _shootTime;

    private void OnEnable(){
        _shootTime = Time.time;
    }

    private void Update(){
        VerifyLifetime();
    }

    private void VerifyLifetime(){
        if(Time.time - _shootTime >= _lifetime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player"))
            other.GetComponent<PlayerController>()
                .TakeDamage(_damageToGive);
        else if(other.CompareTag("Enemy"))
            other.GetComponent<EnemyController>()
                .TakeDamage(_damageToGive);

        GameObject particle = Instantiate(_hitParticle, transform.position, Quaternion.identity);
        Destroy(particle, 0.5f);
        
        gameObject.SetActive(false);
    }
}
