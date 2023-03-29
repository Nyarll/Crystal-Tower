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

    private MapCreator creator;

    // Start is called before the first frame update
    void Start()
    {
        this.creator = this.GetComponent<MapCreator>();
        this.creator.Generate();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeFloor();
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
        foreach (Vector3 p in spawn)
        {
            GameObject obj = Instantiate(enemyPrefab, p, new Quaternion());
            obj.layer = LayerMask.NameToLayer("Entity");
            obj.tag = "Enemy";
            obj.transform.parent = this.transform;
        }
    }

    public void ChangeFloor()
    {
        this.GetComponent<SequenceManager>().ChangeCurrentSequence(Sequence.EndPhase);
        this.GetComponent<MapCreator>().Generate();
        DeleteEnemies();
        Spawn();
        Debug.Log("change floor.");
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
}
