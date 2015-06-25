using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapBuilder {
	public int mapWidth;
	public int mapHeight;
	public int numberOfRooms;
	public bool overlappingRooms;
	public bool mirrorMap;
	public int[] roomWidthRange;
	public int[] roomHeightRange;

	private MapData map;
	private List<Room> rooms;

	public MapBuilder() {
		this.mapWidth = 30;
		this.mapHeight = 30;
		this.numberOfRooms = 20;
		this.overlappingRooms = false;
		this.mirrorMap = false;
		this.roomWidthRange = new [] {4, 8};
		this.roomHeightRange = new [] {4, 8};
	}

	public MapData build() {
		map = new MapData (mapWidth, mapHeight);
		
		rooms = new List<Room>();
		
		// Randomly create the rooms
		for (int i = 0; i < numberOfRooms; i++) {
			int roomWidth = Random.Range(roomWidthRange[0], roomWidthRange[1]);
			int roomHeight = Random.Range(roomHeightRange[0], roomHeightRange[1]);
			
			/* Add 1 because Random.Range() for ints excludes the max value */
			int roomX;
			if(mirrorMap) {
				roomX = Random.Range(0, map.width/2 - roomWidth/2);
			} else {
				roomX = Random.Range(0, map.width - roomWidth + 1);
			}
			
			int roomY = Random.Range(0, map.height - roomHeight + 1);
			
			Room r = new Room (roomX, roomY, roomWidth, roomHeight);
			
			if(overlappingRooms || !roomCollides(r)) {
				createRoom (r);
			}
		}
		
		// Randomly connect the rooms
		for (int i = 0; i < rooms.Count; i++) {
			int j = i + Random.Range(1, rooms.Count);
			j %= rooms.Count;
			
			createHallway (rooms [i], rooms [j]);
		}
		
		// Make sure there are no isolated groups of rooms
		connectAllRooms ();
		
		if (mirrorMap) {
			for (int x = 0; x < map.width/2; x++) {
				for (int y = 0; y < map.height; y++) {
					map [map.width - 1 - x, y] = map [x, y];
				}
			}
		}

		return map;
	}
	
	public bool roomCollides(Room r) {
		foreach (Room r2 in rooms) {
			if(r.innerRoomCollidesWith(r2)) {
				return true;
			}
		}
		
		return false;
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
		
		rooms.Add (r);
	}
	
	private void createHallway(Room r1, Room r2) {
		int x = r1.centerX;
		int y = r1.centerY;
		
		int dx = (x < r2.centerX) ? 1 : -1;
		int dy = (y < r2.centerY) ? 1 : -1;
		
		while (x != r2.centerX) {
			setHallwayTile(x, y);
			x += dx;
		}
		
		while (y != r2.centerY) {
			setHallwayTile(x, y);
			y += dy;
		}
		
		r1.addConnection (r2);
		r2.addConnection (r1);
	}
	
	private void setHallwayTile(int x, int y) {
		map[x, y] = 1;
		
		if (x > 0 && map [x - 1, y] == 0) {
			map [x - 1, y] = 2;
		}
		
		if (x + 1 < map.width && map [x + 1, y] == 0) {
			map [x + 1, y] = 2;
		}
		
		if (y > 0 && map [x, y - 1] == 0) {
			map [x, y - 1] = 2;
		}
		
		if (y + 1 < map.height && map [x, y + 1] == 0) {
			map [x, y + 1] = 2;
		}
		
		if (x > 0 && y > 0 && map [x - 1, y - 1] == 0) {
			map [x - 1, y - 1] = 2;
		}
		
		if (x + 1 < map.width && y > 0 && map [x + 1, y - 1] == 0) {
			map [x + 1, y - 1] = 2;
		}
		
		if (x > 0 && y + 1 < map.height && map [x - 1, y + 1] == 0) {
			map [x - 1, y + 1] = 2;
		}
		
		if (x + 1 < map.width && y + 1 < map.height && map [x + 1, y + 1] == 0) {
			map [x + 1, y + 1] = 2;
		}
	}
	
	private void connectAllRooms() {
		List<List<Room>> connectedRooms = new List<List<Room>>();
		
		HashSet<Room> visited = new HashSet<Room> ();
		
		for(int i = 0; i < rooms.Count; i++) {
			if(!visited.Contains(rooms[i])) {
				connectedRooms.Add(getConnectedRooms(rooms[i], visited));
			}
		}
		
		if (connectedRooms.Count > 1) {
			for(int i = 0; i < connectedRooms.Count-1; i++) {
				Room r1 = connectedRooms[i][0];
				Room r2 = connectedRooms[i+1][0];
				
				createHallway(r1, r2);
			}
		}
	}
	
	private List<Room> getConnectedRooms(Room startRoom, HashSet<Room> visited) {
		List<Room> connectedRooms = new List<Room> ();
		
		List<Room> list = new List<Room> ();
		list.Add (startRoom);
		
		while (list.Count > 0) {
			Room r = list[list.Count-1];
			list.RemoveAt(list.Count-1);
			
			if(!visited.Contains(r)) {
				visited.Add(r);
				connectedRooms.Add(r);
				
				foreach(Room c in r.connectedTo) {
					if(!visited.Contains(c)) {
						list.Add(c);
					}
				}
			}
		}
		
		return connectedRooms;
	}
}
