using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyController : MonoBehaviour{
   [Header("Stats")] 
   [SerializeField] private int _currentHp;
   [SerializeField] private int _maxHp;
   [SerializeField] private int _scoreToGive;

   [Header("Movement")] 
   [SerializeField] private float _moveSpeed;
   [SerializeField] private float _attackRange;
   [SerializeField] private float _yPathOffset;

   private List<Vector3> _path;
   
   private WeaponController _weapon;
   private GameObject _target;

   private void Awake(){
      _weapon = GetComponent<WeaponController>();
      _target = FindObjectOfType<PlayerController>().gameObject;
   }

   private void Start(){
      InvokeRepeating(nameof(UpdatePath), 0.0f, 0.5f);
   }

   private void Update(){
      Movement();
   }

   private void Movement(){
      float distance = Vector3.Distance(transform.position, _target.transform.position);
      if (distance <= _attackRange){
         if(_weapon.CheckToShoot())
            _weapon.Shoot();
      }
      else
         ChaseTarget();
      
      CalculatePlayerAngle();
   }

   private void ChaseTarget(){
      if (_path != null){
         if (_path.Count == 0)
            return;
         Vector3 pathOffset = new Vector3(0, _yPathOffset, 0);
         transform.position = Vector3.MoveTowards(transform.position, 
            _path[0] + pathOffset, _moveSpeed * Time.deltaTime);
         if(transform.position == _path[0] + pathOffset)
            _path.RemoveAt(0);
      }
   }

   private void CalculatePlayerAngle(){
      Vector3 targetDirection = (_target.transform.position - transform.position).normalized;
      float angle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

      transform.eulerAngles = Vector3.up * angle;
   }

   private void UpdatePath(){
      NavMeshPath navMeshPath = new NavMeshPath();
      NavMesh.CalculatePath(transform.position, _target.transform.position,
         NavMesh.AllAreas, navMeshPath);
      _path = navMeshPath.corners.ToList();
   }

   public void TakeDamage(int damage){
      _currentHp -= damage;
      if (_currentHp <= 0)
         Die();
   }

   private void Die(){
      GameManager._instance.AddScore(_scoreToGive);
      Destroy(gameObject);
   }
}
