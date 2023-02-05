using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSpawner : MonoBehaviour
{
    public GameObject root;
    public float spawnInterval = 0.3f;
    public int spawnCount = 10;

    public Vector2 spawnSize = new(10, 20);
    
    private bool _isSpawning = false;

    private float _spawnTimer;
    
    private List<Vector3> _targetOffsets = new();
    public List<GameObject> Roots { get; } = new();

    private List<float> _interpolations = new();

    private bool ShouldSpawn => _isSpawning && _spawnTimer <= 0.0f;

    public bool IsDoneSpawning { get; private set; } = false;

    public void StartSpawning()
    {
        _isSpawning = true;
        IsDoneSpawning = false;
    }

    public void StopSpawning()
    {
        _isSpawning = false;
        IsDoneSpawning = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var delta = Time.deltaTime;

        if (_isSpawning)
        {
            _spawnTimer -= delta;
        }

        if (ShouldSpawn)
        {
            _spawnTimer = spawnInterval;
            var r = Instantiate(root, transform.position, Quaternion.identity);

            Roots.Add(r);
            _targetOffsets.Add(new Vector3(
                Random.Range(0, spawnSize.x),
                -Random.Range(0, spawnSize.y),
                0));
            _interpolations.Add(0.0f);
            
            transform.Find("RootEnter").GetComponent<AudioSource>().Play();

            if (Roots.Count == spawnCount)
            {
                StopSpawning();
            }
        }

        for (var i = 0; i < Roots.Count; i++)
        {
            if (Roots[i] != null)
            {
                _interpolations[i] = Mathf.Min(_interpolations[i] + delta, 1.0f);
                Roots[i].transform.position = transform.position + _targetOffsets[i] * EaseOut(_interpolations[i]);
            }
        }
    }
    
    private static float EaseOut(float x)
    {
        return x >= 1.0 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }
}
