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

    // ��
    [SerializeField]
    GameObject floorPrefab;

    // �K�i
    [SerializeField]
    GameObject nextFloorObject;

    private int[,] mapData;

    private Position playerPosition;
    private Position nextFloorPosition;

    private MapGenerator generator = null;

    // Start is called before the first frame update
    void Start()
    {
        Generate();   
    }

    public void Generate()
    {
        if (this.generator == null)
        {
            this.generator = new MapGenerator();
        }

        this.MapDelete();
        this.GenerateMap();
        this.SpawnPlayer();
        this.SpawnNextFloor();
    }

    private void GenerateMap()
    {
        this.mapData = this.generator.GenerateMap(MapSizeX, MapSizeY, MaxRoom);

        List<Vector3> floorList = new List<Vector3>();

        for (int y = 0; y < this.MapSizeY; y++)
        {
            for (int x = 0; x < this.MapSizeX; x++)
            {
                // �u1�v�̉ӏ��ɏ�����
                if (mapData[x, y] == 1)
                {
                    GameObject obj = Instantiate(floorPrefab, new Vector3(x, y, 0), new Quaternion());
                    obj.transform.parent = this.transform;
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

    }

    /**
     * ���̊K�w�ւ̊K�i�X�|�[��
     */
    private void SpawnNextFloor()
    {

    }
}
