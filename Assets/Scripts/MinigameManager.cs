using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    enum State
    {
        Idle,
        SpawningRoots,
        SpawnedRoots,
        SpawnedPlayer,
        Go,
        WaitForPan,
        Panning,
        Shake,
        WaitForDone,
        Done
    }

    public GameObject playerPrefab;
    public GameObject rootSpawnerPrefab;
    public GameObject mugSpawnerPrefab;
    public GameObject wellDonePrefab;
    public GameObject startCountdownPrefab;

    public int rootSpawnCount = 10;
    public int mugSpawnCount = 5;
    
    public new GameObject camera;
    public Vector3 cameraPositionPostPan;
    
    public Vector3 rootSpawnOrigin;
    public Vector3 playerSpawnPosition;

    public float idleWaitTime = 3.0f;
    public float spawnedRootsWaitTime = 2.0f;
    public float spawnedPlayerWaitTime = 3.0f;
    public float cameraWaitForPanTime = 1.0f;
    public float cameraPanTime = 2.0f;
    public float waitForDoneTime = 10.0f;

    private State _currentState = State.Idle;
    private float _timer = 0.0f;

    private GameObject _rootSpawner;
    private GameObject _player;
    private GameObject _mugSpawner;

    private Vector3 _cameraOrigin;


    public bool IsDone => _currentState == State.Done;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOrigin = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var delta = Time.deltaTime;
        
        if (_currentState == State.Idle)
        {
            if (_timer == 0)
            {
                transform.Find("TimeForRootBeer").GetComponent<AudioSource>().Play();
            }
            
            _timer += delta;

            if (_timer >= idleWaitTime)
            {
                _rootSpawner = Instantiate(rootSpawnerPrefab, rootSpawnOrigin, Quaternion.identity);
                _rootSpawner.GetComponent<RootSpawner>().spawnCount = rootSpawnCount;
                _rootSpawner.GetComponent<RootSpawner>().StartSpawning();
                _currentState = State.SpawningRoots;
            }
        }

        if (_currentState == State.SpawningRoots)
        {
            if (_rootSpawner.GetComponent<RootSpawner>().IsDoneSpawning)
            {
                _currentState = State.SpawnedRoots;
                _timer = 0.0f;
            }
        }

        if (_currentState == State.SpawnedRoots)
        {
            _timer += delta;

            if (_timer >= spawnedRootsWaitTime)
            {
                _player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
                _player.GetComponent<Termina>().paused = true;
                Instantiate(startCountdownPrefab, transform.position, Quaternion.identity);
                _currentState = State.SpawnedPlayer;
                _timer = 0.0f;
            }
        }

        if (_currentState == State.SpawnedPlayer)
        {
            _timer += delta;

            if (_timer >= spawnedPlayerWaitTime)
            {
                _player.GetComponent<Termina>().paused = false;
                _currentState = State.Go;
            }
        }

        if (_currentState == State.Go)
        {
            if (_rootSpawner.GetComponent<RootSpawner>().Roots.All(root => root == null))
            {
                _player.GetComponent<Termina>().paused = true;
                
                transform.Find("LetsGetReadyToShake").GetComponent<AudioSource>().Play();

                _currentState = State.WaitForPan;
                _timer = 0.0f;
            }
        }

        if (_currentState == State.WaitForPan)
        {
            _timer += delta;

            if (_timer >= cameraWaitForPanTime)
            {
                _currentState = State.Panning;
                _timer = 0.0f;

                foreach (var go in GameObject.FindGameObjectsWithTag("Root Explode"))
                {
                    Destroy(go);
                }
            }
        }
        
        if (_currentState == State.Panning)
        {
            _timer += delta;
            var t = Mathf.Min(SmoothStep(_timer / cameraPanTime), 1.0f);
            camera.transform.position = _cameraOrigin * (1 - t) + cameraPositionPostPan * t;

            if (_timer >= cameraPanTime)
            {
                _currentState = State.Shake;
                _mugSpawner = Instantiate(mugSpawnerPrefab, transform.position, Quaternion.identity);
                _mugSpawner.GetComponent<MugSpawner>().spawnCount = mugSpawnCount;
            }
        }

        if (_currentState == State.Shake)
        {
            if (_mugSpawner.GetComponent<MugSpawner>().CurrentState == MugSpawner.State.Done)
            {
                transform.Find("Background Music").GetComponent<AudioSource>().Stop();
                Instantiate(wellDonePrefab, transform.position, Quaternion.identity);
                _currentState = State.WaitForDone;
                _timer = 0.0f;
            }
        }

        if (_currentState == State.WaitForDone)
        {
            _timer += delta;

            if (_timer >= waitForDoneTime)
            {
                _currentState = State.Done;
            }
        }

        if (_currentState == State.Done)
        {
            SceneManager.LoadScene(sceneBuildIndex: 2);
            Debug.Log("Done!");
        }
    }
    
    private static float SmoothStep(float x)
    {
        return x * x * (3.0f - 2.0f * x);
    }
}
