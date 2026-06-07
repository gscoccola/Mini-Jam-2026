using UnityEngine;
using UnityEngine.InputSystem; // Required for the new input system

public class PlayerMovement : Singleton<PlayerMovement>
{
    [SerializeField] private float _moveForce = 5f;
    [SerializeField] private float _ejectForce = 5f;

    private PlayerAnimator _animator;
    private CircleCollider2D _collider;
    private Rigidbody2D _rb;

    private PlayerControls _controls;
    private Vector2 _moveInput;
    private bool _ejectInput;

    private Vector2 _mouseDirection;
    private Camera _camera;

    [SerializeField] private Transform _ejectParticleSystem;
    private ParticleSystem.EmissionModule _ejectEmission;

    [SerializeField] private ParticleSystem _deathParticleSystem;
    private ParticleSystem.EmissionModule _deathEmission;

    [ReadOnly] public float FluidReserve = 100f;
    private float _initialSize;
    private float _initialMass;

    private bool _isDead;
    private float[] _radiusLevels = { 0.26f, 0.35f, 0.56f };

    #region Setup
    protected override void Awake()
    {
        base.Awake();

        //References
        _animator = GetComponent<PlayerAnimator>();
        _collider = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _ejectEmission = _ejectParticleSystem.GetComponent<ParticleSystem>().emission;
        _deathEmission = _deathParticleSystem.GetComponent<ParticleSystem>().emission;

        // Initial values
        _initialSize = _collider.radius;
        _initialMass = _rb.mass;

        // Input setup
        _controls = new PlayerControls();
        _controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
        _controls.Player.Jump.performed += ctx => TryJump(); 
        _controls.Player.Eject.performed += ctx => _ejectInput = true;
        _controls.Player.Eject.canceled += ctx => _ejectInput = false;
    }


    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    #endregion

    private void Update()
    {
        if (_isDead) return;

        // Horizontal Movement
        _moveInput = _controls.Player.Move.ReadValue<Vector2>();
        Vector3 movement = new Vector3(_moveInput.x, 0f, 0f) ;
        if (IsOnFloor()) _rb.AddForce(movement.normalized * _moveForce * Time.deltaTime, ForceMode2D.Force);

        // Eject Aming
        _mouseDirection = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        _mouseDirection.Normalize();
        _ejectParticleSystem.rotation = Quaternion.FromToRotation(Vector3.right, _mouseDirection);

        // Ejection
        if (_ejectInput)
        {
            TryEject(_mouseDirection);
        }
        else _ejectEmission.enabled = false;

        // Animation
        if (_ejectEmission.enabled) _animator.SetFaceAnimation(PlayerFaceState.Shooting);
        else if (movement.magnitude > 0.1f && IsOnFloor()) _animator.SetFaceAnimation(PlayerFaceState.Walking);
        else if (!IsOnFloor()) _animator.SetFaceAnimation(PlayerFaceState.Holding);
        else _animator.SetFaceAnimation(PlayerFaceState.Idle);


    }

    private void TryJump()
    {
        /*if (IsOnFloor())
        {
            _rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }*/
    }

    private bool IsOnFloor()
    {
        return _collider.IsTouchingLayers(LayerMask.GetMask("Floor"));
    }

    private void TryEject(Vector2 direction)
    {
        if (FluidReserve >= 25f)
        {
            FluidReserve -= 10f * Time.deltaTime;
            _rb.AddForce(-direction * _ejectForce * Time.deltaTime, ForceMode2D.Force);
            SetSize(FluidReserve / 100f);
            _ejectEmission.enabled = true;
        }
        else
        {
            InGameUI.Instance.TriggerGameOver();
        }      
    }

    private void SetSize(float relativeSize)
    {
        int sizeLevel = GetSizeLevel(relativeSize);
        float rest = ( 1f + (relativeSize - (sizeLevel + 1) *0.25f) * 4f) *0.5f;

        _rb.mass = _initialMass * relativeSize;
        _collider.radius = _radiusLevels[sizeLevel];
        _animator.SetBodyAnimation(sizeLevel, rest);
    }

    private int GetSizeLevel(float relativeSize)
    {
        if (relativeSize > 0.75f) return 2;
        else if (relativeSize > 0.5f) return 1;
        else return 0;
    }

    public void RefillFluid(float amount)
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.PlayerAbsorb, true);
        FluidReserve = Mathf.Clamp(FluidReserve + amount, 0f, 100f);
        SetSize(FluidReserve / 100f);
    }


    public void Die(bool explode = true)
    {
        _isDead = true;
        _ejectEmission.enabled = false;
        if (explode)
        {
            _deathEmission.rateOverTime = FluidReserve * 100f;
            _deathParticleSystem.Play();
        }
        _rb.bodyType = RigidbodyType2D.Static;
        _animator.Die();
    }
}
