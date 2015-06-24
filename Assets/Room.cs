using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {
	public int x;
	public int y;
	public int width;
	public int height;

	public List<Room> connectedTo;

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

		connectedTo = new List<Room> ();
	}

	public bool innerRoomCollidesWith(Room r) {
		if(maxX-1 <= r.x || x >= r.maxX-1 || maxY-1 <= r.y || y >= r.maxY-1) {
			return false;
		}

		return true;
	}

	public void addConnection(Room r) {
		if (connectedTo.IndexOf (r) == -1) {
			connectedTo.Add(r);
		}
	}

	public override string ToString()
	{
		return "( x: " + this.x + ", y: " + this.y + ", width: " + this.width + ", height: " + this.height + " )";
	}
}
