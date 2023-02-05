using System;
using UnityEngine;

public class Mug : MonoBehaviour
{
    public float shakeOffset = 8.0f / 48.0f;
    public int shakeCount = 10;
    public float shakeTime = 0.2f;

    public bool IsDone
    {
        get;
        private set;
    } = false;

    private Vector3 _origin;
    private int _shakeCounter;
    private float _shakeTimer;

    private Vector2 _previousDir;
    
    public void Start()
    {
        _origin = transform.position;
    }

    public void Shake(float delta)
    {
        transform.position = _origin;
        
        var xDir = Input.GetAxis("Horizontal");
        var yDir = Input.GetAxis("Vertical");
        var dir = new Vector2(xDir, yDir);
        var offset = new Vector3(xDir * shakeOffset, yDir * shakeOffset);

        if (dir != _previousDir && offset != Vector3.zero && !IsDone && _shakeTimer <= 0)
        {
            _shakeTimer = shakeTime;
            
            _shakeCounter++;

            if (_shakeCounter == shakeCount)
            {
                IsDone = true;
                transform.position = _origin;
                _shakeTimer = 0;
            }
        }

        if (_shakeTimer > 0)
        {
            _shakeTimer -= delta;
            
            transform.position = _origin + offset;
        }

        _previousDir = dir;
    }
}