using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
	// 最小エリアサイズ
	private const int MINIMUM_AREA_SIZE = 10;

	// マップサイズ
	private int mapWidth;
	private int mapHeight;

	// 最大部屋数
	int maxRoomNum;

	// マップデータ本体
	Tile[,] map;

	// エリア
	List<Range> areaList = new List<Range>();
	// 部屋
	List<Range> roomList = new List<Range>();
	// 通路
	List<Range> passList = new List<Range>();
	// 部屋が存在するエリアを記憶する
	List<int> areaWhereRoomExists = new List<int>();

	TileType fillType = TileType.None;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <param name="roomNum"></param>
	public MapGenerator(int width, int height, int roomNum)
    {
		this.mapWidth = width;
		this.mapHeight = height;
		this.maxRoomNum = roomNum;
		this.map = new Tile[width, height];
    }

	/// <summary>
	/// マップデータ取得
	/// </summary>
	/// <returns></returns>
	public Tile[,] GetMapData()
    {
		return this.map;
    }

	/// <summary>
	/// ルーム情報取得
	/// </summary>
	/// <returns></returns>
	public List<Range> GetRoomData()
    {
		return this.roomList;
    }

	/// <summary>
	/// マップ生成エントリ
	/// </summary>
	public void Generate()
    {
		_generate();
    }

	/// <summary>
	/// マップ生成
	/// </summary>
	private void _generate()
    {
		_mapInitialize();
		_createArea();
		_createRoom();
		_createPass();
		_reflectListIntoMap();
		_adjacentTileOnlyWall();
	}

	/// <summary>
	/// マップ初期化
	/// </summary>
	private void _mapInitialize()
    {
		_mapDelete();
		for (int y = 0; y < this.mapHeight; y++)
        {
			for (int x = 0; x < this.mapWidth; x++)
            {
				map[y, x] = new Tile(fillType, new Position(x, y));
            }
        }
    }

	private void _mapDelete()
    {
		roomList.Clear();
		areaList.Clear();
		passList.Clear();
		areaWhereRoomExists.Clear();
    }

	/// <summary>
	/// エリア作成
	/// </summary>
	private void _createArea()
    {
		this.areaList.Add(new Range(0, 0, this.mapWidth - 1, this.mapHeight - 1));
		bool isDevided = true;
		while (isDevided)
        {
			isDevided = _splitArea(false);
			isDevided = _splitArea(true) || isDevided;
			if (this.areaList.Count >= this.maxRoomNum)
            {
				break;
            }
		}
    }

	private bool _splitArea(bool isVertical)
    {
		bool isDevided = isVertical;
		List<Range> newAreaList = new List<Range>();
		foreach (Range area in areaList)
        {
			if (isVertical && area.GetWidthY() < MINIMUM_AREA_SIZE * 2 + 1)
            {
				continue;
            }
			else if (!isVertical && area.GetWidthX() < MINIMUM_AREA_SIZE * 2 + 1)
            {
				continue;
            }
			System.Threading.Thread.Sleep(1);
			if (areaList.Count > 1 && RogueUtils.RandomJadge(0.4f))
            {
				continue;
            }

			int length = isVertical ? area.GetWidthY() : area.GetWidthX();
			int margin = length - MINIMUM_AREA_SIZE * 2;
			int base_index = isVertical ? area.Start.Y : area.Start.X;
			int devide_index = base_index + MINIMUM_AREA_SIZE + RogueUtils.GetRandomInt(1, margin) - 1;
			Range new_area = new Range();
			if (isVertical)
            {
				new_area = new Range(area.Start.X, devide_index + 1, area.End.X, area.End.Y);
				area.End.Y = devide_index - 1;
            }
			else
            {
				new_area = new Range(devide_index + 1, area.Start.Y, area.End.X, area.End.Y);
				area.End.X = devide_index - 1;
            }
			newAreaList.Add(new_area);
			isDevided = true;
        }
		areaList.AddRange(newAreaList);
		return isDevided;
    }

	/// <summary>
	/// 部屋作成
	/// </summary>
	private void _createRoom()
    {
		areaList.Sort((a, b) => RogueUtils.GetRandomInt(0, 1) - 1);
		for (int i = 0; i < areaList.Count; i++)
        {
			System.Threading.Thread.Sleep(1);
			if (roomList.Count > maxRoomNum / 2 && RogueUtils.RandomJadge(0.3f))
            {
				continue;
            }
			Range area = areaList[i];

			int marginX = area.GetWidthX() - MINIMUM_AREA_SIZE + 1;
			int marginY = area.GetWidthY() - MINIMUM_AREA_SIZE + 1;
			int randomX = RogueUtils.GetRandomInt(1, marginX);
			int randomY = RogueUtils.GetRandomInt(1, marginY);
			int startX = area.Start.X + randomX;
			int startY = area.Start.Y + randomY;
			int endX = area.End.X - RogueUtils.GetRandomInt(0, (marginX - randomX)) - 1;
			int endY = area.End.Y - RogueUtils.GetRandomInt(0, (marginY - randomY)) - 1;

			Range room = new Range(startX, startY, endX, endY);
			roomList.Add(room);
			areaWhereRoomExists.Add(i);
		}
    }

	/// <summary>
	/// 通路作成
	/// </summary>
	private void _createPass()
    {
		_extendPassFromRoom();
		_connectPass();
    }

	/// <summary>
	/// 部屋から通路を伸ばす
	/// </summary>
	private void _extendPassFromRoom()
    {
		int count = 0;
		for (int i = 0; i < areaList.Count; i++)
        {
			// 部屋の存在しないエリアはスキップ
			if (!areaWhereRoomExists.Contains(i))
            {
				continue;
            }

			Range room = roomList[count];
			// 部屋内のランダムな点を取る
			int randomX = RogueUtils.GetRandomInt(room.Start.X, room.End.X);
			int randomY = RogueUtils.GetRandomInt(room.Start.Y, room.End.Y);

			int startX = randomX;
			int startY = randomY;
			int endX = randomX;
			int endY = randomY;

			if (areaList[i].End.X < mapWidth - 1)
            {
				int targetX = areaList[i].End.X + 1;
				Range pass = new Range(startX, startY, targetX, endY);
				passList.Add(pass);
            }
			if (areaList[i].End.Y < mapHeight - 1)
            {
				int targetY = areaList[i].End.Y + 1;
				Range pass = new Range(startX, startY, endX, targetY);
				passList.Add(pass);
			}
			if (areaList[i].Start.X > 0)
			{
				int targetX = areaList[i].Start.X - 1;
				Range pass = new Range(targetX, startY, endX, endY);
				passList.Add(pass);
			}
			if (areaList[i].Start.Y > 0)
            {
				int targetY = areaList[i].Start.Y - 1;
				Range pass = new Range(startX, targetY, endX, endY);
				passList.Add(pass);
			}
			count++;
		}
    }

	/// <summary>
	/// 伸ばした通路を接続する
	/// </summary>
	private void _connectPass()
    {
		int nowPassSize = passList.Count;
		for (int i = 0; i < nowPassSize; i++)
        {
			for (int k = 0; k < nowPassSize; k++)
            {
				if (i == k)
                {
					continue;
                }
				if (passList[i].Start.Equals(passList[k].Start) ||
					passList[i].Start.Equals(passList[k].End) ||
					passList[i].End.Equals(passList[k].Start) ||
					passList[i].End.Equals(passList[k].End))
                {
					continue;
                }
				__connectPass(i, k);
            }
        }
    }

	private void __connectPass(int i, int k)
    {
		Range v1 = passList[i];
		Range v2 = passList[k];
		Range pass = new Range();
		{
			if (v1.Start.X == v2.Start.X)
			{
				if (v1.Start.Y < v2.Start.Y)
				{
					pass = new Range(v1.Start, v2.Start);
					passList.Add(pass);
					return;
				}
				if (v1.Start.Y > v2.Start.Y)
				{
					pass = new Range(v2.Start, v1.Start);
					passList.Add(pass);
					return;
				}
			}
			if (v1.Start.X == v2.End.X)
			{
				if (v1.Start.Y < v2.End.Y)
				{
					pass = new Range(v1.Start, v2.End);
					passList.Add(pass);
					return;
				}
				if (v1.Start.Y > v2.End.Y)
				{
					pass = new Range(v2.End, v1.Start);
					passList.Add(pass);
					return;
				}
			}
			if (v1.End.X == v2.End.X)
			{
				if (v1.End.Y < v2.End.Y)
				{
					pass = new Range(v1.End, v2.End);
					passList.Add(pass);
					return;
				}
				if (v1.End.Y > v2.End.Y)
				{
					pass = new Range(v2.End, v1.End);
					passList.Add(pass);
					return;
				}
			}
		}
		{
			if (v1.Start.Y == v2.Start.Y)
			{
				if (v1.Start.X < v2.Start.X)
				{
					pass = new Range(v1.Start, v2.Start);
					passList.Add(pass);
					return;
				}
				if (v1.Start.X > v2.Start.X)
				{
					pass = new Range(v2.Start, v1.Start);
					passList.Add(pass);
					return;
				}
			}
			if (v1.Start.Y == v2.End.Y)
			{
				if (v1.Start.X < v2.End.X)
				{
					pass = new Range(v1.Start, v2.End);
					passList.Add(pass);
					return;
				}
				if (v1.Start.X > v2.End.X)
				{
					pass = new Range(v2.End, v1.Start);
					passList.Add(pass);
					return;
				}
			}
			if (v1.End.Y == v2.End.Y)
			{
				if (v1.End.X < v2.End.X)
				{
					pass = new Range(v1.End, v2.End);
					passList.Add(pass);
					return;
				}
				if (v1.End.X > v2.End.X)
				{
					pass = new Range(v2.End, v1.End);
					passList.Add(pass);
					return;
				}
			}
		}
	}

	/// <summary>
	/// 作成した情報をマップに反映する
	/// </summary>
	private void _reflectListIntoMap()
    {
		// 通路から反映する
		foreach (Range pass in passList)
        {
			for (int y = pass.Start.Y; y <= pass.End.Y; y++)
            {
				for (int x = pass.Start.X; x <= pass.End.X; x++)
                {
					map[y, x].SetType(TileType.Pass);
                }
            }
        }
		foreach (Range room in roomList)
		{
			try
			{
				if (RogueUtils.GetRandomInt(0, 1) == 1)
				{
					for (int y = room.Start.Y; y <= room.End.Y; y++)
					{
						for (int x = room.Start.X; x <= room.End.X; x++)
						{
							map[y, x].SetType(TileType.Room);
						}
					}
				}
				else
				{
					_createCircleRoom(room);
				}
			}
			catch (Exception e)
            {
				for (int y = room.Start.Y; y <= room.End.Y; y++)
				{
					for (int x = room.Start.X; x <= room.End.X; x++)
					{
						map[y, x].SetType(TileType.Room);
					}
				}
			}
		}
	}

	private void _createCircleRoom(Range room)
    {
		int radius = (room.GetWidthY() / 2);
		Position center = new Position(room.Start.X + (room.GetWidthX() / 2) + 1, room.Start.Y + radius + 1);
		if (room.GetWidthX() < room.GetWidthY())
		{
			radius = (room.GetWidthX() / 2);
			center = new Position(room.Start.X + radius + 1, room.Start.Y + (room.GetWidthY() / 2) + 1);
		}
		{
			int x = radius;
			int y = 0;
			int F = -2 * radius + 3;
			while (x >= y)
			{
				map[center.Y + y, center.X + x].SetType(TileType.Room);
				map[center.Y + y, center.X - x].SetType(TileType.Room);
				map[center.Y - y, center.X + x].SetType(TileType.Room);
				map[center.Y - y, center.X - x].SetType(TileType.Room);

				map[center.Y + x, center.X + y].SetType(TileType.Room);
				map[center.Y + x, center.X - y].SetType(TileType.Room);
				map[center.Y - x, center.X + y].SetType(TileType.Room);
				map[center.Y - x, center.X - y].SetType(TileType.Room);

				if (F >= 0)
				{
					x--;
					F -= 4 * x;
				}
				y++;
				F += 4 * y + 2;
			}
		}
		for (int y = room.Start.Y; y < room.End.Y + 2; y++)
		{
			for (int x = room.Start.X; x < room.End.X + 2; x++)
			{
				int lx = (x - center.X) * (x - center.X);
				int ly = (y - center.Y) * (y - center.Y);
				int lr = radius * radius;
				if (lx + ly < lr)
				{
					map[y, x].SetType(TileType.Room);
				}
			}
		}
	}

	private void _adjacentTileOnlyWall()
    {
		for (int y = 1; y < mapHeight - 1; y++)
		{
			for (int x = 1; x < mapWidth - 1; x++)
			{
				if (map[y, x].GetType() != TileType.None && map[y, x].GetType() != TileType.Wall)
				{
					// 上下左右
					if (map[y, x - 1].GetType() == TileType.None)
					{
						map[y, x - 1].SetType(TileType.Wall);
					}
					if (map[y, x + 1].GetType() == TileType.None)
					{
						map[y, x + 1].SetType(TileType.Wall);
					}
					if (map[y - 1, x].GetType() == TileType.None)
					{
						map[y - 1, x].SetType(TileType.Wall);
					}
					if (map[y + 1, x].GetType() == TileType.None)
					{
						map[y + 1, x].SetType(TileType.Wall);
					}
					// 四隅
					if (map[y - 1, x - 1].GetType() == TileType.None)
					{
						map[y - 1, x - 1].SetType(TileType.Wall);
					}
					if (map[y + 1, x - 1].GetType() == TileType.None)
					{
						map[y + 1, x - 1].SetType(TileType.Wall);
					}
					if (map[y - 1, x + 1].GetType() == TileType.None)
					{
						map[y - 1, x + 1].SetType(TileType.Wall);
					}
					if (map[y + 1, x + 1].GetType() == TileType.None)
					{
						map[y + 1, x + 1].SetType(TileType.Wall);
					}
				}
			}
		}
	}
}
