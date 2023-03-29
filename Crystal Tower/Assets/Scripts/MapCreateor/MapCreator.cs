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

    // �^�C��
    [SerializeField]
    GameObject tilePrefab;

    [SerializeField]
    int enemyNumMin = 5;

    [SerializeField]
    int enemyNumMax = 10;

    private Tile[,] mapData;

    private Position playerSpawnPoint;
    private Vector3 playerSpawn;

    private Position nextFloorSpawnPoint;
    private Vector3 nextFloorSpawn;

    private List<Position> enemySpawnPointList;
    private List<Vector3> enemySpawnList;

    private MapGenerator generator = null;

    public Tile[,] GetMapData()
    {
        return this.mapData;
    }

    public void Generate()
    {
        if (this.generator == null)
        {
            this.generator = new MapGenerator(MapSizeX, MapSizeY, MaxRoom);
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
        this.generator.Generate();
        this.mapData = this.generator.GetMapData();

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
        foreach (Transform obj in gameObject.transform)
        {
            GameObject.Destroy(obj.gameObject);
        }
    }

    /**
     * �v���C���[�X�|�[��
     */
    private void SpawnPlayer()
    {
        Position spawn;
        do
        {
            spawn = new Position(RogueUtils.GetRandomInt(0, MapSizeX - 1), RogueUtils.GetRandomInt(0, MapSizeY - 1));
        } while (this.mapData[spawn.X, spawn.Y].GetType() != TileType.Room);

        this.playerSpawnPoint = spawn;
        this.playerSpawn = new Vector3(spawn.X, spawn.Y, 0);
    }

    /// <summary>
    /// �v���C���[�X�|�[���|�C���g���擾����
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerSpawnPoint()
    {
        return this.playerSpawn;
    }

    /**
     * ���̊K�w�ւ̊K�i�X�|�[��
     */
    private void SpawnNextFloor()
    {
        Position spawn;
        do
        {
            spawn = new Position(RogueUtils.GetRandomInt(0, MapSizeX - 1), RogueUtils.GetRandomInt(0, MapSizeY - 1));
        } while ((this.mapData[spawn.X, spawn.Y].GetType() != TileType.Room) || (spawn == this.playerSpawnPoint));

        this.nextFloorSpawnPoint = spawn;
        this.nextFloorSpawn = new Vector3(spawn.X, spawn.Y, 0);
    }

    /// <summary>
    /// �K�i�̃X�|�[���|�C���g���擾����
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextFloorSpawnPoint()
    {
        return this.nextFloorSpawn;
    }

    /**
     * �G�X�|�[��
     */
    private void SpawnEnemies()
    {
        if (enemySpawnList == null)
        {
            enemySpawnList = new List<Vector3>();
        }
        if(enemySpawnPointList == null)
        {
            enemySpawnPointList = new List<Position>();
        }
        enemySpawnList.Clear();
        enemySpawnPointList.Clear();
        // �X�|�[��������Enemy�̐�
        int num = RogueUtils.GetRandomInt(enemyNumMin, enemyNumMax);
        for (int i = 0; i < num; i++)
        {
            Position spawn;
            while (true)
            {
                spawn = new Position(RogueUtils.GetRandomInt(0, MapSizeX - 1), RogueUtils.GetRandomInt(0, MapSizeY - 1));
                if (!enemySpawnPointList.Contains(spawn) && 
                    (spawn != this.playerSpawnPoint) &&
                    ((this.mapData[spawn.X, spawn.Y].GetType() == TileType.Room) ||
                    (this.mapData[spawn.X, spawn.Y].GetType() == TileType.Pass)))
                {
                    enemySpawnPointList.Add(spawn);
                    enemySpawnList.Add(new Vector3(spawn.X, spawn.Y, 0));
                    break;
                }
            }

        }
    }

    /// <summary>
    /// �G�X�|�[���|�C���g�̃��X�g���擾
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetEnemySpawnPointList()
    {
        return enemySpawnList;
    }

    /**
     * �A�C�e���X�|�[��
     */
    private void SpawnItems()
    {

    }
}
