using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MugSpawner : MonoBehaviour
{
    public enum State
    {
        Spawning,
        Shaking,
        Done
    }
    
    public GameObject mugPrefab;

    public int spawnCount = 5;
    public int mugsPerRow = 4;

    public Rect spawnArea;

    public float spawnInterval = 1.0f;

    public List<GameObject> Mugs
    {
        get;
    } = new();

    public Mug CurrentMug => Mugs[_currentMugIndex].GetComponent<Mug>();

    public State CurrentState
    {
        get;
        private set;
    } = State.Spawning;
    
    private float _timer;
    private int _spawnCounter;
    private Vector2 _gridSize;
    private int _currentMugIndex;
    

    public void Start()
    {
        _timer = spawnInterval;
        _gridSize = spawnArea.size / mugsPerRow;
    }

    public void Update()
    {
         var delta = Time.deltaTime;
         _timer -= delta;

         if (CurrentState == State.Spawning)
         {
             if (_timer <= 0)
             {
                 _timer = spawnInterval;
                 SpawnMug();
             }
         }

         if (CurrentState == State.Shaking)
         {
             CurrentMug.Shake(delta);

             if (CurrentMug.IsDone)
             {
                 _currentMugIndex++;

                 if (_currentMugIndex == spawnCount)
                 {
                     CurrentState = State.Done;
                 }
             }
         }
    }

    private void SpawnMug()
    {
        // ReSharper disable once PossibleLossOfFraction
        var gridPos = new Vector2(_spawnCounter % mugsPerRow, _spawnCounter / mugsPerRow);
        var offset = gridPos * _gridSize;
        var spawnPoint = new Vector3(spawnArea.xMin, spawnArea.yMax, transform.position.z)
                         + new Vector3(offset.x, -offset.y, 0);

        var mug = Instantiate(mugPrefab, spawnPoint, Quaternion.identity);
        Mugs.Add(mug);
        
        _spawnCounter++;
        if (_spawnCounter == spawnCount)
        {
            CurrentState = State.Shaking;
        }
    }
}