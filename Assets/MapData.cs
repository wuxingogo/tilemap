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
	}
}
