using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using SoundSystem;

public class PlayerMovement : MonoBehaviour
{
    public static bool IsMouseOverUi
    {
        get
        {
            var events = EventSystem.current;
            return events != null && events.IsPointerOverGameObject();
        }
    }
    [SerializeField] PlayerStats _playerStats;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] bool _showCenterOfMass;
    [SerializeField, ShowIf("_showCenterOfMass")] Vector3 _centerOfMass;
    [SerializeField, ReadOnly] bool _isMoving;
    [SerializeField] WeaponParent weaponParent;

    [Header("Dash")]
    [SerializeField] bool dashUnlocked = true;
    [SerializeField] bool useMouseDirection = false;
    [SerializeField] ButtonCoolDownHandler dashButton;
    [SerializeField] float startDashTime = 1f;
    [SerializeField] float dashSpeed = 1f;
    [SerializeField] float dashCoolDown = 1f;

    [SerializeField] GameObject _dashParticles;
    [SerializeField] Transform startTransform;
    [SerializeField] SFXEvent _sfxDash;

    AgentAnimations agentAnimations;

    public bool IsMoving => _isMoving;

    public float MoveSpeed
    {
        get { return _playerStats.Speed; }
    }

    public bool CanMove { get { return canMove; } set { canMove = value; } }

    private Vector2 _moveDirection;
    private Vector2 _mousePosition;

    void OnValidate()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
    }

    void Awake()
    {
        CanMove = false;
        agentAnimations = GetComponent<AgentAnimations>();
        _rb.centerOfMass = _centerOfMass;

        //Here we set ability is locked or not
        if (dashUnlocked)
            DashUnlocked();
        else
            DashLocked();
    }

    void Update()
    {
        Cursor.visible = true;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector2(moveX, moveY).normalized;

        if (_moveDirection.magnitude > 0.01f) _isMoving = true;
        else _isMoving = false;

        dashButton.CurrentAbilityCooldown = currentDashCooldownTime;

        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        weaponParent.PointerPosition = _mousePosition;

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
        _rb.MovePosition(new Vector2((transform.position.x + _moveDirection.x * _playerStats.Speed * Time.deltaTime),
            transform.position.y + _moveDirection.y * _playerStats.Speed * Time.deltaTime));

        //Vector2 aimDirection = _mousePosition - _rb.position;
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //_rb.rotation = aimAngle;              
    }

    public void HandleDash()
    {
        if (!canDash)
            return;
        Debug.Log("Dash");
        if (ScreenShakeController.Instance) ScreenShakeController.Instance.StartShake(0.07f, 0.1f); 

        if (useMouseDirection) StartCoroutine(Dash((_mousePosition - (Vector2)transform.position).normalized));
        else StartCoroutine(Dash(_moveDirection));
    }

    private void DashLocked()
    {
        canDash = false;
    }

    public void DashUnlocked()
    {
        canDash = true;
        dashButton.SetTotalcooldown(dashCoolDown);
        currentDashCooldownTime = dashCoolDown;
    }

    float currentDashTime;
    public float currentDashCooldownTime { get; set; }

    bool canMove = true;
    bool canDash = true; //Start as true for testing
    bool isDashing = false;
    bool playerCollision = true;

    IEnumerator Dash(Vector2 direction)
    {
        canMove = false;
        canDash = false;
        playerCollision = false;
        currentDashTime = startDashTime; // Reset the dash timer.
        currentDashCooldownTime = 0;

        Utility.SpawnParticles(_dashParticles, this.gameObject, false);
        _sfxDash.Play();
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
    }

    private void AnimateCharacter()
    {
        Vector2 lookDirection = (_mousePosition - (Vector2)transform.position);
        agentAnimations.RotateToPointer(lookDirection, weaponParent.characterRenderer);
    }

    public Vector3 GetWeaponDirection()
    {
        return weaponParent.transform.right;
    }

    //private void OnDisable()
    //{
    //    FadeAnimationScript.OnFaded -= MovePlayerToLocation;    
    //}

    //public void FadeMovePlayerEvent()
    //{
    //    FadeAnimationScript.OnFaded += MovePlayerToLocation;
    //    MovePlayerToLocation();
    //}

    public void MovePlayerToStart()
    {
        StartCoroutine(MovePlayerCoroutine());
    }

    IEnumerator MovePlayerCoroutine()
    {
        Debug.Log("Moving player to start");
        canMove = false;
        float currentPlayerMove = 0;
        while(currentPlayerMove< 0.25f)
        {
            currentPlayerMove += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, startTransform.position, currentPlayerMove / 0.25f);
            yield return null;
        }
        canMove = true;
    }
}
