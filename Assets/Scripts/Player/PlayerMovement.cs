using System;
using Combat;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour //err?
{
    [Header("PlayerMovement")]
    [SerializeField] private float _speed = 5f;
    
    [Header("Jump")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private int _jumpCount = 2;
    
    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundMask;
    
    private Rigidbody2D _rb;
    private Weapon _weapon;
    
    private Vector2 _movement;

    private bool _jumpRequested;
    private int _jumpRemaining;

    private bool _isGrounded;
    
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _weapon = GetComponent<Weapon>();
        _jumpRemaining = _jumpCount;
    }

    private void OnEnable()
    {
        InputReader.OnFire += HandleFire;
        InputReader.OnJump += Jump;
    }
    
    private void OnDisable()
    {
        InputReader.OnFire -= HandleFire;
        InputReader.OnJump -= Jump;
    }

    private void FixedUpdate()
    {
        Move();
        UpdateGrounded();
    }

    private void Move()
    {
        Vector2 direction = InputReader.MoveDirection;
        if(direction.sqrMagnitude > 1)
            direction.Normalize();
        
        Vector2 velocity = _rb.linearVelocity;
        velocity.x = direction.x * _speed;
        
        _rb.linearVelocity = velocity;

        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x);
            transform.localScale = scale;
        }
        _animator.SetFloat("Movement", Mathf.Abs(velocity.x)); // так всегда будет положительным
    }
    
    private void Jump()
    {
        if(_jumpRemaining <= 0)
            return;
        
        Vector2 velocity = _rb.linearVelocity;
        velocity.y = _jumpForce;
        _rb.linearVelocity = velocity;
        _jumpRemaining--;
        
        //_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce); 
        //_jumpRemaining--;
        
        /*Vector2 velocity = _rb.linearVelocity;
        velocity.y = _jumpForce;// с каким-то ускорением
        _rb.linearVelocity = velocity;
        _jumpRemaining--;*/
    }

    private void UpdateGrounded()
    {
        bool isGrounded = _isGrounded;
        if (isGrounded && !_isGrounded)
            _jumpRemaining = _jumpCount;
        
        _isGrounded = isGrounded;
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            _groundCheck.position, 
            _groundCheckRadius, 
            _groundMask);
        
        /*RaycastHit2D hit = Physics2D.Raycast(
            _groundCheck.position,
            Vector2.down,
            _groundCheckRadius,
            _groundMask);
        return hit.collider;*/
    }

    private void OnDrawGizmosSelected()// не входит в билд
    {
        if (_groundCheck ==  null)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position,  _groundCheckRadius);
        // _groundCheck.position, 
        //_groundCheck.position + Vector3.down * _groundCheckRadius);
        //DrawLine < 
    }
    
    private void HandleFire()
    {
        _weapon.TryFire();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {// при приземлении на землю проверяется кол-во доступных прыжков
        //if(collision.gameObject.CompareTag("Ground"))
        if (((1 << collision.gameObject.layer) & _groundMask) != 0)
            _jumpRemaining = _jumpCount;
    }
    
}
