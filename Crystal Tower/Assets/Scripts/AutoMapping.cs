using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AutoMapping : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject miniPlayer;

    [SerializeField]
    private GameObject roads;

    [SerializeField]
    private GameObject enemies;

    [SerializeField]
    private GameObject items;

    [SerializeField]
    private GameObject roadImage;

    private float pw, ph;
    private Tile[,] map;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Mapping(int x, int y, TileType type)
    {
        if (map[x, y] == null && (type == TileType.Room || type == TileType.Pass))
        {
            map[x, y] = new Tile(type, new Position(x, y));
            GameObject obj = Instantiate(roadImage, roads.transform);
            obj.transform.position = 
                new Vector3(roads.transform.position.x + x, roads.transform.position.y + y, roads.transform.position.z);
        }
    }

    public void ResetMap()
    {
        for (int i = 0; i < roads.transform.childCount; i++)
        {
            Destroy(roads.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            Destroy(enemies.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < items.transform.childCount; i++)
        {
            Destroy(items.transform.GetChild(i).gameObject);
        }
        map = new Tile[MapCreator.MapSizeX, MapCreator.MapSizeY];
        //GetComponent<RectTransform>().sizeDelta = new Vector2(MapCreator.MapSizeX * pw, MapCreator.MapSizeY * ph);
    }

    private int ToMirrorX(int xgrid)
    {
        return MapCreator.MapSizeX - xgrid - 1;
    }

    private void Update()
    {
        Vector3 position = new Vector3(this.transform.position.x + player.transform.position.x,
            this.transform.position.y + player.transform.position.y,
            miniPlayer.transform.position.z);
        miniPlayer.transform.position = position;
    }

    /// <summary>
    /// •”‰®‚É“ü‚Á‚½‚Æ‚«‚ÉŒÄ‚Ô
    /// </summary>
    /// <param name="roomMst"></param>
    public void InRoom(GameObject roomMst)
    {
        try
        {
            var mst = roomMst.GetComponent<RoomMST>();
            if (!mst.isEnter())
            {
                mst.Enter();
                foreach (Transform child in roomMst.transform)
                {
                    GameObject obj = child.gameObject;
                    this.Mapping((int)obj.transform.position.x, (int)obj.transform.position.y, TileType.Room);
                }
            }
        }
        catch (Exception e)
        {

        }
    }
}
