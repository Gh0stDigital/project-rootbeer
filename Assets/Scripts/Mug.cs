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

        var xDir = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
            ? 1.0f
            : (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
                ? -1.0f 
                : 0.0f;
        var yDir  = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
            ? 1.0f 
            : (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) 
                ? -1.0f 
                : 0.0f;
        var dir = new Vector2(xDir, yDir);
        var offset = new Vector3(dir.x * shakeOffset, dir.y * shakeOffset);

        if (dir != _previousDir && offset != Vector3.zero && !IsDone && _shakeTimer <= 0)
        {
            _shakeTimer = shakeTime;
                
            transform.Find("RootBeerShake").GetComponent<AudioSource>().Play();
            
            _shakeCounter++;

            if (_shakeCounter == shakeCount)
            {
                IsDone = true;
                transform.position = _origin;
                _shakeTimer = 0;
                
                transform.Find("RootBeerDone").GetComponent<AudioSource>().Play();
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