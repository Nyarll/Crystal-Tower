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

    public void Mapping(int x, int y, TileType type)
    {
        mappingTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);

        if ((type == TileType.Room || type == TileType.Pass))
        {
            //mappingTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
        }
        /**
        if (map[x, y] == null && (type == TileType.Room || type == TileType.Pass))
        {
            map[x, y] = new Tile(type, new Position(x, y));
            GameObject obj = Instantiate(roadImage, roads.transform);
            obj.transform.position = 
                new Vector3(roads.transform.position.x + x, roads.transform.position.y + y, roads.transform.position.z);
        }
        /**/
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
