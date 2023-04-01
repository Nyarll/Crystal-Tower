using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator : MonoBehaviour
{
    // マップサイズ
    [SerializeField]
    public static int MapSizeX = 64;

    // マップサイズ
    [SerializeField]
    public static int MapSizeY = 64;

    // 最大部屋数
    [SerializeField]
    int MaxRoom = 10;

    // 壁のタイルマップ
    [SerializeField]
    Tilemap tilemap_walls;

    // 床のタイルマップ
    [SerializeField]
    Tilemap floor_tilemap;

    // 壁のタイル
    [SerializeField]
    UnityEngine.Tilemaps.Tile wall;

    // 部屋のタイル
    [SerializeField]
    UnityEngine.Tilemaps.Tile room;

    // 通路のタイル
    [SerializeField]
    UnityEngine.Tilemaps.Tile pass;

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
     * 各オブジェクトスポーン
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

        /**/
        List<Range> roomList = this.generator.GetRoomData();

        for (int i = 0; i < roomList.Count; i++)
        {
            GameObject roomObject = new GameObject("room" + i);
            roomObject.transform.parent = tilemap_walls.transform.parent;
            roomObject.AddComponent<UnityEngine.Tilemaps.Tilemap>();
            roomObject.AddComponent<UnityEngine.Tilemaps.TilemapRenderer>();
            roomObject.AddComponent<UnityEngine.Tilemaps.TilemapCollider2D>();
        }
        /**/

        // 生成
        for (int y = 0; y < MapSizeY; y++)
        {
            for (int x = 0; x < MapSizeX; x++)
            {
                if (this.mapData[y, x].GetType() != TileType.None)
                {
                    switch (this.mapData[y, x].GetType())
                    {
                        case TileType.Wall:
                            tilemap_walls.SetTile(new Vector3Int(x, y, 0), wall);
                            break;
                        /**/
                        case TileType.Room:
                            floor_tilemap.SetTile(new Vector3Int(x, y, 0), room);
                            break;
                        /**/
                        case TileType.Pass:
                            floor_tilemap.SetTile(new Vector3Int(x, y, 0), pass);
                            break;
                    }
                }
                CreateCircumscribedWall(x, y);
            }
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
        tilemap_walls.SetTile(new Vector3Int(x, y, 0), wall);
    }

    private void MapDelete()
    {
        tilemap_walls.ClearAllTiles();
        floor_tilemap.ClearAllTiles();
    }

    /**
     * プレイヤースポーン
     */
    private void SpawnPlayer()
    {
        Position spawn;
        do
        {
            spawn = new Position(RogueUtils.GetRandomInt(0, MapSizeX - 1), RogueUtils.GetRandomInt(0, MapSizeY - 1));
        } while (this.mapData[spawn.Y, spawn.X].GetType() != TileType.Room);

        this.playerSpawnPoint = spawn;
        this.playerSpawn = new Vector3(spawn.X + 0.5f, spawn.Y + 0.5f, 0);
    }

    /// <summary>
    /// プレイヤースポーンポイントを取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerSpawnPoint()
    {
        return this.playerSpawn;
    }

    /**
     * 次の階層への階段スポーン
     */
    private void SpawnNextFloor()
    {
        Position spawn;
        do
        {
            spawn = new Position(RogueUtils.GetRandomInt(0, MapSizeX - 1), RogueUtils.GetRandomInt(0, MapSizeY - 1));
        } while ((this.mapData[spawn.Y, spawn.X].GetType() != TileType.Room) || (spawn == this.playerSpawnPoint));

        this.nextFloorSpawnPoint = spawn;
        this.nextFloorSpawn = new Vector3(spawn.X + 0.5f, spawn.Y + 0.5f, 0);
    }

    /// <summary>
    /// 階段のスポーンポイントを取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextFloorSpawnPoint()
    {
        return this.nextFloorSpawn;
    }

    /**
     * 敵スポーン
     */
    private void SpawnEnemies()
    {
        if (enemySpawnList == null)
        {
            enemySpawnList = new List<Vector3>();
        }
        if (enemySpawnPointList == null)
        {
            enemySpawnPointList = new List<Position>();
        }
        enemySpawnList.Clear();
        enemySpawnPointList.Clear();

        // スポーンさせるEnemyの数
        int num = RogueUtils.GetRandomInt(enemyNumMin, enemyNumMax);
        for (int i = 0; i < num; i++)
        {
            Position spawn;
            while (true)
            {
                spawn = new Position(RogueUtils.GetRandomInt(0, MapSizeX - 1), RogueUtils.GetRandomInt(0, MapSizeY - 1));
                // 敵同士スポーンポイントの被りなく
                // プレイヤースポーンと被らず
                // 部屋か通路に湧く
                if (!enemySpawnPointList.Contains(spawn) &&
                    (!spawn.Equals(this.playerSpawnPoint)) &&
                    ((this.mapData[spawn.Y, spawn.X].GetType() == TileType.Room) ||
                    (this.mapData[spawn.Y, spawn.X].GetType() == TileType.Pass)))
                {
                    enemySpawnPointList.Add(spawn);
                    enemySpawnList.Add(new Vector3(spawn.X + 0.5f, spawn.Y + 0.5f, 0));
                    break;
                }
            }

        }
    }

    /// <summary>
    /// 敵スポーンポイントのリストを取得
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetEnemySpawnPointList()
    {
        return enemySpawnList;
    }

    /**
     * アイテムスポーン
     */
    private void SpawnItems()
    {

    }
}
