using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
    [SerializeField] UnityEvent<Vector2> _onPointerPosition;
    [SerializeField] WeaponParent weaponParent;

    AgentAnimations agentAnimations;

    public bool IsMoving => _isMoving;

    public float MoveSpeed 
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
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
    }

    private void OnEnable()
    {
        _onSecondaryAttack.AddListener(HandleDash);
    }

    private void OnDisable()
    {
        _onSecondaryAttack.RemoveListener(HandleDash);
    }

    void Update()
    {
        Cursor.visible = true;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0) && !IsMouseOverUi)
        {
            _onPrimaryAttack?.Invoke(); 
        }
        if (Input.GetMouseButtonDown(1) && !IsMouseOverUi)
        {
            _onSecondaryAttack?.Invoke();
        }

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

    [SerializeField] float startDashTime = 1f;
    [SerializeField] float dashSpeed = 1f;
    [SerializeField] float dashCoolDown = 1f;

    float currentDashTime;
    float currentCooldownTime;

    bool canMove = true;
    bool canDash = true;
    bool playerCollision = true;

    IEnumerator Dash(Vector2 direction)
    {
        canMove = false;
        canDash = false;
        playerCollision = false;
        currentDashTime = startDashTime; // Reset the dash timer.

        while (currentDashTime > 0f)
        {
            currentDashTime -= Time.deltaTime; // Lower the dash timer each frame.

            _rb.velocity = direction * dashSpeed; // Dash in the direction that was held down.
                                                            // No need to multiply by Time.DeltaTime here, physics are already consistent across different FPS.

            yield return null; // Returns out of the coroutine this frame so we don't hit an infinite loop.
        }

        _rb.velocity = new Vector2(0f, 0f); // Stop dashing.

        currentCooldownTime = dashCoolDown;

        while(currentCooldownTime > 0f)
        {
            currentCooldownTime -= Time.deltaTime;

            yield return null;
        }

        canMove = true;
        canDash = true;
        playerCollision = true;
    }

    private void AnimateCharacter()
    {
        Vector2 lookDirection = (_mousePosition - (Vector2)transform.position);
        agentAnimations.RotateToPointer(lookDirection, weaponParent.characterRenderer);
    }
}
