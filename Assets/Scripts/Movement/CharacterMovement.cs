using System.Collections;
using System.Collections.Generic;
using System.EnterpriseServices;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameController _gameController;

    [Header("Movement")]
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _turnSpeed = 0.05f;

    [Header("Lane Setting")]
    [SerializeField] private float _distance = 5.0f;
    [SerializeField] private int _direction = 1;                // 0 = left, 1 = middle, 2 = right

    [Header("Jump Setting")]
    [SerializeField] private float _gravity = 12.0f;
    [SerializeField] private float _jumpForce = 10.0f;

    [Header("Roll Setting")]
    [SerializeField] private float _rollingTime = 1.0f;
    [SerializeField] private float _targetCapsuleHeight = 0.5f;
    [SerializeField] private Vector3 _targetCapsuleCenter = new Vector3(0, 0.3f, 0);

    [Header("Grounding")]
    [SerializeField] private float _groundCheckRadius = 0.25f;
    [SerializeField] private float _groundCheckOffset = -0.5f;
    [SerializeField] private float _groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;

    [Header("SFX")]
    [SerializeField] private AudioSource _swipeSFX;
    [SerializeField] private AudioSource _jumpSFX;
    [SerializeField] private AudioSource _rollSFX;

    // private
    private float _verticalVelocity = -0.1f;
    private CharacterController _characterController;
    private Animator _animator;
    private float _capsuleHeight;
    private Vector3 _capsuleCenter;
    private Vector3 _groundNormal = Vector3.up;
    private bool _isRolling;
    private float _rollingElapsedTime;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _capsuleHeight = _characterController.height;
        _capsuleCenter = _characterController.center;

        _animator = GetComponent<Animator>();
    }

    private void SetMoveDirection(bool isRight)
    {
        _direction += isRight ? 1 : -1;
        _direction = Mathf.Clamp(_direction, 0, 2);
    }

    private void SetLookDirection()
    {
        // rotate the character when it's move to left or right
        Vector3 lookDirection = _characterController.velocity;

        if (lookDirection == Vector3.zero) return;

        lookDirection.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, lookDirection, _turnSpeed);
    }

    private void MoveTo()
    {
        // move forward
        Vector3 targetPosition = transform.position.z * Vector3.forward;

        // move side
        if (_direction == 0) targetPosition += Vector3.left * _distance;
        else if (_direction == 2) targetPosition += Vector3.right * _distance;

        // calculate the movement
        Vector3 movement = Vector3.zero;
        movement.x = (targetPosition - transform.position).normalized.x * _speed;
        movement.y = _verticalVelocity;
        movement.z = _speed;

        // Move the character
        _characterController.Move(movement * Time.deltaTime);
    }

    private void Jump()
    {
        if (CheckGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpSFX.Play();
                _verticalVelocity = _jumpForce;
            }
        }
        else
        {
            // set falling time
            _verticalVelocity -= (_gravity * Time.deltaTime);
        }
        _animator.SetBool("IsGrounded", CheckGrounded());
    }

    private bool CheckGrounded()
    {
        // find start position
        Vector3 start = transform.position + Vector3.up * _groundCheckOffset;

        // perform spherecast - start, radius, direction, hit info, distance, layermask
        if (Physics.SphereCast(start, _groundCheckRadius, Vector3.down, out RaycastHit hit, _groundCheckDistance, _groundMask))
        {
            _groundNormal = hit.normal;
            return true;
        }
        _groundNormal = Vector3.up;
        return false;
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!_isRolling && !CheckGrounded()) return;
            _rollSFX.Play();
            _rollingElapsedTime = 0f;
            _isRolling = true;
            _characterController.height = _targetCapsuleHeight;
            _characterController.center = _targetCapsuleCenter;
        }
        _rollingElapsedTime += Time.deltaTime;

        // reset all the value
        if (_rollingElapsedTime >= _rollingTime && _isRolling)
        {
            _characterController.height = _capsuleHeight;
            _characterController.center = _capsuleCenter;
            _rollingElapsedTime = 0f;
            _isRolling = false;
        }
        _animator.SetBool("IsRolling", _isRolling);
    }

    private void Update()
    {
        // set direction by MoveInput
        if (Input.GetKeyDown(KeyCode.A))
        {
            _swipeSFX.Play();
            SetMoveDirection(false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _swipeSFX.Play();
            SetMoveDirection(true);
        }

        if (_gameController.Timer <= 90f && _gameController.Timer >= 45f) _speed = 15f;
        if (_gameController.Timer <= 44f) _speed = 20f;

        MoveTo();
        SetLookDirection();
        Jump();
        Roll();
    }

    private void OnDrawGizmosSelected()
    {
        // set gizmos color
        Gizmos.color = Color.red;
        if (CheckGrounded()) Gizmos.color = Color.green;

        // find start / end position of spherecast
        Vector3 start = transform.position + Vector3.up * _groundCheckOffset;
        Vector3 end = start + Vector3.down * _groundCheckDistance;

        // draw wire spheres
        Gizmos.DrawWireSphere(start, _groundCheckRadius);
        Gizmos.DrawWireSphere(end, _groundCheckRadius);

    }
}
