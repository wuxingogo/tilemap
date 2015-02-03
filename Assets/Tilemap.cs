﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TileMap : MonoBehaviour {
	public int tileResolution = 64;
	public int mapWidth = 3;
	public int mapHeight = 2;
	public float tileSize = 1.0f;
	public float halfMapDepth = 0.125f;

	// Use this for initialization
	void Start () {
		buildMesh ();
	}

	public void buildMesh() {
		MapData map = new MapData (mapWidth, mapHeight);

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
	
}
