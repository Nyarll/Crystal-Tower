using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Wall,   // �ǂ̃^�C��
    Room,   // �����̃^�C��
    Pass    // �ʘH�̃^�C��
}

public class Tile
{
    private TileType type;
    private Position position;

    public Tile(TileType type, Position position)
    {
        this.type = type;
        this.position = new Position(position.X, position.Y);
    }

    public void SetType(TileType type)
    {
        this.type = type;
    }

    public TileType GetType()
    {
        return this.type;
    }

    public void SetPosition(int x, int y)
    {
        this.position = new Position(x, y);
    }

    public void SetPosition(Position pos)
    {
        this.position = new Position(pos.X, pos.Y);
    }

    public Position GetPosition()
    {
        return this.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
