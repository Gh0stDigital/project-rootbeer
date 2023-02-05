using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePallet_Behavior : MonoBehaviour
{
    private Tilemap _tilemap;
    private PlayerController _player;

    [SerializeField]
    private ParticleSystem _digParticleSystem;

        //need to store collision pos OR store players cur movePoint as the post to pass into our custom tile map delete function
    // Start is called before the first frame update
    void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        _player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void DigTile(Vector3Int tilePos)
    {
        //Spawns Dig Effect      
        SpawnEffect(_tilemap.CellToWorld(tilePos));
        _tilemap.SetTile(tilePos, null);
    }

    private void SpawnEffect(Vector3 spawnPos)
    {
        Vector3 playerGridOffset = _player.transform.parent.localPosition;

        _digParticleSystem.startColor = _tilemap.color;
        Instantiate(_digParticleSystem, (spawnPos + playerGridOffset), Quaternion.identity);//Quaternion.LookRotation(newDirection)
    }
}
