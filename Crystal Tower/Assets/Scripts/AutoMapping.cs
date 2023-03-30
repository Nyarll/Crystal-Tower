using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMapping : MonoBehaviour
{
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
        RectTransform rect = roadImage.GetComponent<RectTransform>();
        pw = rect.sizeDelta.x;
        ph = rect.sizeDelta.y;
    }

    public void Mapping(int x, int y, TileType type)
    {
        Debug.Log("mapping");
        if (map[x, y] == null && (type == TileType.Room || type == TileType.Pass))
        {
            Debug.Log("mapping");
            map[x, y] = new Tile(type, new Position(x, y));
            GameObject obj = Instantiate(roadImage, roads.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * 10, y * 10);
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
        GetComponent<RectTransform>().sizeDelta = new Vector2(MapCreator.MapSizeX * pw, MapCreator.MapSizeY * ph);
    }

    private int ToMirrorX(int xgrid)
    {
        return MapCreator.MapSizeX - xgrid - 1;
    }
}
