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
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] bool _showCenterOfMass;
    [SerializeField, ShowIf("_showCenterOfMass")] Vector3 _centerOfMass;
    [SerializeField, ReadOnly] bool _isMoving;
    [SerializeField] UnityEvent _onAttack;
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
    void Start()
    {
        _rb.centerOfMass = _centerOfMass;
    }
    
    void Update()
    {
        Cursor.visible = true;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0) && !IsMouseOverUi)
        {
            _onAttack?.Invoke(); 
        }

        _moveDirection = new Vector2(moveX, moveY).normalized;

        if (_moveDirection.magnitude > 0.01f) _isMoving = true;
        else _isMoving = false; 

    }

    void OnDrawGizmosSelected()
    {
        if (_showCenterOfMass) Gizmos.DrawSphere(_rb.centerOfMass, 0.2f);
    }

    private void FixedUpdate()
    {
        // _rb.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed );
        _rb.MovePosition(new Vector2((transform.position.x + _moveDirection.x * _moveSpeed * Time.deltaTime),
            transform.position.y + _moveDirection.y * _moveSpeed * Time.deltaTime));

        
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = _mousePosition - _rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        _rb.rotation = aimAngle;
        
       
    }
}
