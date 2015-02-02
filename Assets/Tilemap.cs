using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TileMap : MonoBehaviour {
	public int mapWidth = 3;
	public int mapHeight = 2;
	public float tileSize = 1.0f;
	public float halfMapDepth = 0.25f;

	// Use this for initialization
	void Start () {
		buildMesh ();
	}

	public void buildMesh() {
		int numTiles = mapWidth * mapHeight;
		int numTris = numTiles * 2;

		int numVertsX = 2 * (mapWidth - 1) + 2;
		int numVertsZ = 2 * (mapHeight - 1) + 2;
		int numVerts = numVertsX * numVertsZ;

		Mesh mesh = new Mesh ();

		Vector3[] vertices = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];


		for (int z = 0; z < numVertsZ; z++) {
			for(int x = 0; x < numVertsX; x++) {
				int i = z * numVertsX + x;

				Vector3 vert = new Vector3((x / 2) * tileSize, 0, (z / 2) * tileSize);
				Vector2 texCoord = Vector2.zero;

				if(x % 2 == 1) {
					vert.x += tileSize;
					texCoord.x = 1;
				}

				if(z % 2 == 1) {
					vert.z += tileSize;
					texCoord.y = 1;
				}

				// Set vertex depth
				if(z == 0 || z % 2 == 1) {
					if(x % 2 == 0) {
						vert.y = Random.Range(-halfMapDepth, halfMapDepth);

						if(x != 0) {
							vertices[i-1].y = vert.y;
						}
					}
				} else {
					vert.y = vertices[i-numVertsX].y;
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
	
}
