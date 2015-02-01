using UnityEngine;
using System.Collections;

public class TileMapMouse : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hitInfo;
		if (collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			Debug.Log("----------------------------------------------------------");
			Debug.Log (transform.InverseTransformPoint(hitInfo.point));
			Debug.Log (transform.worldToLocalMatrix * new Vector4(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z, 1));
		} else {

		}
	}
}
