using System;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    [Header("Stats")] 
    [SerializeField] private int _maxHp;
    [SerializeField] private int _currentHp;
    
    [Header("Movement")] 
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    
    [Header("Camera")] 
    [SerializeField] private float _lookSensivity;
    [SerializeField] private float _minLookX;
    [SerializeField] private float _maxLookX;
    private float _xRotation;

    private Camera _camera;
    private Rigidbody _rb;
    private WeaponController _weapon;

    private void Awake(){
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody>();
        _weapon = GetComponent<WeaponController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start(){
        SetupUI();
    }
    
    private void Update(){
        if (GameManager._instance._gamePaused)
            return;
        
        Movement();
        CameraLook();
        InputListener();
    }

    private void Movement(){
        float movementXInput = Input.GetAxis("Horizontal") * _moveSpeed;
        float movementZInput = Input.GetAxis("Vertical") * _moveSpeed;

        Vector3 direction = transform.right * movementXInput + transform.forward * movementZInput;
        direction.y = _rb.velocity.y;

        _rb.velocity = direction;
    }

    private void CameraLook(){
        float cameraYInput = Input.GetAxis("Mouse X") * _lookSensivity;
        _xRotation += Input.GetAxis("Mouse Y") * _lookSensivity;

        _xRotation = Mathf.Clamp(_xRotation, _minLookX, _maxLookX);

        _camera.transform.localRotation = Quaternion.Euler(-_xRotation, 0, 0);
        transform.eulerAngles += Vector3.up * cameraYInput;
    }

    private void InputListener(){
        if (Input.GetButtonDown("Jump"))
            HandleJump();
        if (Input.GetButtonDown("Fire1"))
            HandleShoot();
    }

    private void HandleJump(){
        Ray ray = new Ray(transform.position, Vector3.down);
        
        if(Physics.Raycast(ray, 1.1f))
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void HandleShoot(){
        if(_weapon.CheckToShoot())
            _weapon.Shoot();
    }

    public void TakeDamage(int damage){
        _currentHp -= damage;
        GameUIController._instance.UpdateHealthBar(_currentHp, _maxHp);
        if(_currentHp <= 0)
            Die();
    }

    private void Die(){
        GameManager._instance.HandleLoss();
    }

    public void GiveHealth(int amount){
        _currentHp = Mathf.Clamp(_currentHp + amount, 0, _maxHp);
        GameUIController._instance.UpdateHealthBar(_currentHp, _maxHp);
    }

    public void GiveAmmo(int amount){
        _weapon._currentAmmo = Mathf.Clamp(_weapon._currentAmmo + amount, 0, _weapon._maxAmmo);
        GameUIController._instance.UpdateAmmoText(_weapon._currentAmmo, _weapon._maxAmmo);
    }

    private void SetupUI(){
        GameUIController._instance.UpdateHealthBar(_currentHp, _maxHp);
        GameUIController._instance.UpdateScoreText(0);
        GameUIController._instance.UpdateAmmoText(_weapon._currentAmmo, _weapon._maxAmmo);
    }
}
