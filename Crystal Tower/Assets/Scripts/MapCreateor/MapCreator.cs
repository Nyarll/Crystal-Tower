using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    // �}�b�v�T�C�Y
    [SerializeField]
    public static int MapSizeX = 64;
    
    // �}�b�v�T�C�Y
    [SerializeField]
    public static int MapSizeY = 64;

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

        GameObject walls = new GameObject("walls");
        walls.transform.parent = this.gameObject.transform;
        walls.transform.position = Vector3.zero;

        GameObject rooms = new GameObject("rooms");
        rooms.transform.parent = this.gameObject.transform;
        rooms.transform.position = Vector3.zero;

        GameObject routes = new GameObject("routes");
        routes.transform.parent = this.gameObject.transform;
        routes.transform.position = Vector3.zero;

        // ����
        for (int y = 0; y < MapSizeY; y++)
        {
            for (int x = 0; x < MapSizeX; x++)
            {
                if (this.mapData[y, x].GetType() != TileType.None)
                {
                    GameObject obj = Instantiate(tilePrefab, new Vector3(x, y, 1), new Quaternion());
                    SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();

                    switch (this.mapData[y, x].GetType())
                    {
                        case TileType.Wall:
                            obj.transform.parent = walls.transform;
                            obj.layer = LayerMask.NameToLayer("Wall");
                            obj.AddComponent<Wall>();
                            obj.tag = "Wall";
                            obj.name = "WallTile";
                            sprite.color = new Color32(64, 32, 0, 255);
                            break;

                        case TileType.Room:
                            obj.transform.parent = rooms.transform;
                            obj.tag = "Room";
                            obj.name = "RoomTile";
                            obj.AddComponent<RoomTile>();
                            sprite.color = new Color32(128, 255, 255, 255);
                            break;

                        case TileType.Pass:
                            obj.transform.parent = routes.transform;
                            obj.tag = "Pass";
                            obj.name = "PassTile";
                            sprite.color = new Color32(192, 192, 255, 255);
                            break;
                    }
                }
                CreateCircumscribedWall(x, y);
            }
        }

        List<Range> roomList = this.generator.GetRoomData();
        int count = 0;
        float padding = 0.5f;
        foreach (Range roomData in roomList)
        {
            GameObject room = new GameObject("room" + count);
            room.tag = "RoomMST";
            room.transform.parent = rooms.transform;
            room.transform.position = 
                new Vector3(roomData.End.X - (roomData.GetWidthX() / 2) + padding,
                roomData.End.Y - (roomData.GetWidthY() / 2) + padding);

            room.AddComponent<RoomMST>();

            count++;
        }
    }

    private void CreateCircumscribedWall(int x, int y)
    {
        if (this.mapData[0, x].GetType() != TileType.None)
        {
            CreateWall(x, -1);
        }
        if (this.mapData[y, 0].GetType() != TileType.None)
        {
            CreateWall(-1, y);
        }
        if (this.mapData[MapSizeY - 1, x].GetType() != TileType.None)
        {
            CreateWall(x, MapSizeY);
        }
        if (this.mapData[y, MapSizeX - 1].GetType() != TileType.None)
        {
            CreateWall(MapSizeX, y);
        }
    }

    private void CreateWall(int x, int y)
    {
        GameObject obj = Instantiate(tilePrefab, new Vector3(x, y, 1), new Quaternion());
        obj.transform.parent = transform.Find("walls");
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        obj.layer = LayerMask.NameToLayer("Wall");
        obj.AddComponent<Wall>();
        sprite.color = new Color32(64, 32, 0, 255);
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
        } while (this.mapData[spawn.Y, spawn.X].GetType() != TileType.Room);

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
        } while ((this.mapData[spawn.Y, spawn.X].GetType() != TileType.Room) || (spawn == this.playerSpawnPoint));

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
                // �G���m�X�|�[���|�C���g�̔��Ȃ�
                // �v���C���[�X�|�[���Ɣ�炸
                // �������ʘH�ɗN��
                if (!enemySpawnPointList.Contains(spawn) && 
                    (!spawn.Equals(this.playerSpawnPoint)) &&
                    ((this.mapData[spawn.Y, spawn.X].GetType() == TileType.Room) ||
                    (this.mapData[spawn.Y, spawn.X].GetType() == TileType.Pass)))
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
