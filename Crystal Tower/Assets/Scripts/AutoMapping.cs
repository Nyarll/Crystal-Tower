using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class AutoMapping : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject miniPlayer;

    [SerializeField]
    private Tilemap mappingTilemap;

    [SerializeField]
    private TileBase floorTile;


    private float pw, ph;
    private Tile[,] map;

    public void Mapping(int x, int y)
    {
        mappingTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
    }

    public void InRoomMapping(GameObject roomObject)
    {
        Tilemap roomTilemap = roomObject.GetComponent<Tilemap>();
        foreach (var position in roomTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int cellPosition = new Vector3Int(position.x, position.y, position.z);
            if (roomTilemap.HasTile(cellPosition))
            {
                mappingTilemap.SetTile(cellPosition, floorTile);
            }
        }
    }

    public void ResetMap()
    {
        mappingTilemap.ClearAllTiles();
    }

    private void Update()
    {
        var shift_position = mappingTilemap.gameObject.transform.parent.transform.position;
        Vector3 position = new Vector3(shift_position.x + player.transform.position.x,
            shift_position.y + player.transform.position.y,
            miniPlayer.transform.position.z);
        miniPlayer.transform.position = position;
    }
}
