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

		for (int i = 0; i < 10; i++) {
			int roomWidth = Random.Range(4, 8);
			int roomHeight = Random.Range(4, 8);
			/* Add 1 because Random.Range() for ints excludes the max value */
			int roomX = Random.Range(0, width - roomWidth + 1);
			int roomY = Random.Range(0, height - roomHeight + 1);
			
			Rect r = new Rect (roomX, roomY, roomWidth, roomHeight);
			createRoom (r);
		}
	}

	private void createRoom(Rect r) {
		for(int x = 0; x < r.width; x++) {
			for(int y = 0; y < r.height; y++) {
				int i = x + (int) r.x;
				int j = y + (int) r.y;

				if(x == 0 || x == r.width-1 || y == 0 || y == r.height-1) {
					map[i, j] = 2;
				} else {
					map[i, j] = 1;
				}
			}
		}
	}
}
