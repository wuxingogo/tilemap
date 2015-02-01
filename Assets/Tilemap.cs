using UnityEngine;
using System.Collections;

public class Tilemap : MonoBehaviour {
	public int mapWidth = 3;
	public int mapHeight = 2;
	public float tileSize = 1.0f;

	// Use this for initialization
	void Start () {
		buildMesh ();
	}

	private void buildMesh() {
		int numTiles = mapWidth * mapWidth;
		int numTris = numTiles * 2;

		int numVertsX = mapWidth + 1;
		int numVertsZ = mapHeight + 1;
		int numVerts = numVertsX * numVertsZ;

		Mesh mesh = new Mesh ();

		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];


		for (int z = 0; z < numVertsZ; z++) {
			for(int x = 0; x < numVertsX; x++) {
				int i = z * numVertsX + x;

				vertices[i] = new Vector3(x * tileSize, 0, -1 * z * tileSize);
				normals[i] = Vector3.up;
				/* Unity uv orgin is bottom left (I'm using a vertex origin of top left
				 * so flip the y value by subtracting by one) */
				uv[i] = new Vector2((float)x/mapWidth, 1 - (float)z/mapHeight);
			}
		}

		int[] triangles = new int[numTris * 3];

		for (int z = 0; z < mapHeight; z++) {
			for (int x = 0; x < mapWidth; x++) {
				int i = (z * mapWidth + x) * 2 * 3;
				int j = z * numVertsX + x;

				triangles [i] = j;
				triangles [i + 1] = j + 1;
				triangles [i + 2] = j + numVertsX + 1;

				triangles [i + 3] = j;
				triangles [i + 4] = j + numVertsX + 1;
				triangles [i + 5] = j + numVertsX;
			}
		}

		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uv;
		mesh.triangles = triangles;

		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		meshFilter.mesh = mesh;
	}
}
