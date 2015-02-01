using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TileMap : MonoBehaviour {
	public int mapWidth = 3;
	public int mapHeight = 2;
	public float tileSize = 1.0f;

	// Use this for initialization
	void Start () {
		buildMesh ();
	}

	public void buildMesh() {
		int numTiles = mapWidth * mapWidth;
		int numTris = numTiles * 2;

		int numVertsX = mapWidth + 1;
		int numVertsZ = mapHeight + 1;
		int numVerts = numVertsX * numVertsZ;

		Mesh mesh = new Mesh ();

		Vector3[] vertices = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];


		for (int z = 0; z < numVertsZ; z++) {
			for(int x = 0; x < numVertsX; x++) {
				int i = z * numVertsX + x;

				vertices[i] = new Vector3(x * tileSize, Random.Range(-0.25f, 0.25f), z * tileSize);
				uv[i] = new Vector2((float)x/mapWidth, (float)z/mapHeight);
			}
		}

		int[] triangles = new int[numTris * 3];

		for (int z = 0; z < mapHeight; z++) {
			for (int x = 0; x < mapWidth; x++) {
				int i = (z * mapWidth + x) * 2 * 3;
				int j = z * numVertsX + x;

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
