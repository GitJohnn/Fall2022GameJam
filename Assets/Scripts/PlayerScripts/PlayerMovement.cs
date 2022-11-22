using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
    [SerializeField] ButtonCoolDownHandler dashButton;
    [SerializeField] float startDashTime = 1f;
    [SerializeField] float dashSpeed = 1f;
    [SerializeField] float dashCoolDown = 1f;

    AgentAnimations agentAnimations;

    public bool IsMoving => _isMoving;

    public float MoveSpeed 
    {
        get { return _playerStats.Speed; }
    }

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
        StartCoroutine(Dash((_mousePosition - (Vector2)transform.position).normalized));
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
    float currentDashCooldownTime;

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
}
