using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using SoundSystem;
public class PlayerInput : MonoBehaviour
{
    public static bool IsMouseOverUi
    {
        get
        {
            var events = EventSystem.current;
            return events != null && events.IsPointerOverGameObject();
        }
    }
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] bool _showCenterOfMass;
    [SerializeField, ShowIf("_showCenterOfMass")] Vector3 _centerOfMass;
    [SerializeField, ReadOnly] bool _isMoving;
    [SerializeField] UnityEvent _onPrimaryAttack;
    [SerializeField] UnityEvent _onSecondaryAttack;
    [SerializeField] UnityEvent _onThirdAbility;
    [SerializeField] KeyCode thirdAbilityKey;
    [SerializeField] UnityEvent _onFourthAbility;
    [SerializeField] KeyCode fourthAbilityKey;
    [SerializeField] UnityEvent<Vector2> _onPointerPosition;
    [SerializeField] UnityEvent<float> _ondashcooldown;
    [SerializeField] WeaponParent weaponParent;

    AgentAnimations agentAnimations;

    public bool IsMoving => _isMoving;

    public float MoveSpeed 
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    [Header("Dash")]
    [SerializeField] bool dashUnlocked = true;
    [SerializeField] ButtonCoolDownHandler dashButton;
    [SerializeField] float startDashTime = 1f;
    [SerializeField] float dashSpeed = 1f;
    [SerializeField] float dashCoolDown = 1f;

    [Header("RangeAttack")]
    [SerializeField] bool rangeUnlocked = true;
    [SerializeField] ButtonCoolDownHandler rangeButton;
    [SerializeField] float totalRangeCooldown;
    [SerializeField] bool rangeCooldown;    

    public float CurrentDashCoolDown { get { return currentDashCooldownTime; } }
    public float CurrentRangeCoolDown { get { return currentRangeCooldown; } }

    [SerializeField] SFXEvent _sfxBombPlace;
    [SerializeField] SFXEvent _sfxSwingStaff;
    [SerializeField] SFXEvent _sfxBowShoot;

    private Vector2 _moveDirection;
    private Vector2 _mousePosition;

    void OnValidate()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();    
    }

    void Awake()
    {
        agentAnimations = GetComponent<AgentAnimations>();
        _rb.centerOfMass = _centerOfMass;
        //Set all abilities as locked
        //DashLocked()
        //Call unlock abilities when unlocked when opening a chest
        //DashUnlocked()
        if (dashUnlocked)
            DashUnlocked();
        else
            DashLocked();

        if (rangeUnlocked)
            RangeUnlocked();
        else
            RangeLocked();
    }

    private void OnEnable()
    {
        _onThirdAbility.AddListener(HandleDash);
    }

    private void OnDisable()
    {
        _onThirdAbility.RemoveListener(HandleDash);
    }

    void Update()
    {
        Cursor.visible = true;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        //Neutral attack
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUi)
        {
            _sfxSwingStaff.Play();
            _onPrimaryAttack?.Invoke(); 
        }
        //Right mouse click is range attack
        if (Input.GetMouseButtonDown(1) && !IsMouseOverUi && canRangeAttack)
        {
            _sfxBowShoot.Play();
            _onSecondaryAttack?.Invoke();
            StartCoroutine(Range());
        }
        //Dash attack
        if (Input.GetKeyDown(thirdAbilityKey) && canDash)
        {
            _onThirdAbility?.Invoke();
        }
        //Bomb placing
        if (Input.GetKeyDown(fourthAbilityKey))
        {
            _sfxBombPlace.Play();
            _onFourthAbility?.Invoke();
        }

        if (isDashing)
            _ondashcooldown?.Invoke(currentDashCooldownTime);

        _moveDirection = new Vector2(moveX, moveY).normalized;

        if (_moveDirection.magnitude > 0.01f) _isMoving = true;
        else _isMoving = false;

        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _onPointerPosition?.Invoke(_mousePosition);
        //weaponParent.PointerPosition = _mousePosition;

        AnimateCharacter();
    }

    void OnDrawGizmosSelected()
    {
        if (_showCenterOfMass) Gizmos.DrawSphere(_rb.centerOfMass, 0.2f);
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;
        // _rb.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed );
        _rb.MovePosition(new Vector2((transform.position.x + _moveDirection.x * _moveSpeed * Time.deltaTime),
            transform.position.y + _moveDirection.y * _moveSpeed * Time.deltaTime));
                
        //Vector2 aimDirection = _mousePosition - _rb.position;
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //_rb.rotation = aimAngle;              
    }

    private void HandleDash()
    {
        Debug.Log("Dash");
        StartCoroutine(Dash((_mousePosition - (Vector2)transform.position).normalized));
    }

    float currentDashTime;
    float currentDashCooldownTime;

    bool canMove = true;
    bool canDash = true; //Start as true for testing
    bool isDashing = false;
    bool playerCollision = true;

    private void DashLocked()
    {
        canDash = false;
    }

    public void DashUnlocked()
    {
        canDash = true;
        dashButton.SetTotalcooldown(dashCoolDown);
    }

    IEnumerator Dash(Vector2 direction)
    {
        canMove = false;
        canDash = false;
        isDashing = true;
        playerCollision = false;
        currentDashTime = startDashTime; // Reset the dash timer.
        currentDashCooldownTime = 0;

        while (currentDashTime > 0f)
        {
            currentDashTime -= Time.deltaTime; // Lower the dash timer each frame.

            _rb.velocity = direction * dashSpeed; // Dash in the direction that was held down.
                                                            // No need to multiply by Time.DeltaTime here, physics are already consistent across different FPS.
            yield return null; // Returns out of the coroutine this frame so we don't hit an infinite loop.
        }

        _rb.velocity = new Vector2(0f, 0f); // Stop dashing.

        canMove = true;        
        playerCollision = true;

        while (currentDashCooldownTime <= dashCoolDown)
        {
            currentDashCooldownTime += Time.deltaTime;

            yield return null;
        }
        
        canDash = true;
        isDashing = false;
    }

    private float currentRangeCooldown;
    private bool canRangeAttack;

    public void RangeUnlocked()
    {
        canRangeAttack = true;
    }

    private void RangeLocked()
    {
        canRangeAttack = false;
    }

    IEnumerator Range()
    {
        rangeCooldown = true;
        currentRangeCooldown = 0;
        while(currentRangeCooldown <= totalRangeCooldown)
        {
            currentRangeCooldown += Time.deltaTime;
            yield return null;
        }
        rangeCooldown = false;
    }

    private void AnimateCharacter()
    {
        Vector2 lookDirection = (_mousePosition - (Vector2)transform.position);
        agentAnimations.RotateToPointer(lookDirection, weaponParent.characterRenderer);
    }
}
