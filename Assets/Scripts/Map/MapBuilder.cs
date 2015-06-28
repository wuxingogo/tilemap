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
	public int[] innerHallwayWidthRange;

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
		this.roomHeightRange = new [] {1, 1};
	}

	public MapData build() {
		map = new MapData (mapWidth, mapHeight);
		
		rooms = new List<Room>();
		
		// Randomly create the rooms
		for (int i = 0; i < numberOfRooms; i++) {
			/* Add 1 because Random.Range() for ints excludes the max value */
			int roomWidth = Random.Range(roomWidthRange[0], roomWidthRange[1] + 1);
			int roomHeight = Random.Range(roomHeightRange[0], roomHeightRange[1] + 1);

			int roomX;
			if(mirrorMap) {
				roomX = Random.Range(0, (map.width/2) - (roomWidth/2));
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

		verifyBorderWalls ();
		
		if (mirrorMap) {
			for (int x = 0; x < map.width/2; x++) {
				for (int y = 0; y < map.height; y++) {
					map [map.width - 1 - x, y] = map [x, y];
				}
			}

			if(!map.isConnectedMap(1)) {
				connectMirroredMap ();
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
		int dx = (r1.centerX < r2.centerX) ? 1 : -1;
		int dy = (r1.centerY < r2.centerY) ? 1 : -1;

		int innerWidth = Random.Range (innerHallwayWidthRange [0], innerHallwayWidthRange [1] + 1);

		int width1 = -1 * innerWidth / 2;
		int width2 = innerWidth / 2;
		if (innerWidth % 2 == 1) {
			width2 += 1;
		}

		for (int y = r1.centerY + width1; y < r1.centerY + width2; y++) {
			for (int x = r1.centerX; x != r2.centerX; x += dx) {
				setHallwayTile(x, y);
			}
		}

		for (int x = r2.centerX + width1; x < r2.centerX + width2; x++) {
			for (int y = r1.centerY; y != r2.centerY; y += dy) {
				setHallwayTile(x, y);
			}
		}
		
		r1.addConnection (r2);
		r2.addConnection (r1);
	}
	
	private void setHallwayTile(int x, int y) {
		if (x > 0 && x < map.width && y > 0 && y < map.height) {
			map [x, y] = 1;

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

	/** 
	 * Make sure any hallways against the borders of the map are fully 
	 * enclosed in wall times (even if that means we have to shave off 
	 * some tiles from the hallway 
	 */
	private void verifyBorderWalls() {
		for (int x = 0; x < map.width; x++) {
			setBorderWall(x, 0);
			setBorderWall(x, map.height-1);
		}

		for (int y = 0; y < map.height; y++) {
			setBorderWall(0, y);
			setBorderWall(map.width-1, y);
		}
	}

	private void setBorderWall(int x, int y) {
		if (map [x, y] == 1) {
			map[x, y] = 2;
		}
	}

	private void connectMirroredMap ()
	{
		int currentMaxX = -1;
		Room currentRoom = null;
		for (int i = 0; i < rooms.Count; i++) {
			if (rooms [i].maxX > currentMaxX) {
				currentMaxX = rooms [i].maxX;
				currentRoom = rooms [i];
			}
		}
		int connectionWidth = map.width - 2 * currentRoom.x;
		for (int x = 1; x < connectionWidth - 1; x++) {
			for (int y = 1; y < currentRoom.height - 1; y++) {
				setHallwayTile (x + currentRoom.x, y + currentRoom.y);
			}
		}
	}
}
