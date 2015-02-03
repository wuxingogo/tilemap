using UnityEngine;
using System.Collections;

public class MapData {
	int[,] map;

	// Indexer declaration.
	public int this[int x, int z]
	{
		get
		{
			return map[x, z];
		}
		
		set
		{
			map[x, z] = value;
		}
	}

	public MapData(int width, int height) {
		map = new int[width,height];

		for (int z = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				map[x,z] = Random.Range(0,4);
			}
		}
	}
}
