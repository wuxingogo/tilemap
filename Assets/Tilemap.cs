using UnityEngine;
using System.Collections;

public class Tilemap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		buildMesh ();
	}

	private void buildMesh() {
		int numTris = 2;

		Mesh mesh = new Mesh ();

		Vector3[] vertices = new Vector3[4];
		vertices [0] = new Vector3 (0, 0, 0);
		vertices [1] = new Vector3 (1, 0, 0);
		vertices [2] = new Vector3 (1, 0, -1);
		vertices [3] = new Vector3 (0, 0, -1);

		Vector3[] normals = new Vector3[4];
		normals [0] = Vector3.up;
		normals [1] = Vector3.up;
		normals [2] = Vector3.up;
		normals [3] = Vector3.up;

		/* Unity uv orgin is bottom left (I'm using a vertex origin of top left) */
		Vector2[] uv = new Vector2[4];
		uv [0] = new Vector2 (0, 1);
		uv [1] = new Vector2 (1, 1);
		uv [2] = new Vector2 (1, 0);
		uv [3] = new Vector2 (0, 0);

		int[] triangles = new int[numTris * 3];
		triangles [0] = 0;
		triangles [1] = 1;
		triangles [2] = 2;
		triangles [3] = 0;
		triangles [4] = 2;
		triangles [5] = 3;


		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uv;
		mesh.triangles = triangles;

		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		meshFilter.mesh = mesh;
	}
}
