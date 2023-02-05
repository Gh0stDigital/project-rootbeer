using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Termina : MonoBehaviour
{
    public Vector2 facingDirection = new(0, 1);
    public float moveSpeed = 5;
    public float turnTime = 0.3f;
    public float skidTime = 0.1f;
    public float skidMovementMultiplier = 0.3f;

    public Rect bounds;
    public Vector3 extents;

    public bool paused = false;
    
    private Vector2 _initialFacingDirection;
    private Vector2 _moveDirection;
    
    private float _turnTimer = 0;
    private float _skidTimer = 0;
    private Vector2 _targetDirection;

    private bool IsTurning => _turnTimer > 0;
    private bool IsSkidding => _skidTimer > 0;

    // Start is called before the first frame update
    private void Start()
    {
        _initialFacingDirection = facingDirection;
        _moveDirection = _initialFacingDirection;
    }

    // Update is called once per frame
    private void Update()
    {
        if (paused) return;
        
        var delta = Time.deltaTime;

        HandleInput(delta);
        UpdateMovement(delta);
        UpdateTurning(delta);
    }

    private void HandleInput(float delta)
    {
        var xDir = Input.GetAxis("Horizontal");
        var yDir = Input.GetAxis("Vertical");
        
        Turn(xDir, yDir);
    }

    private void UpdateMovement(float delta)
    {
        var offset = CalculateOffset2D(delta);
        var offset3d = new Vector3(offset.x, offset.y, 0);

        UpdatePositionWithBounds(transform.position + offset3d);
    }

    private Vector2 CalculateOffset2D(float delta) =>
        IsTurning
            ? IsSkidding
                ? moveSpeed * delta * skidMovementMultiplier * _moveDirection
                : moveSpeed * skidTime * skidMovementMultiplier * _moveDirection
            : moveSpeed * delta * _moveDirection;

    private void UpdatePositionWithBounds(Vector3 position)
    {
        var min = position - extents;
        var max = position + extents;
        
        if (min.x < bounds.xMin)
        {
            position.x = bounds.xMin + extents.x;
        }

        if (max.x > bounds.xMax)
        {
            position.x = bounds.xMax - extents.x;
        }

        if (min.y < bounds.yMin)
        {
            position.y = bounds.yMin + extents.y;
        }

        if (max.y > bounds.yMax)
        {
            position.y = bounds.yMax - extents.y;
        }

        transform.position = position;
    }

    private void UpdateTurning(float delta)
    {
        if (IsTurning)
        {
            _turnTimer -= delta;

            if (_turnTimer <= 0)
            {
                _turnTimer = 0;
                _skidTimer = 0;

                _moveDirection = _targetDirection;
            }
        }

        if (IsTurning && !IsSkidding)
        {
            _skidTimer = skidTime;
        }

        if (IsSkidding)
        {
            _skidTimer -= delta;

            if (_skidTimer <= 0)
            {
                _skidTimer = 0;
            }
        }
        
        UpdateAngle(_targetDirection);
    }

    private void Turn(float xDir, float yDir)
    {
        var direction = new Vector2();
        
        if (xDir != 0)
        {
            direction = new Vector2(xDir, 0).normalized;
        }
        else if (yDir != 0)
        {
            direction = new Vector2(0, yDir).normalized;
        }
        
        if (direction != Vector2.zero && direction != _targetDirection)
        {
            if (!IsTurning)
            {
                _turnTimer = turnTime;
                _skidTimer = skidTime;
            }

            _targetDirection = direction;
        }
    }

    private void UpdateAngle(Vector2 direction)
    {
        facingDirection = direction;
        var z = Vector2.SignedAngle(_initialFacingDirection, facingDirection);
        var t = (_turnTimer / turnTime);
        var offsetDir = -math.sign(Vector2.SignedAngle(facingDirection, _moveDirection));
        var zOffset = IsTurning? offsetDir * 30 * SmoothStep(t) : 0.0f;
        
        transform.eulerAngles = new Vector3(0, 0, z + zOffset);
    }
    
    private static float SmoothStep(float x)
    {
        return x * x * (2.0f * x - 3.0f);
    }
}
