using UnityEngine;
using System.Collections;

public class TileMapInteraction : MonoBehaviour {
	public Transform selectionCube;

	private TileMap tileMap;

	void Start() {
		tileMap = GetComponent<TileMap> ();

		selectionCube.localScale = new Vector3 (tileMap.tileSize, tileMap.tileSize, Mathf.Max(2*tileMap.halfMapDepth, 0.01f));
	}

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hitInfo;
		if (GetComponent<Collider>().Raycast (ray, out hitInfo, Mathf.Infinity)) {
			selectionCube.gameObject.SetActive(true);

			Vector3 tilePos = transform.InverseTransformPoint(hitInfo.point);
			tilePos /= tileMap.tileSize;

			tilePos.x = Mathf.Floor(tilePos.x) * tileMap.tileSize;
			tilePos.y = Mathf.Floor(tilePos.y) * tileMap.tileSize;
			tilePos.z = 0;

			selectionCube.position = tilePos;
		} else {
			selectionCube.gameObject.SetActive(false);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			tileMap.buildMap();
		}
	}

}
