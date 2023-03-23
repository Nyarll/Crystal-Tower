using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject mapCreator;

    private Tile[,] map;

    public void SetMapData(Tile[,] map)
    {
        this.map = map;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.Move();
    }

    private void Move()
    {
        Vector2 next_position = transform.position;
        bool isMove = false;

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            next_position.x -= 1;
            isMove = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            next_position.x += 1;
            isMove = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            next_position.y += 1;
            isMove = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            next_position.y -= 1;
            isMove = true;
        }

        if(isMove)
        {
            if (map[(int)Mathf.Floor(next_position.x), (int)Mathf.Floor(next_position.y)].GetType() != TileType.Wall)
            {
                transform.position = next_position;
            }
        }
    }
}
