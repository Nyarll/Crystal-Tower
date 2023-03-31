using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject nextFloor;

    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    AutoMapping miniMap;

    private MapCreator creator;

    private UIManager uiManager = null;

    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.uiManager = GetComponent<UIManager>();
        this.creator = this.GetComponent<MapCreator>();
        this.creator.Generate();
        Spawn();

        miniMap.ResetMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeFloor();
        }
        if (Input.GetKey(KeyCode.T))
        {
            uiManager.AddLogText("(" + count + ")ログテスト <color=red>テスト</color> aaa あああ", LogSystem.LogType.All);
            count++;
        }
    }

    private void Spawn()
    {
        SpawnPlayer();
        SpawnNextFloor();
        SpawnEnemies();
    }

    private void SpawnPlayer()
    {
        player.transform.position = this.creator.GetPlayerSpawnPoint();
        
    }

    private void SpawnNextFloor()
    {
        nextFloor.transform.position = this.creator.GetNextFloorSpawnPoint();
    }

    private void SpawnEnemies()
    {
        List<Vector3> spawn = creator.GetEnemySpawnPointList();
        GameObject enemies = new GameObject("enemies");
        enemies.transform.parent = this.transform;
        enemies.transform.position = Vector3.zero;
        foreach (Vector3 p in spawn)
        {
            GameObject obj = Instantiate(enemyPrefab, p, new Quaternion());
            obj.layer = LayerMask.NameToLayer("Entity");
            obj.tag = "Enemy";
            obj.transform.parent = enemies.transform;
        }
    }

    public void ChangeFloor()
    {
        this.GetComponent<SequenceManager>().ChangeCurrentSequence(Sequence.EndPhase);
        this.GetComponent<MapCreator>().Generate();
        DeleteEnemies();
        Spawn();
        miniMap.ResetMap();
    }

    private void DeleteEnemies()
    {
        foreach (Transform obj in gameObject.transform)
        {
            if (obj.gameObject.tag == "Enemy")
            {
                GameObject.Destroy(obj.gameObject);
            }
        }
    }

    public void Mapping(GameObject obj)
    {
        if (obj.tag == "Room")
        {
            miniMap.InRoom(obj.transform.parent.gameObject);
        }
        else if (obj.tag == "Pass")
        {
            miniMap.Mapping((int)obj.transform.position.x, (int)obj.transform.position.y, TileType.Room);
        }
    }
}
