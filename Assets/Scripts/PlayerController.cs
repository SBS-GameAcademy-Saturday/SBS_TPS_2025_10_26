using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Aim Settings")] [SerializeField]
    private CinemachineThirdPersonFollow personFollow;

    [Header("Locomotion Settings")] [SerializeField]
    private float moveSpeed = 2.0f;

    [SerializeField] private float sprintSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 5.0f;

    [Header("Jump Settings")] [SerializeField]
    private float gravity = -9.81f;

    [SerializeField] private float fallingSpeed = 0.5f;

    [Header("Surface Detection Settings")] [SerializeField]
    private Transform surfaceOffset;

    [SerializeField] private float surfaceDistance;
    [SerializeField] private LayerMask surfaceMask;
    [SerializeField] private float jumpRange = 1;

    [Header("Shot Settings")] [SerializeField]
    private float fireCharge = 15.0f;

    [SerializeField] private float shotRange = 100.0f;
    [SerializeField] private float damage = 10.0f;

    [Header("Recoil Settings")] [SerializeField]
    private float normalRange = 0.02f;

    [SerializeField] private float aimmingRange = 0.01f;

    [Header("Shot Effects")] [SerializeField]
    private ParticleSystem muzzleSpark;

    [SerializeField] private ParticleSystem impactEffect;

    [Header("Lock Setting")]
    [SerializeField] private float mouse_Y_Max = 15.0f;
    [SerializeField] private float mouse_Y_Min = -15.0f;
    [SerializeField] private Transform thirdPersonCameraTarget;
    [SerializeField] private float mouse_Y_Axis_Speed = 1.0f;

    private Vector2 _movementInput = Vector2.zero;
    private Vector2 _lookInput = Vector2.zero;
    private CharacterController _controller;
    private Animator _animator;
    private bool _isSprint;
    private bool _isOnSurface = true;
    private Vector3 _velocity = Vector3.zero;
    private bool _shot = false;
    private float _nextTimeToShot = 0.0f;
    private float _cameraLookYAxis = 0;

    private bool _aimming = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Rotate();
        Jump();
        Shot();
    }

    public void OnMoveAction(InputAction.CallbackContext movementValue)
    {
        Vector2 readvalue = movementValue.ReadValue<Vector2>();
        _movementInput = readvalue.normalized;
    }

    public void OnJumpAction(InputAction.CallbackContext context)
    {
        if (context.started && _isOnSurface && !_aimming)
        {
            _velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
            _animator.SetTrigger("Jump");
        }
    }

    public void OnAttackAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _animator.SetBool("Shot", true);
            _shot = true;
        }
        else if (context.canceled)
        {
            _animator.SetBool("Shot", false);
            _shot = false;
        }
    }

    public void OnLookAction(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprintAction(InputAction.CallbackContext sprintValue)
    {
        if (sprintValue.started)
        {
            _isSprint = true;
        }
        else if (sprintValue.canceled)
        {
            _isSprint = false;
        }
    }

    public void OnAimAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _aimming = true;
            _animator.SetBool("Aimming", true);
            personFollow.CameraDistance = 2f;
        }
        else if (context.canceled)
        {
            _aimming = false;
            _animator.SetBool("Aimming", false);
            personFollow.CameraDistance = 3.0f;
        }
    }

    private void Move()
    {
        float currentSpeed = _isSprint ? sprintSpeed : moveSpeed;
        Vector3 direction = new Vector3(_movementInput.x, 0, _movementInput.y);
        if (direction.sqrMagnitude < 0.1f)
        {
            currentSpeed = 0;
        }

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                            cinemachineCamera.transform.eulerAngles.y;
        Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        _controller.Move(moveDirection * currentSpeed * Time.fixedDeltaTime);
        _animator.SetFloat("Speed", _movementInput.y < 0 ? -currentSpeed : currentSpeed);
        _animator.SetFloat("Horizontal", _movementInput.x * currentSpeed);
        _animator.SetFloat("Vertical", _movementInput.y * currentSpeed);
    }

    private void Rotate()
    {
        float mouseX = _lookInput.x * Time.fixedDeltaTime * rotateSpeed;
        float mouseY = _lookInput.y * Time.fixedDeltaTime * mouse_Y_Axis_Speed;

        _cameraLookYAxis -= mouseY;
        _cameraLookYAxis = Mathf.Clamp(_cameraLookYAxis, mouse_Y_Min, mouse_Y_Max);
        thirdPersonCameraTarget.localEulerAngles = new Vector3(_cameraLookYAxis, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Jump()
    {
        _isOnSurface = Physics.CheckSphere(surfaceOffset.position, surfaceDistance, surfaceMask);
        if (_isOnSurface && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += gravity * Time.fixedDeltaTime;
        _controller.Move(_velocity * Time.fixedDeltaTime * fallingSpeed);
    }

    private void Shot()
    {
        if (!_shot)
            return;
        if (_shot && Time.time >= _nextTimeToShot)
        {
            Debug.Log("Shot Fired");
            _nextTimeToShot = Time.time + 1f / fireCharge;
            muzzleSpark.Play();
            Shooting();
            return;
        }
    }

    private void Shooting()
    {
        bool isHit = Physics.Raycast(cinemachineCamera.transform.position, GetRandomForward(), out RaycastHit hitInfo,
            shotRange);
        if (isHit)
        {
            if (hitInfo.collider.TryGetComponent<Damagable>(out Damagable damagable)) 
                damagable.HitDamage(damage);
            ParticleSystem effect = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(effect.gameObject, 1);
        }
    }

    private Vector3 GetRandomForward()
    {
        Vector3 forward = cinemachineCamera.transform.forward;
        if (_aimming)
        {
            forward.x += Random.Range(-aimmingRange, aimmingRange);
            forward.y += Random.Range(-aimmingRange, aimmingRange);
            return forward;
        }
        else
        {
            forward.x += Random.Range(-normalRange, normalRange);
            forward.y += Random.Range(-normalRange, normalRange);
            return forward;
        }
    }
}