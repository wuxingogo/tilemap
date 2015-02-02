﻿using UnityEngine;
using System.Collections;

public class TileMapMouse : MonoBehaviour {
	public Transform selectionCube;

	private TileMap tileMap;

	void Start() {
		tileMap = GetComponent<TileMap> ();



		selectionCube.localScale = new Vector3 (tileMap.tileSize, Mathf.Max(2*tileMap.halfMapDepth, 0.01f), tileMap.tileSize);
	}

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hitInfo;
		if (collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			selectionCube.gameObject.SetActive(true);

			Vector3 tilePos = transform.InverseTransformPoint(hitInfo.point);
			tilePos /= tileMap.tileSize;

			tilePos.x = Mathf.Floor(tilePos.x) * tileMap.tileSize;
			tilePos.y = 0;
			tilePos.z = Mathf.Floor(tilePos.z) * tileMap.tileSize;

			selectionCube.position = tilePos;
		} else {
			selectionCube.gameObject.SetActive(false);
		}
	}
}
