using UnityEngine;
using System.Collections;

public class MapData {
	public int width;
	public int height;
	private int[,] map;

	// Indexer declaration.
	public int this[int x, int y]
	{
		get
		{
			return map[x, y];
		}
		
		set
		{
			map[x, y] = value;
		}
	}

	public MapData(int width, int height) {
		this.width = width;
		this.height = height;
		map = new int[width,height];

		for (int i = 0; i < 20; i++) {
			int roomWidth = Random.Range(4, 8);
			int roomHeight = Random.Range(4, 8);
			/* Add 1 because Random.Range() for ints excludes the max value */
			int roomX = Random.Range(0, width - roomWidth + 1);
			int roomY = Random.Range(0, height - roomHeight + 1);
			
			Room r = new Room (roomX, roomY, roomWidth, roomHeight);
			createRoom (r);
		}
	}

	private void createRoom(Room r) {
		for(int x = 0; x < r.width; x++) {
			for(int y = 0; y < r.height; y++) {
				if(x == 0 || x == r.width-1 || y == 0 || y == r.height-1) {
					map[x + r.x, y + r.y] = 2;
				} else {
					map[x + r.x, y + r.y] = 1;
				}
			}
		}
	}
}
