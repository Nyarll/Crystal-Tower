using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    // �}�b�v�T�C�Y
    [SerializeField]
    int MapSizeX = 64;
    
    // �}�b�v�T�C�Y
    [SerializeField]
    int MapSizeY = 64;

    // �ő啔����
    [SerializeField]
    int MaxRoom = 10;

    // �v���C���[
    [SerializeField]
    GameObject playerObject;

    // �^�C��
    [SerializeField]
    GameObject tilePrefab;

    // �K�i
    [SerializeField]
    GameObject nextFloorObject;

    private Tile[,] mapData;

    private Position playerPosition;
    private Position nextFloorPosition;

    private MapGenerator generator = null;

    public Tile[,] GetMapData()
    {
        return this.mapData;
    }

    // Start is called before the first frame update
    void Start()
    {
        Generate();   
    }


    public void Generate()
    {
        if (this.generator is null)
        {
            this.generator = new MapGenerator();
        }

        this.MapDelete();
        this.GenerateMap();
        this.Spawn();
    }

    /**
     * �e�I�u�W�F�N�g�X�|�[��
     */
    private void Spawn()
    {
        this.SpawnPlayer();
        this.SpawnNextFloor();
        this.SpawnEnemies();
        this.SpawnItems();
    }

    private void GenerateMap()
    {
        this.mapData = this.generator.GenerateMap(MapSizeX, MapSizeY, MaxRoom);

        List<Vector3> floorList = new List<Vector3>();

        for (int y = 0; y < this.MapSizeY; y++)
        {
            for (int x = 0; x < this.MapSizeX; x++)
            {
                if (this.mapData[x, y].GetType() != TileType.None)
                {
                    GameObject obj = Instantiate(tilePrefab, new Vector3(x, y, 1), new Quaternion());
                    obj.transform.parent = this.transform;
                    SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();

                    switch (this.mapData[x, y].GetType())
                    {
                        case TileType.Wall:
                            obj.layer = LayerMask.NameToLayer("Wall");
                            obj.AddComponent<Wall>();
                            sprite.color = new Color32(64, 32, 0, 255);
                            break;

                        case TileType.Room:
                            sprite.color = new Color32(128, 255, 255, 255);
                            break;

                        case TileType.Pass:
                            sprite.color = new Color32(192, 192, 255, 255);
                            break;

                        default:
                            sprite.color = new Color32(255, 0, 255, 255);
                            break;
                    }
                }
            }
        }
    }

    private void MapDelete()
    {
        Debug.Log("delete");
        foreach (Transform obj in gameObject.transform)
        {
            GameObject.Destroy(obj.gameObject);
        }
        generator.DeleteMap();
    }

    /**
     * �v���C���[�X�|�[��
     */
    private void SpawnPlayer()
    {
        if (this.playerObject is null)
        {
            Debug.Log("player object is null.");
            return;
        }

        Position spawn;
        do
        {
            spawn = new Position(RogueUtils.GetRandomInt(0, MapSizeX - 1), RogueUtils.GetRandomInt(0, MapSizeY - 1));
        } while (this.mapData[spawn.X, spawn.Y].GetType() != TileType.Room);

        this.playerObject.transform.position = new Vector3(spawn.X, spawn.Y, 0);
        this.playerPosition = spawn;
        this.playerObject.GetComponent<Player>().SetMapData(mapData);
    }

    /**
     * ���̊K�w�ւ̊K�i�X�|�[��
     */
    private void SpawnNextFloor()
    {
        if (this.nextFloorObject is null)
        {
            Debug.Log("next floor object is null.");
            return;
        }

        Position spawn;
        do
        {
            spawn = new Position(RogueUtils.GetRandomInt(0, MapSizeX - 1), RogueUtils.GetRandomInt(0, MapSizeY - 1));
        } while ((this.mapData[spawn.X, spawn.Y].GetType() != TileType.Room) || (spawn == this.playerPosition));

        this.nextFloorObject.transform.position = new Vector3(spawn.X, spawn.Y, 0);
        this.nextFloorPosition = spawn;
        Debug.Log("Spawn : (X: " + spawn.X + ", Y:" + spawn.Y + "), Type:" + this.mapData[spawn.X, spawn.Y].GetType());

    }

    /**
     * �G�X�|�[��
     */
    private void SpawnEnemies()
    {

    }

    /**
     * �A�C�e���X�|�[��
     */
    private void SpawnItems()
    {

    }
}
