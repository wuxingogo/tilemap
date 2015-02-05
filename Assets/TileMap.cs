using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TileMap : MonoBehaviour {
	public int tileResolution = 64;
	public int mapWidth = 3;
	public int mapHeight = 2;
	public float tileSize = 1.0f;
	public float halfMapDepth = 0.125f;

	public bool overlappingRooms = false;
	public int numberOfRooms = 20;
	[HideInInspector]
	public int[] roomWidthRange = new [] {4, 8};
	[HideInInspector]
	public int[] roomHeightRange = new [] {4, 8};

	private MapData map;
	private List<Room> rooms;

	// Use this for initialization
	void Start () {
		buildMap ();
	}

	public void buildMap() {
		buildMapData ();
		buildMesh ();
	}

	public void buildMesh() {
		int numTiles = mapWidth * mapHeight;
		int numTris = numTiles * 2;

		int numVertsX = 2 * (mapWidth - 1) + 2;
		int numVertsZ = 2 * (mapHeight - 1) + 2;
		int numVerts = numVertsX * numVertsZ;

		float textureStep = (float)tileResolution / renderer.sharedMaterial.mainTexture.width;

		Mesh mesh = new Mesh ();

		Vector3[] vertices = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];

		for (int z = 0; z < numVertsZ; z++) {
			for(int x = 0; x < numVertsX; x++) {
				int i = z * numVertsX + x;

				Vector3 vert = new Vector3((x / 2) * tileSize, 0, (z / 2) * tileSize);
				
				// Set vertex depth
				if(z == 0 || z % 2 == 1) {
					if(x == numVertsX-1 || x % 2 == 0) {
						vert.y = Random.Range(-halfMapDepth, halfMapDepth);
						
						if(x != 0 && x != numVertsX-1 ) {
							vertices[i-1].y = vert.y;
						}
					}
				} else {
					vert.y = vertices[i-numVertsX].y;
				}

				Vector2 texCoord;

				if(x % 2 == 0 && z % 2 == 0) {
					int type = map[x/2, z/2];
					texCoord = new Vector2(type * textureStep, 0);
				} else {
					int tileX = (x/2)*2;
					int tileZ = (z/2)*2;
					int tileIndex = tileZ * numVertsX + tileX;
					texCoord = uv[tileIndex];
				}

				if(x % 2 == 1) {
					vert.x += tileSize;
					texCoord.x += textureStep;
				}

				if(z % 2 == 1) {
					vert.z += tileSize;
					texCoord.y = 1;
				}

				vertices[i] = vert;
				uv[i] = texCoord;
			}
		}

		int[] triangles = new int[numTris * 3];

		for (int z = 0; z < mapHeight; z++) {
			for (int x = 0; x < mapWidth; x++) {
				int i = (z * mapWidth + x) * 2 * 3;
				int j = z*2 * numVertsX + x*2;

				triangles [i] = j;
				triangles [i + 1] = j + numVertsX;
				triangles [i + 2] = j + 1;

				triangles [i + 3] = j + numVertsX;
				triangles [i + 4] = j + numVertsX + 1;
				triangles [i + 5] = j + 1;
			}
		}

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;

		mesh.RecalculateNormals ();

		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		meshFilter.mesh = mesh;

		MeshCollider meshCollider = GetComponent<MeshCollider> ();
		meshCollider.sharedMesh = mesh;
	}

	private void buildMapData() {
		map = new MapData (mapWidth, mapHeight);

		rooms = new List<Room>();
		
		for (int i = 0; i < numberOfRooms; i++) {
			int roomWidth = Random.Range(roomWidthRange[0], roomWidthRange[1]);
			int roomHeight = Random.Range(roomHeightRange[0], roomHeightRange[1]);
			/* Add 1 because Random.Range() for ints excludes the max value */
			int roomX = Random.Range(0, map.width - roomWidth + 1);
			int roomY = Random.Range(0, map.height - roomHeight + 1);
			
			Room r = new Room (roomX, roomY, roomWidth, roomHeight);
			
			if(overlappingRooms || !roomCollides(r)) {
				createRoom (r);
			}
		}

		for (int i = 0; i < rooms.Count; i++) {
			if(!rooms[i].isConnected) {
				int j = i + Random.Range(1, rooms.Count);
				j %= rooms.Count;

				createHallway (rooms [i], rooms [j]);
			}
		}
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
}
