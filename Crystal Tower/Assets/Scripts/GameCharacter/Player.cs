using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    private Tile[,] map;

    private bool isMove = false;

    private Vector2 now_position;
    private Vector2 next_position;

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
        now_position = transform.position;
        if (!isMove)
        {
            this.Move();
        }
        else
        {
            this.onMove();
        }
    }

    private void Move()
    {
        next_position = transform.position;
        move_direction = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move_direction.x -= 1;
            next_position.x -= 1;
            isMove = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            move_direction.x += 1;
            next_position.x += 1;
            isMove = true;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            move_direction.y += 1;
            next_position.y += 1;
            isMove = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            move_direction.y -= 1;
            next_position.y -= 1;
            isMove = true;
        }
    }

    private void onMove()
    {
        // à⁄ìÆêÊÇ™ï«Ç≈Ç»ÇØÇÍÇŒà⁄ìÆÇ∑ÇÈ
        if (map[(int)Mathf.Floor(next_position.x), (int)Mathf.Floor(next_position.y)].GetType() != TileType.Wall)
        {
            if (now_position != next_position)
            {
                now_position += move_direction / MOVING_INTERVAL;
            }
            transform.position = now_position;
            if (transform.position.x == next_position.x && transform.position.y == next_position.y)
            {
                isMove = false;
            }
        }
        else
        {
            isMove = false;
        }
    }
}
