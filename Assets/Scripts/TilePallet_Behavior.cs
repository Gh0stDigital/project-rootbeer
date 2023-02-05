using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePallet_Behavior : MonoBehaviour
{
    private Tilemap _tilemap;
    private Grid _grid;
    private PlayerController _player;

        //need to store collision pos OR store players cur movePoint as the post to pass into our custom tile map delete function
    // Start is called before the first frame update
    void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        _grid = _tilemap.layoutGrid;
        _player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_player.obsticlePresent)
        {         
            DigTile(Vector3Int.RoundToInt(_player.movePoint.localPosition));
        }
        
    }

    private void DigTile(Vector3Int tilePos)
    {
        _tilemap.SetTile(tilePos, null);
    }
}
