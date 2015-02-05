using UnityEngine;
using System.Collections;

public class Room {
	public int x;
	public int y;
	public int width;
	public int height;
	public bool isConnected;

	public int maxX {
		get 
		{
			return x+width; 
		}
	}

	
	public int maxY {
		get 
		{
			return y+height; 
		}
	}

	public int centerX {
		get
		{
			return x + width/2;
		}
	}

	public int centerY {
		get
		{
			return y + height/2;
		}
	}

	public Room(int x, int y, int width, int height) {
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.isConnected = false;
	}

	public bool innerRoomCollidesWith(Room r) {
		if(maxX-1 <= r.x || x >= r.maxX-1 || maxY-1 <= r.y || y >= r.maxY-1) {
			return false;
		}

		return true;
	}

}
